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

        [InitializeOnLoadMethod]
        private static unsafe void InitialiseEditor()
        {
            var ulv = OdinCompilerPersistentData.staticLibraryHandle;
            if (ulv == 0) HotReload();
            else OdinCompilerUtils.RaiseHotReloadEvt(ulv);
        }

        public static unsafe void HotReload()
        {
            if (!CompileOdinInteropLibrary(out var outputFile, null, false)) return;
            var lv = LibraryUtils.OpenLibrary(outputFile).ToInt64();
            var ulv = *(ulong*)&lv;

            OdinCompilerPersistentData.staticLibraryHandle = ulv;
            OdinCompilerUtils.RaiseHotReloadEvt(ulv);
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

#if UNITY_EDITOR_WIN
                const string k_OutputLibName = "OdinInteropEditor.dll";
#else
                const string k_OutputLibName = "libOdinInteropEditor.so";
#endif

                outFile = Path.Combine(outDir, k_OutputLibName);
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
