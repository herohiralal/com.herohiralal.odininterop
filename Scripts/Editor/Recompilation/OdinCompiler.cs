using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Text;
using System.Collections.Generic;

namespace OdinInterop.Editor
{
    internal static class OdinCompiler
    {
        private static readonly string ODIN_LIB_INPUT_PATH = InteropGenerator.ODIN_INTEROP_OUT_DIR;
        private static readonly string ODIN_LIB_OUTPUT_DIR_PATH = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Library", "OdinInterop"));
        private static readonly string ODIN_LIB_EDITOR_OUTPUT_DIR_PATH = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, "Editor", "Debug");
        public static readonly string ODIN_PLUGIN_DIR_PATH = Path.GetFullPath(Path.Combine(Application.dataPath, "Plugins"));
        public static readonly string ODIN_WINDOWS_PLUGIN_DIR_PATH = Path.Combine(ODIN_PLUGIN_DIR_PATH, "Windows");
        public static readonly string ODIN_WINDOWS_PLUGIN_PATH = Path.Combine(ODIN_WINDOWS_PLUGIN_DIR_PATH, "OdinInterop.lib");
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

            if (!CompileOdinInteropLibraryForEditor())
            {
                Debug.LogError("[OdinCompiler]: Failed to compile OdinInteropEditor library. No active library present.");
                return;
            }

            var libraryHandle = LibraryUtils.OpenLibrary(ODIN_LIB_EDITOR_OUTPUT_PATH);
            if (libraryHandle == System.IntPtr.Zero)
            {
                Debug.LogError("[OdinCompiler]: Failed to load compiled OdinInteropEditor library. No active library present.");
                return;
            }

            OdinCompilerUtils.RaiseHotReloadEvt(libraryHandle);
        }

        internal static bool CompileOdinInteropLibraryForEditor()
        {
            // TODO: rename pdb for windows
            return RunOdinCompiler(new List<string>
            {
                $"-out:{ODIN_LIB_EDITOR_OUTPUT_PATH}",
                "-build-mode:dynamic",
                "-debug",
            });
        }

        // assumes windows host
        internal static bool CompileOdinInteropLibraryForWindows(bool isRelease)
        {
            if (!Directory.Exists(ODIN_WINDOWS_PLUGIN_DIR_PATH))
                Directory.CreateDirectory(ODIN_WINDOWS_PLUGIN_DIR_PATH);

            var l = new List<string>
            {
                $"-out:{ODIN_WINDOWS_PLUGIN_PATH}",
                "-build-mode:static",
            };

            if (!isRelease)
            {
                l.Add("-debug");
            }

            return RunOdinCompiler(l);
        }

        private static bool RunOdinCompiler(List<string> args)
        {
            var psi = new System.Diagnostics.ProcessStartInfo("odin");
            psi.WorkingDirectory = ODIN_LIB_INPUT_PATH;
            psi.ArgumentList.Clear();
            psi.ArgumentList.Add("build");
            psi.ArgumentList.Add(".");

            psi.ArgumentList.Add("-min-link-libs");
            psi.ArgumentList.Add("-use-single-module");
            psi.ArgumentList.Add("-reloc-mode:pic");
            foreach (var arg in args)
                psi.ArgumentList.Add(arg);

            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;

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
            if (report.summary.platform == BuildTarget.StandaloneWindows64)
            {
                var isRelease = !report.summary.options.HasFlag(BuildOptions.Development);
                if (!OdinCompiler.CompileOdinInteropLibraryForWindows(isRelease))
                {
                    throw new BuildFailedException("Failed to compile OdinInterop library for Windows build.");
                }
            }
        }
    }

    internal class OdinCompilerBuildPostprocessor : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.StandaloneWindows64)
            {
                if (File.Exists(OdinCompiler.ODIN_WINDOWS_PLUGIN_PATH)) File.Delete(OdinCompiler.ODIN_WINDOWS_PLUGIN_PATH);
            }
        }
    }
}
