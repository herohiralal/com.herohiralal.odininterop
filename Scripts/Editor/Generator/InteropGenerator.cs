using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Reflection;
using System.Linq;

namespace OdinInterop.Editor
{
    internal static class InteropGenerator
    {
        private static readonly string ENGINE_APIS_OUT_DIR = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Packages", "com.herohiralal.odininterop", "Scripts", "Runtime", "Generated"));
        private static readonly string PROJECT_APIS_OUT_DIR = Path.GetFullPath(Path.Combine(Application.dataPath, "OdinInterop"));
        internal static readonly string ODIN_INTEROP_OUT_DIR = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Source", "OdinInterop"));

        private static HashSet<Type> s_ExportedTypes = new HashSet<Type>(256); // to create in odin
        private static HashSet<Type> s_ImportedTypes = new HashSet<Type>(256); // to create interoperable proxies in C#

        [MenuItem("Tools/Odin Interop/Generate Interop Code")]
        private static void GenerateInteropCode()
        {
            s_ExportedTypes.Clear();

            // create a clean odn out dir
            {
                if (!Directory.Exists(ODIN_INTEROP_OUT_DIR))
                {
                    Directory.CreateDirectory(ODIN_INTEROP_OUT_DIR);
                }

                foreach (var file in Directory.GetFiles(ODIN_INTEROP_OUT_DIR, "odntrop_*.odin", SearchOption.TopDirectoryOnly))
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
            }

#if ODIN_INTEROP_REGENERATE_ENGINE_CODE
            if (Directory.Exists(ENGINE_APIS_OUT_DIR))
            {
                Directory.Delete(ENGINE_APIS_OUT_DIR, true);
            }

            Directory.CreateDirectory(ENGINE_APIS_OUT_DIR);
            GenerateInteropCodeInternal(typeof(EngineBindings), true);
#endif

            if (Directory.Exists(PROJECT_APIS_OUT_DIR))
            {
                Directory.Delete(PROJECT_APIS_OUT_DIR, true);
            }

            Directory.CreateDirectory(PROJECT_APIS_OUT_DIR);
            File.WriteAllText(
                Path.Combine(PROJECT_APIS_OUT_DIR, ".gitignore"),
                "*\n!.gitignore\n"
            );

            foreach (var t in TypeCache.GetTypesWithAttribute<GenerateOdinInteropAttribute>())
            {
                // public static partial classes only
                if (!t.IsAbstract || !t.IsSealed)
                {
                    Debug.LogError($"[Odin Interop] Type {t.FullName} is marked with GenerateOdinInteropAttribute but is not a static class. Skipping...");
                    continue;
                }

                if (!t.IsPublic)
                {
                    Debug.LogError($"[Odin Interop] Type {t.FullName} is marked with GenerateOdinInteropAttribute but is not public. Skipping...");
                    continue;
                }

                GenerateInteropCodeInternal(t, false);
            }

            AssetDatabase.Refresh();
        }

        private static StringBuilder s_StrBld = new StringBuilder(16384);
        private static int s_StrBldIndent = 0;
        private static StringBuilder AppendIndent(this StringBuilder sb)
        {
            for (int i = 0; i < s_StrBldIndent; i++)
            {
                sb.Append('\t');
            }

            return sb;
        }

        private static void GenerateInteropCodeInternal(Type t, bool isEngineCode)
        {
            s_ImportedTypes.Clear();

            string asmName = t.Assembly.GetName().Name;
            var asmIsSpl = false;

            string containingDir;
            if (isEngineCode)
            {
                containingDir = ENGINE_APIS_OUT_DIR;
                asmIsSpl = true;
            }
            else
            {
                switch (asmName)
                {
                    case "Assembly-CSharp":
                    case "Assembly-CSharp-Editor":
                        asmIsSpl = true;
                        break;
                }

                containingDir = Path.Combine(PROJECT_APIS_OUT_DIR, asmName);
            }

            Directory.CreateDirectory(containingDir);

            if (!asmIsSpl)
            {
                File.WriteAllText(
                    Path.Combine(containingDir, $"{asmName}.Ref.asmref"),
                    $"{{\"reference\":\"{asmName}\"}}"
                );
            }

            var tyName = t.FullName.Replace('+', '.').Replace(".", "___");

            var exportedFns = t.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(x => !x.Name.StartsWith("odntrop_") && x.GetCustomAttribute<GeneratedMethodAttribute>() == null).ToArray();

            var toImport = t.GetNestedType("ToImport", BindingFlags.Public | BindingFlags.NonPublic);
            var importedFns = (toImport?.GetMethods(BindingFlags.Public | BindingFlags.Static) ?? Array.Empty<MethodInfo>()).Where(x => !x.Name.StartsWith("odntrop_")).ToArray();

            {
                var tgtFile = Path.GetFullPath(Path.Combine(containingDir, $"{t.Name}.g.cs"));

                s_StrBld.Clear();

                s_StrBld.AppendLine("using OdinInterop;");
                s_StrBld.AppendLine("using System;");
                s_StrBld.AppendLine("using System.Collections.Generic;");
                s_StrBld.AppendLine("using System.Runtime.InteropServices;");
                s_StrBld.AppendLine("#if UNITY_EDITOR");
                s_StrBld.AppendLine("using UnityEditor;");
                s_StrBld.AppendLine("#endif");
                s_StrBld.AppendLine("using UnityEngine;");
                s_StrBld.AppendLine();

                if (!string.IsNullOrWhiteSpace(t.Namespace))
                {
                    s_StrBld.AppendLine($"namespace {t.Namespace}");
                    s_StrBld.AppendLine("{");
                    s_StrBldIndent++;
                }

                s_StrBld.AppendIndent().AppendLine($"public static partial class {t.Name}");
                s_StrBld.AppendIndent().AppendLine("{");
                s_StrBldIndent++;

                // dll name
                s_StrBld.AppendIndent().AppendLine("private const string k_OdinInteropDllName = ");
                s_StrBldIndent++;
                s_StrBld.AppendLine("#if UNITY_IOS && !UNITY_EDITOR");
                s_StrBld.AppendIndent().AppendLine("\"__Internal\";");
                s_StrBld.AppendLine("#else");
                s_StrBld.AppendIndent().AppendLine("\"OdinInterop\";");
                s_StrBld.AppendLine("#endif").AppendLine();
                s_StrBldIndent--;

                // generate some delegates
                foreach (var exportedFn in exportedFns)
                {
                    s_StrBld.AppendIndent().Append("private delegate ");
                    s_StrBld.AppendCSharpTypeName(exportedFn.ReturnType, true);
                    s_StrBld.Append($" odntrop_del_{exportedFn.Name}(");
                    var parms = exportedFn.GetParameters();
                    for (int i = 0; i < parms.Length; i++)
                    {
                        var p = parms[i];
                        s_StrBld.AppendCSharpTypeName(p.ParameterType, true).Append(' ').Append(p.Name);
                        if (i < parms.Length - 1)
                        {
                            s_StrBld.Append(", ");
                        }
                    }
                    s_StrBld.AppendLine(");").AppendLine();

                    // delegate for the imported setter fn
                    s_StrBld
                        .AppendIndent()
                        .Append("private delegate void odntrop_del_Set")
                        .Append(exportedFn.Name)
                        .Append("Delegate(")
                        .Append("odntrop_del_")
                        .Append(exportedFn.Name)
                        .AppendLine(" value);")
                        .AppendLine();
                }

                foreach (var importedFn in importedFns)
                {
                    s_StrBld.AppendIndent().Append("private delegate ");
                    s_StrBld.AppendCSharpTypeName(importedFn.ReturnType, true);
                    s_StrBld.Append($" odntrop_del_{importedFn.Name}(");
                    var parms = importedFn.GetParameters();
                    for (int i = 0; i < parms.Length; i++)
                    {
                        var p = parms[i];
                        s_StrBld.AppendCSharpTypeName(p.ParameterType, true).Append(' ').Append(p.Name);
                        if (i < parms.Length - 1)
                        {
                            s_StrBld.Append(", ");
                        }
                    }
                    s_StrBld.AppendLine(");").AppendLine();
                }

                // set up p/invoke friendly-wrappers for exported functions
                foreach (var exportedFn in exportedFns)
                {
                    s_StrBld
                        .AppendIndent()
                        .Append("[AOT.MonoPInvokeCallback(typeof(odntrop_del_")
                        .Append(exportedFn.Name)
                        .AppendLine("))]")
                        .AppendIndent()
                        .Append("private static ")
                        .AppendCSharpTypeName(exportedFn.ReturnType, true)
                        .Append(" odntrop_exported_")
                        .Append(exportedFn.Name)
                        .Append("(");

                    var parms = exportedFn.GetParameters();
                    for (int i = 0; i < parms.Length; i++)
                    {
                        var p = parms[i];
                        s_StrBld.AppendCSharpTypeName(p.ParameterType, true).Append(' ').Append(p.Name);
                        if (i < parms.Length - 1)
                        {
                            s_StrBld.Append(", ");
                        }
                    }
                    s_StrBld.AppendLine(")");
                    s_StrBld.AppendIndent().AppendLine("{");
                    s_StrBldIndent++;
                    s_StrBld.AppendIndent().Append(typeof(void) == exportedFn.ReturnType ? "" : "return ");
                    s_StrBld.Append(exportedFn.Name).Append("(");
                    for (int i = 0; i < parms.Length; i++)
                    {
                        var p = parms[i];
                        s_StrBld.Append(p.Name);
                        if (i < parms.Length - 1)
                        {
                            s_StrBld.Append(", ");
                        }
                    }
                    s_StrBld.AppendLine(");");
                    s_StrBldIndent--;
                    s_StrBld.AppendIndent().AppendLine("}").AppendLine();
                }

                // editor-specific code (hot-reload style)
                s_StrBld.AppendLine("#if UNITY_EDITOR").AppendLine();

                // set up proper hot reload for imported functions
                foreach (var importedFn in importedFns)
                {
                    // global var for delegate
                    {
                        s_StrBld
                            .AppendIndent()
                            .Append("private static odntrop_del_")
                            .Append(importedFn.Name)
                            .Append(" odntrop_delref_")
                            .Append(importedFn.Name)
                            .AppendLine(";")
                            .AppendLine();
                    }
                }

                s_StrBld.AppendIndent().AppendLine("[InitializeOnLoadMethod]");
                s_StrBld.AppendIndent().AppendLine("private static void odntrop_EditorInit()");
                s_StrBld.AppendIndent().AppendLine("{");
                s_StrBldIndent++;
                s_StrBld.AppendIndent().AppendLine("OdinCompilerUtils.onHotReload += odntrop_OnHotReload;");
                s_StrBld.AppendIndent().AppendLine("if (OdinCompilerUtils.initialisedAfterDomainReload) odntrop_OnHotReload(OdinCompilerUtils.libraryHandle);");
                s_StrBldIndent--;
                s_StrBld.AppendIndent().AppendLine("}").AppendLine();

                // on hot reload
                s_StrBld.AppendIndent().AppendLine("private static void odntrop_OnHotReload(IntPtr libraryHandle)");
                s_StrBld.AppendIndent().AppendLine("{");
                s_StrBldIndent++;

                foreach (var importedFn in importedFns)
                {
                    s_StrBld.AppendIndent()
                        .Append("odntrop_delref_")
                        .Append(importedFn.Name)
                        .Append(" = libraryHandle == IntPtr.Zero ? null : LibraryUtils.GetDelegate<odntrop_del_")
                        .Append(importedFn.Name)
                        .Append(">(libraryHandle, ")
                        .Append($"\"odntrop_export_{tyName}_{importedFn.Name}\"")
                        .AppendLine(");");
                }

                s_StrBld
                    .AppendLine()
                    .AppendIndent()
                    .AppendLine("if (libraryHandle == IntPtr.Zero) return;")
                    .AppendLine();

                foreach (var exportedFn in exportedFns)
                {
                    s_StrBld.AppendIndent()
                        .Append("LibraryUtils.GetDelegate<odntrop_del_Set")
                        .Append(exportedFn.Name)
                        .Append("Delegate>(libraryHandle, ")
                        .Append($"\"odntrop_export_setter_{tyName}_{exportedFn.Name}\"")
                        .Append(")?.Invoke(odntrop_exported_")
                        .Append(exportedFn.Name)
                        .AppendLine(");");
                }

                s_StrBldIndent--;
                s_StrBld.AppendIndent().AppendLine("}").AppendLine();

                // runtime code (bindings)
                s_StrBld.AppendLine("#else").AppendLine();

                // p/invoke declarations for imported functions
                foreach (var importedFn in importedFns)
                {
                    s_StrBld
                        .AppendIndent()
                        .Append("[DllImport(k_OdinInteropDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = \"odntrop_export_")
                        .Append(tyName)
                        .Append("_")
                        .Append(importedFn.Name)
                        .AppendLine("\")]")
                        .AppendIndent()
                        .Append("private static extern ")
                        .AppendCSharpTypeName(importedFn.ReturnType, true)
                        .Append(" odntrop_delref_")
                        .Append(importedFn.Name)
                        .Append("(");
                    var parms = importedFn.GetParameters();
                    for (int i = 0; i < parms.Length; i++)
                    {
                        var p = parms[i];
                        s_StrBld.AppendCSharpTypeName(p.ParameterType, true).Append(' ').Append(p.Name);
                        if (i < parms.Length - 1)
                        {
                            s_StrBld.Append(", ");
                        }
                    }
                    s_StrBld.AppendLine(");").AppendLine();
                }

                // exported function setters
                foreach (var exportedFn in exportedFns)
                {
                    s_StrBld
                        .AppendIndent()
                        .Append("[DllImport(k_OdinInteropDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = \"odntrop_export_setter_")
                        .Append(tyName)
                        .Append("_")
                        .Append(exportedFn.Name)
                        .AppendLine("\")]")
                        .AppendIndent()
                        .Append("private static extern void odntrop_set_")
                        .Append(exportedFn.Name)
                        .Append("(")
                        .Append("odntrop_del_")
                        .Append(exportedFn.Name)
                        .AppendLine(" value);")
                        .AppendLine();
                }

                s_StrBld
                    .AppendIndent()
                    .AppendLine("[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]")
                    .AppendIndent()
                    .AppendLine("private static void odntrop_RuntimeInit()")
                    .AppendIndent()
                    .AppendLine("{");

                s_StrBldIndent++;

                foreach (var exportedFn in exportedFns)
                {
                    s_StrBld.AppendIndent()
                        .Append("odntrop_set_")
                        .Append(exportedFn.Name)
                        .Append("(")
                        .Append("odntrop_exported_")
                        .Append(exportedFn.Name)
                        .AppendLine(");");
                }

                s_StrBldIndent--;
                s_StrBld.AppendIndent().AppendLine("}").AppendLine();

                s_StrBld.AppendLine("#endif").AppendLine();

                // wrapper for the user to call
                foreach (var importedFn in importedFns)
                {
                    s_StrBld
                        .AppendIndent()
                        .AppendLine("[GeneratedMethod]")
                        .AppendIndent()
                        .Append("public static ")
                        .AppendCSharpTypeName(importedFn.ReturnType, false)
                        .Append(" ")
                        .Append(importedFn.Name)
                        .Append("(");

                    var parms = importedFn.GetParameters();
                    for (int i = 0; i < parms.Length; i++)
                    {
                        var p = parms[i];
                        s_StrBld.AppendCSharpTypeName(p.ParameterType, false).Append(' ').Append(p.Name);
                        if (i < parms.Length - 1)
                        {
                            s_StrBld.Append(", ");
                        }
                    }
                    s_StrBld.AppendLine(")");
                    s_StrBld.AppendIndent().AppendLine("{");
                    s_StrBldIndent++;
                    s_StrBld
                        .AppendLine("#if UNITY_EDITOR")
                        .AppendIndent()
                        .Append("if (odntrop_delref_")
                        .Append(importedFn.Name)
                        .Append(" == null) return")
                        .Append(importedFn.ReturnType == typeof(void) ? "" : " default")
                        .AppendLine(";")
                        .AppendLine("#endif");

                    s_StrBld.AppendIndent().Append(importedFn.ReturnType == typeof(void) ? "" : "return ");
                    s_StrBld.Append("odntrop_delref_");
                    s_StrBld.Append(importedFn.Name);
                    s_StrBld.Append("(");
                    for (int i = 0; i < parms.Length; i++)
                    {
                        var p = parms[i];
                        s_StrBld.Append(p.Name);
                        if (i < parms.Length - 1)
                        {
                            s_StrBld.Append(", ");
                        }
                    }
                    s_StrBld.Append(")").Append(importedFn.ReturnType == typeof(void) ? "" : " ?? default");
                    s_StrBld.AppendLine(";");
                    s_StrBldIndent--;
                    s_StrBld.AppendIndent().AppendLine("}").AppendLine();
                }

                s_StrBldIndent--;
                s_StrBld.AppendIndent().AppendLine("}");

                if (!string.IsNullOrWhiteSpace(t.Namespace))
                {
                    s_StrBldIndent--;
                    s_StrBld.AppendLine("}");
                }

                File.WriteAllText(tgtFile, s_StrBld.ToString());
            }

            {
                var tgtFile = Path.GetFullPath(Path.Combine(ODIN_INTEROP_OUT_DIR, $"odntrop_{t.FullName.Replace('+', '.')}.odin"));

                s_StrBld
                    .Clear()
                    .AppendLine("#+vet !tabs !unused !style")
                    .AppendLine("package src")
                    .AppendLine()
                    .AppendLine("@require import \"base:runtime\"")
                    .AppendLine();

                foreach (var importedFn in importedFns)
                {
                    // delegate so user can stick to the signature
                    {
                        s_StrBld
                            .AppendIndent()
                            .Append($"odntrop_del_{tyName}_{importedFn.Name} :: #type proc(");

                        var parms = importedFn.GetParameters();
                        for (int i = 0; i < parms.Length; i++)
                        {
                            var p = parms[i];
                            s_StrBld.Append(p.Name).Append(": ").AppendOdnTypeName(p.ParameterType, false);
                            s_StrBld.Append(", ");
                        }

                        s_StrBld.Append(")");
                        if (importedFn.ReturnType != typeof(void))
                        {
                            s_StrBld.Append(" -> ").AppendOdnTypeName(importedFn.ReturnType, false);
                        }

                        s_StrBld.AppendLine().AppendLine();
                    }

                    // exported p/invoke wrapper
                    {
                        s_StrBld
                            .AppendIndent()
                            .AppendLine("@(export, private = \"file\")")
                            .AppendIndent()
                            .Append($"odntrop_export_{tyName}_{importedFn.Name} :: proc \"c\" (");

                        var parms = importedFn.GetParameters();
                        for (int i = 0; i < parms.Length; i++)
                        {
                            var p = parms[i];
                            s_StrBld.Append(p.Name).Append(": ").AppendOdnTypeName(p.ParameterType, true);
                            s_StrBld.Append(", ");
                        }

                        s_StrBld.Append(")");
                        if (importedFn.ReturnType != typeof(void))
                        {
                            s_StrBld.Append(" -> ").AppendOdnTypeName(importedFn.ReturnType, true);
                        }

                        s_StrBld.AppendLine(" {");

                        s_StrBldIndent++;
                        s_StrBld
                            .AppendIndent()
                            .AppendLine("context = runtime.default_context()")
                            .AppendIndent()
                            .Append(importedFn.ReturnType == typeof(void) ? "" : "return ")
                            .Append($"odntrop_{tyName}_{importedFn.Name}(");

                        for (int i = 0; i < parms.Length; i++)
                        {
                            var p = parms[i];
                            s_StrBld.Append(p.Name);
                            s_StrBld.Append(", ");
                        }

                        s_StrBld.AppendLine(")");

                        s_StrBldIndent--;
                        s_StrBld.AppendIndent().AppendLine("}").AppendLine();
                    }
                }

                foreach (var exportedFn in exportedFns)
                {
                    // delegate
                    {
                        s_StrBld
                            .AppendIndent()
                            .AppendLine("@(private = \"file\")")
                            .AppendIndent()
                            .Append($"odntrop_del_{tyName}_{exportedFn.Name} :: #type proc \"c\" (");

                        var parms = exportedFn.GetParameters();
                        for (int i = 0; i < parms.Length; i++)
                        {
                            var p = parms[i];
                            s_StrBld.Append(p.Name).Append(": ").AppendOdnTypeName(p.ParameterType, true);
                            s_StrBld.Append(", ");
                        }

                        s_StrBld.Append(")");
                        if (exportedFn.ReturnType != typeof(void))
                        {
                            s_StrBld.Append(" -> ").AppendOdnTypeName(exportedFn.ReturnType, true);
                        }

                        s_StrBld.AppendLine().AppendLine();
                    }

                    // delegate global var
                    {
                        s_StrBld
                            .AppendIndent()
                            .AppendLine("@(private = \"file\")")
                            .AppendIndent()
                            .AppendLine($"odntrop_dydel_{tyName}_{exportedFn.Name}: odntrop_del_{tyName}_{exportedFn.Name} = nil")
                            .AppendLine();
                    }

                    // global var setter fn
                    {
                        s_StrBld
                            .AppendIndent()
                            .AppendLine("@(export, private = \"file\")")
                            .AppendIndent()
                            .AppendLine($"odntrop_export_setter_{tyName}_{exportedFn.Name} :: proc (value: odntrop_del_{tyName}_{exportedFn.Name}) {{");

                        s_StrBldIndent++;
                        s_StrBld
                            .AppendIndent()
                            .AppendLine($"odntrop_dydel_{tyName}_{exportedFn.Name} = value");
                        s_StrBldIndent--;
                        s_StrBld.AppendIndent().AppendLine("}").AppendLine();
                    }

                    // actual function that the fucking user can call
                    {
                        s_StrBld
                            .AppendIndent()
                            .Append($"{tyName}_{exportedFn.Name} :: proc(");

                        var parms = exportedFn.GetParameters();
                        for (int i = 0; i < parms.Length; i++)
                        {
                            var p = parms[i];
                            s_StrBld.Append(p.Name).Append(": ").AppendOdnTypeName(p.ParameterType, false);
                            s_StrBld.Append(", ");
                        }

                        s_StrBld.Append(")");
                        if (exportedFn.ReturnType != typeof(void))
                        {
                            s_StrBld.Append(" -> ").AppendOdnTypeName(exportedFn.ReturnType, false);
                        }

                        s_StrBld.AppendLine(" {");
                        s_StrBldIndent++;
                        s_StrBld
                            .AppendIndent()
                            .Append(exportedFn.ReturnType == typeof(void) ? "" : "return ")
                            .Append($"odntrop_dydel_{tyName}_{exportedFn.Name}(");

                        for (int i = 0; i < parms.Length; i++)
                        {
                            var p = parms[i];
                            s_StrBld.Append(p.Name);
                            s_StrBld.Append(", ");
                        }

                        s_StrBld.AppendLine(")");

                        s_StrBldIndent--;
                        s_StrBld.AppendIndent().AppendLine("}").AppendLine();
                    }
                }

                File.WriteAllText(tgtFile, s_StrBld.ToString());
            }
        }

        private static StringBuilder AppendCSharpTypeName(this StringBuilder sb, Type t, bool useInteroperableVersion)
        {
            if (t == typeof(void))
            {
                sb.Append("void");
            }
            else if (t == typeof(byte))
            {
                sb.Append("byte");
            }
            else if (t == typeof(sbyte))
            {
                sb.Append("sbyte");
            }
            else if (t == typeof(short))
            {
                sb.Append("short");
            }
            else if (t == typeof(ushort))
            {
                sb.Append("ushort");
            }
            else if (t == typeof(int))
            {
                sb.Append("int");
            }
            else if (t == typeof(uint))
            {
                sb.Append("uint");
            }
            else if (t == typeof(long))
            {
                sb.Append("long");
            }
            else if (t == typeof(ulong))
            {
                sb.Append("ulong");
            }
            else if (t == typeof(float))
            {
                sb.Append("float");
            }
            else if (t == typeof(double))
            {
                sb.Append("double");
            }
            else if (t == typeof(bool))
            {
                sb.Append("bool");
            }
            else if (t == typeof(Vector2))
            {
                sb.Append("UnityEngine.Vector2");
            }
            else if (t == typeof(Vector3))
            {
                sb.Append("UnityEngine.Vector3");
            }
            else if (t == typeof(Vector4))
            {
                sb.Append("UnityEngine.Vector4");
            }
            else if (t == typeof(Quaternion))
            {
                sb.Append("UnityEngine.Quaternion");
            }
            else if (t == typeof(Color))
            {
                sb.Append("UnityEngine.Color");
            }
            else if (t.IsEnum)
            {
                sb.Append(t.FullName.Replace('+', '.'));
                s_ExportedTypes.Add(t); // only need to export; c# side is already blittable
            }
            else if (typeof(UnityEngine.Object).IsAssignableFrom(t)) // unity object or subclass
            {
                if (useInteroperableVersion) // instance id
                {
                    sb.Append("int");
                }
                else
                {
                    sb.Append(t.FullName.Replace('+', '.'));
                }

                s_ExportedTypes.Add(t);
            }
            else
            {
                if (useInteroperableVersion)
                {
                    sb.Append("odntrop_type_");
                    sb.Append(t.FullName.Replace('+', '.').Replace(".", "___"));
                }
                else
                {
                    sb.Append(t.FullName.Replace('+', '.'));
                }

                s_ExportedTypes.Add(t);
                s_ImportedTypes.Add(t); // need to create a blittable proxy
            }

            return sb;
        }

        private static StringBuilder AppendOdnTypeName(this StringBuilder sb, Type t, bool useInteroperableVersion)
        {
            var isUnityNativeType = !string.IsNullOrWhiteSpace(t.Namespace) && t.Namespace.StartsWith("UnityEngine");
            var resolvedName = isUnityNativeType ? t.Name : t.FullName.Replace('+', '.').Replace('.', '_'); // for unity native types, just use the type name

            if (t == typeof(void))
            {
                sb.Append("()");
            }
            else if (t == typeof(byte))
            {
                sb.Append("u8");
            }
            else if (t == typeof(sbyte))
            {
                sb.Append("i8");
            }
            else if (t == typeof(short))
            {
                sb.Append("i16");
            }
            else if (t == typeof(ushort))
            {
                sb.Append("u16");
            }
            else if (t == typeof(int))
            {
                sb.Append("i32");
            }
            else if (t == typeof(uint))
            {
                sb.Append("u32");
            }
            else if (t == typeof(long))
            {
                sb.Append("i64");
            }
            else if (t == typeof(ulong))
            {
                sb.Append("u64");
            }
            else if (t == typeof(float))
            {
                sb.Append("f32");
            }
            else if (t == typeof(double))
            {
                sb.Append("f64");
            }
            else if (t == typeof(bool))
            {
                sb.Append("bool");
            }
            else if (t == typeof(Vector2))
            {
                sb.Append("[2]f32");
            }
            else if (t == typeof(Vector3))
            {
                sb.Append("[3]f32");
            }
            else if (t == typeof(Vector4))
            {
                sb.Append("[4]f32");
            }
            else if (t == typeof(Quaternion))
            {
                sb.Append("quaternion128");
            }
            else if (t == typeof(Color))
            {
                sb.Append("[4]f32");
            }
            else
            {
                sb.Append(resolvedName);
            }

            return sb;
        }
    }
}
