using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Text;

namespace OdinInterop.Editor
{
    internal static class OdinCompiler
    {
        private static readonly string ODIN_LIB_INPUT_PATH = InteropGenerator.ODIN_INTEROP_OUT_DIR;
        private static readonly string ODIN_LIB_OUTPUT_DIR_PATH = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Library", "OdinInterop"));
        private static readonly string ODIN_LIB_EDITOR_OUTPUT_DIR_PATH = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, "Editor", "Debug");
        private static readonly string ODIN_LIB_EDITOR_OUTPUT_PATH = Path.Combine(ODIN_LIB_EDITOR_OUTPUT_DIR_PATH,
#if UNITY_EDITOR_WIN
            "OdinInteropEditor.dll"
#else
            "libOdinInteropEditor.so"
#endif
        );

        [InitializeOnLoadMethod]
        private static void InitialiseEditor()
        {
            /**
             * because of how unity domain reload works, we don't know if we've previously loaded this native lib
             * so we just always load+unload it to clear up any dangling references
             */
            if (File.Exists(ODIN_LIB_EDITOR_OUTPUT_PATH))
            {
                LibraryUtils.CloseLibraryIfLoaded(ODIN_LIB_EDITOR_OUTPUT_PATH);
            }

            HotReload();
        }

        [MenuItem("Tools/Odin Interop/Hot Reload %&R")]
        public static void HotReload()
        {
            if (OdinCompilerUtils.libraryHandle != System.IntPtr.Zero)
            {
                LibraryUtils.CloseLibrary(OdinCompilerUtils.libraryHandle);
                OdinCompilerUtils.RaiseHotReloadEvt(System.IntPtr.Zero);
            }

            if (!CompileOdinInteropLibrary(out var outFile, null, false))
            {
                Debug.LogError("[OdinCompiler]: Failed to compile OdinInteropEditor library. No active library present.");
                return;
            }

            var libraryHandle = LibraryUtils.OpenLibrary(outFile);
            if (libraryHandle == System.IntPtr.Zero)
            {
                Debug.LogError("[OdinCompiler]: Failed to load compiled OdinInteropEditor library. No active library present.");
                return;
            }

            OdinCompilerUtils.RaiseHotReloadEvt(libraryHandle);
        }

        internal static bool CompileOdinInteropLibrary(out string outFile, BuildTargetGroup? tgt, bool isReleaseBuild)
        {
            if (!Directory.Exists(ODIN_LIB_OUTPUT_DIR_PATH))
                Directory.CreateDirectory(ODIN_LIB_OUTPUT_DIR_PATH);

            var platformSpecificOutDir = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, tgt?.ToString() ?? "Editor");
            if (!Directory.Exists(platformSpecificOutDir))
                Directory.CreateDirectory(platformSpecificOutDir);

            var outDir = Path.GetFullPath(Path.Combine(platformSpecificOutDir, isReleaseBuild ? "Release" : "Debug"));
            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);

            var psi = new System.Diagnostics.ProcessStartInfo("odin");
            psi.WorkingDirectory = ODIN_LIB_INPUT_PATH;
            psi.ArgumentList.Clear();
            psi.ArgumentList.Add("build");
            psi.ArgumentList.Add(".");

            psi.ArgumentList.Add("-min-link-libs");
            psi.ArgumentList.Add("-use-single-module");
            psi.ArgumentList.Add("-reloc-mode:pic");

            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;

            if (!tgt.HasValue) // for editor
            {
                // no need to specify what platform yay

                outFile = ODIN_LIB_EDITOR_OUTPUT_PATH;
                psi.ArgumentList.Add($"-out:{outFile}");
                psi.ArgumentList.Add("-build-mode:dynamic");

                // TODO: rename pdb for windows
            }
            else
            {
                outFile = "";
            }

            if (!isReleaseBuild)
                psi.ArgumentList.Add("-debug");

            var process = System.Diagnostics.Process.Start(psi);
            var sb = new StringBuilder(2048);
            process.OutputDataReceived += (_, args) => { if (args != null) sb.AppendLine(args.Data); };
            process.ErrorDataReceived += (_, args) => { if (args != null) sb.AppendLine(args.Data); };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            var success = process.ExitCode == 0;

            process.Close();

            var s = sb.ToString();
            if (!string.IsNullOrWhiteSpace(s))
                Debug.LogFormat(success ? LogType.Log : LogType.Error, LogOption.NoStacktrace, null, "[OdinCompiler]: {0}", s);

            return success;
        }
    }

    internal class OdinCompilerBuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            // TODO
        }
    }
}
