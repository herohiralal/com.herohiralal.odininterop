using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Reflection;

namespace OdinInterop.Editor
{
    internal static class InteropGenerator
    {
        private static readonly string ENGINE_APIS_OUT_DIR = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Packages", "com.herohiralal.odininterop", "Scripts", "Runtime", "Generated"));
        private static readonly string PROJECT_APIS_OUT_DIR = Path.GetFullPath(Path.Combine(Application.dataPath, "OdinInterop", "Generated"));
        private static readonly string ODIN_INTEROP_OUT_DIR = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Source", "OdinInterop", "Generated"));

        private static HashSet<Type> s_ExportedTypes = new HashSet<Type>();

        [MenuItem("Tools/Odin Interop/Generate Interop Code")]
        private static void GenerateInteropCode()
        {
            s_ExportedTypes.Clear();

            if (Directory.Exists(ODIN_INTEROP_OUT_DIR))
            {
                Directory.Delete(ODIN_INTEROP_OUT_DIR, true);
            }
            Directory.CreateDirectory(ODIN_INTEROP_OUT_DIR);

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
        private static int s_StdBldIndent = 0;
        private static StringBuilder AppendStrBldIndent(this StringBuilder sb)
        {
            for (int i = 0; i < s_StdBldIndent; i++)
            {
                sb.Append('\t');
            }

            return sb;
        }

        private static void GenerateInteropCodeInternal(Type t, bool isEngineCode)
        {
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

            var exportedFns = t.GetMethods(BindingFlags.Public | BindingFlags.Static);
            var nothingToExport = (exportedFns.Length == 0);

            var toImport = t.GetNestedType("ToImport", BindingFlags.Public | BindingFlags.NonPublic);
            var importedFns = toImport?.GetMethods(BindingFlags.Public | BindingFlags.Static) ?? Array.Empty<MethodInfo>();
            var nothingToImport = (importedFns.Length == 0);

            var tgtFile = Path.GetFullPath(Path.Combine(containingDir, $"{t.Name}.g.cs"));

            s_StrBld.Clear();

            s_StrBld.AppendLine("using OdinInterop;");
            s_StrBld.AppendLine("using System;");
            s_StrBld.AppendLine("using System.Collections.Generic;");
            s_StrBld.AppendLine("#if UNITY_EDITOR");
            s_StrBld.AppendLine("using UnityEditor;");
            s_StrBld.AppendLine("using UnityEditor.Callbacks;");
            s_StrBld.AppendLine("#endif");
            s_StrBld.AppendLine("using UnityEngine;");
            s_StrBld.AppendLine();

            if (!string.IsNullOrWhiteSpace(t.Namespace))
            {
                s_StrBld.AppendLine($"namespace {t.Namespace}");
                s_StrBld.AppendLine("{");
                s_StdBldIndent++;
            }

            s_StrBld.AppendStrBldIndent().AppendLine($"public static partial class {t.Name}");
            s_StrBld.AppendStrBldIndent().AppendLine("{");
            s_StdBldIndent++;

            // dll name
            s_StrBld.AppendStrBldIndent().AppendLine("private const string k_OdinInteropDllName = ");
            s_StdBldIndent++;
            s_StrBld.AppendLine("#if UNITY_IOS && !UNITY_EDITOR");
            s_StrBld.AppendStrBldIndent().AppendLine("\"__Internal\";");
            s_StrBld.AppendLine("#else");
            s_StrBld.AppendStrBldIndent().AppendLine("\"odininteropcode\";");
            s_StrBld.AppendLine("#endif");
            s_StdBldIndent--;

            // generate some delegates
            foreach (var exportedFn in exportedFns)
            {
            }

            // editor-specific code (hot-reload style)
            s_StrBld.AppendLine("#if UNITY_EDITOR");

            // runtime code (bindings)
            s_StrBld.AppendLine("#else");

            s_StrBld.AppendLine("#endif");

            s_StdBldIndent--;
            s_StrBld.AppendStrBldIndent().AppendLine("}");

            if (!string.IsNullOrWhiteSpace(t.Namespace))
            {
                s_StdBldIndent--;
                s_StrBld.AppendLine("}");
            }

            File.WriteAllText(tgtFile, s_StrBld.ToString());
        }
    }
}
