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
        internal static readonly string ODIN_INTEROP_OUT_DIR = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Source", "OdinInterop"));

        private static HashSet<Type> s_ExportedTypes = new HashSet<Type>(256); // to create in odin

        internal static void GenerateInteropCode()
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

            foreach (var (n, c) in InteropGeneratorInbuiltFiles.files)
            {
                var tgtFile = Path.GetFullPath(Path.Combine(ODIN_INTEROP_OUT_DIR, $"odntrop_{n}"));
                File.WriteAllText(tgtFile, c);
            }

            foreach (var t in TypeCache.GetTypesWithAttribute<GenerateOdinInteropAttribute>())
            {
                // public static partial classes only
                if (!t.IsAbstract || !t.IsSealed)
                {
                    Debug.LogError($"[Odin Interop] Type {t.FullName} is marked with GenerateOdinInteropAttribute but is not a static class. Skipping...");
                    continue;
                }

                GenerateInteropCodeInternal(t);
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

        private static void GenerateInteropCodeInternal(Type t)
        {
            var tyName = t.FullName.Replace('+', '.').Replace(".", "___");
            var cleanTyName = tyName == "OdinInterop___EngineBindings" ? "" : tyName;
            var underScoreIfCleanTyName = cleanTyName == "" ? "" : "_";

            var exportedFns = t.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Where(x => !x.Name.StartsWith("odntrop_")).ToArray();
            var importedFns = t.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(x => !x.Name.StartsWith("odntrop_")).ToArray();

            {
                var tgtFile = Path.GetFullPath(Path.Combine(ODIN_INTEROP_OUT_DIR, $"odntrop_{tyName}.odin"));

                s_StrBld
                    .Clear()
                    .AppendLine("// THIS IS A GENERATED FILE - DO NOT MODIFY OR YOUR CHANGES WILL BE LOST!")
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
                            .Append($"{cleanTyName}{underScoreIfCleanTyName}{importedFn.Name}Delegate :: #type proc(");

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
                            .AppendLine("context = runtime.default_context() if G_OdnTrop_Internal_CtxNesting == 0 else G_OdnTrop_Internal_Ctx")
                            .AppendIndent()
                            .AppendLine("G_OdnTrop_Internal_CtxNesting += 1")
                            .AppendIndent()
                            .AppendLine("defer G_OdnTrop_Internal_CtxNesting -= 1")
                            .AppendIndent()
                            .Append(importedFn.ReturnType == typeof(void) ? "" : "return ")
                            .Append($"{cleanTyName}{underScoreIfCleanTyName}{importedFn.Name}(");

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
                            .Append($"{cleanTyName}{underScoreIfCleanTyName}{exportedFn.Name} :: proc(");

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
                            .AppendLine("odntrop_internal_tempCtx := G_OdnTrop_Internal_Ctx")
                            .AppendIndent()
                            .AppendLine("G_OdnTrop_Internal_Ctx = context")
                            .AppendIndent()
                            .AppendLine("defer G_OdnTrop_Internal_Ctx = odntrop_internal_tempCtx")
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

        private static StringBuilder AppendOdnTypeName(this StringBuilder sb, Type t, bool useInteroperableVersion)
        {
            if (t.IsPointer || t.IsByRef)
            {
                sb.Append("^");
                sb.AppendOdnTypeName(t.GetElementType(), useInteroperableVersion);
                return sb;
            }

            var isInternalType = t.Namespace == "UnityEngine" || t.Namespace == "OdinInterop";
            var resolvedName = isInternalType ? t.Name : t.FullName.Replace('+', '.').Replace('.', '_'); // for internal types, just use the type name

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
            else if (t.IsArray)
            {
                sb.Append("[]").AppendOdnTypeName(t.GetElementType(), useInteroperableVersion);
            }
            else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Slice<>))
            {
                sb.Append("[]").AppendOdnTypeName(t.GetGenericArguments()[0], useInteroperableVersion);
            }
            else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
            {
                sb.Append("[dynamic]").AppendOdnTypeName(t.GetGenericArguments()[0], useInteroperableVersion);
            }
            else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(DynamicArray<>))
            {
                sb.Append("[dynamic]").AppendOdnTypeName(t.GetGenericArguments()[0], useInteroperableVersion);
            }
            else if (t == typeof(RawSlice))
            {
                sb.Append("runtime.Raw_Slice");
            }
            else if (t == typeof(RawDynamicArray))
            {
                sb.Append("runtime.Raw_Dynamic_Array");
            }
            else if (t == typeof(string))
            {
                sb.Append("string16");
            }
            else if (t == typeof(String8))
            {
                sb.Append("string");
            }
            else if (t == typeof(String16))
            {
                sb.Append("string16");
            }
            else
            {
                sb.Append(resolvedName);
                s_ExportedTypes.Add(t);
            }

            return sb;
        }
    }
}
