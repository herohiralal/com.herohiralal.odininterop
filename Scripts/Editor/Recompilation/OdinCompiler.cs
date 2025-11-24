using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Android;
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

        // editor stuff
        private static readonly string ODIN_LIB_EDITOR_OUTPUT_DIR_PATH = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, "Editor");
        private static readonly string ODIN_LIB_EDITOR_OUTPUT_PATH = Path.Combine(ODIN_LIB_EDITOR_OUTPUT_DIR_PATH,
#if UNITY_EDITOR_WIN
            "OdinInteropEditor.dll"
#else
            "libOdinInteropEditor.so"
#endif
        );

        // runtime stuff
        public static readonly string ODIN_PLUGIN_DIR_PATH = Path.GetFullPath(Path.Combine(Application.dataPath, "Plugins"));

        // windows
        public static readonly string ODIN_WINDOWS_PLUGIN_DIR_PATH = Path.Combine(ODIN_PLUGIN_DIR_PATH, "Windows");
        public static readonly string ODIN_WINDOWS_PLUGIN_PATH = Path.Combine(ODIN_WINDOWS_PLUGIN_DIR_PATH, "OdinInterop.dll");
        public static readonly string ODIN_WINDOWS_PLUGIN_EXP_PATH = Path.Combine(ODIN_WINDOWS_PLUGIN_DIR_PATH, "OdinInterop.exp");
        public static readonly string ODIN_WINDOWS_PLUGIN_PDB_PATH = Path.Combine(ODIN_WINDOWS_PLUGIN_DIR_PATH, "OdinInterop.pdb");
        public static readonly string ODIN_WINDOWS_PLUGIN_STAT_PATH = Path.Combine(ODIN_WINDOWS_PLUGIN_DIR_PATH, "OdinInterop.lib");

        // android
        public static readonly string ODIN_ANDROID_OBJ_TEMP_DIR_PATH = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, "Android");
        public static readonly string ODIN_ANDROID_ARMv7_OBJ_TEMP_DIR_PATH = Path.Combine(ODIN_ANDROID_OBJ_TEMP_DIR_PATH, "armv7");
        public static readonly string ODIN_ANDROID_ARMv8_OBJ_TEMP_DIR_PATH = Path.Combine(ODIN_ANDROID_OBJ_TEMP_DIR_PATH, "armv8");
        public static readonly string ODIN_ANDROID_X8664_OBJ_TEMP_DIR_PATH = Path.Combine(ODIN_ANDROID_OBJ_TEMP_DIR_PATH, "x86_64");
        public static readonly string ODIN_ANDROID_ARMv7_OBJ_PATH = Path.Combine(ODIN_ANDROID_ARMv7_OBJ_TEMP_DIR_PATH, "OdinInterop.o");
        public static readonly string ODIN_ANDROID_ARMv8_OBJ_PATH = Path.Combine(ODIN_ANDROID_ARMv8_OBJ_TEMP_DIR_PATH, "OdinInterop.o");
        public static readonly string ODIN_ANDROID_X8664_OBJ_PATH = Path.Combine(ODIN_ANDROID_X8664_OBJ_TEMP_DIR_PATH, "OdinInterop.o");
        public static readonly string ODIN_ANDROID_PLUGIN_DIR_PATH = Path.Combine(ODIN_PLUGIN_DIR_PATH, "Android");
        public static readonly string ODIN_ANDROID_ARMv7_PLUGIN_DIR_PATH = Path.Combine(ODIN_ANDROID_PLUGIN_DIR_PATH, "armv7");
        public static readonly string ODIN_ANDROID_ARMv8_PLUGIN_DIR_PATH = Path.Combine(ODIN_ANDROID_PLUGIN_DIR_PATH, "aarch64");
        public static readonly string ODIN_ANDROID_X8664_PLUGIN_DIR_PATH = Path.Combine(ODIN_ANDROID_PLUGIN_DIR_PATH, "x86_64");
        public static readonly string ODIN_ANDROID_ARMv7_PLUGIN_PATH = Path.Combine(ODIN_ANDROID_ARMv7_PLUGIN_DIR_PATH, "libOdinInterop.so");
        public static readonly string ODIN_ANDROID_ARMv8_PLUGIN_PATH = Path.Combine(ODIN_ANDROID_ARMv8_PLUGIN_DIR_PATH, "libOdinInterop.so");
        public static readonly string ODIN_ANDROID_X8664_PLUGIN_PATH = Path.Combine(ODIN_ANDROID_X8664_PLUGIN_DIR_PATH, "libOdinInterop.so");

        // linux
        public static readonly string ODIN_LINUX_OBJ_TEMP_DIR_PATH = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, "Linux");
        public static readonly string ODIN_LINUX_OBJ_PATH = Path.Combine(ODIN_LINUX_OBJ_TEMP_DIR_PATH, "OdinInterop.o");
        public static readonly string ODIN_LINUX_PLUGIN_DIR_PATH = Path.Combine(ODIN_PLUGIN_DIR_PATH, "Linux", "x86_64");
        public static readonly string ODIN_LINUX_PLUGIN_PATH = Path.Combine(ODIN_LINUX_PLUGIN_DIR_PATH, "libOdinInterop.so");

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
                "-build-mode:dynamic",
            };

            var success = RunOdinCompiler(l, isRelease: isRelease);
            if (!success) return false;

            AssetDatabase.Refresh();
            return true;
        }

        internal static bool CompileOdinInteropLibraryForAndroid(bool isRelease)
        {
            if (!Directory.Exists(ODIN_ANDROID_ARMv7_PLUGIN_DIR_PATH))
                Directory.CreateDirectory(ODIN_ANDROID_ARMv7_PLUGIN_DIR_PATH);
            if (!Directory.Exists(ODIN_ANDROID_ARMv8_PLUGIN_DIR_PATH))
                Directory.CreateDirectory(ODIN_ANDROID_ARMv8_PLUGIN_DIR_PATH);
            if (!Directory.Exists(ODIN_ANDROID_X8664_PLUGIN_DIR_PATH))
                Directory.CreateDirectory(ODIN_ANDROID_X8664_PLUGIN_DIR_PATH);

            if (Directory.Exists(ODIN_ANDROID_ARMv7_OBJ_TEMP_DIR_PATH))
                Directory.Delete(ODIN_ANDROID_ARMv7_OBJ_TEMP_DIR_PATH, true);
            Directory.CreateDirectory(ODIN_ANDROID_ARMv7_OBJ_TEMP_DIR_PATH);

            if (Directory.Exists(ODIN_ANDROID_ARMv8_OBJ_TEMP_DIR_PATH))
                Directory.Delete(ODIN_ANDROID_ARMv8_OBJ_TEMP_DIR_PATH, true);
            Directory.CreateDirectory(ODIN_ANDROID_ARMv8_OBJ_TEMP_DIR_PATH);

            if (Directory.Exists(ODIN_ANDROID_X8664_OBJ_TEMP_DIR_PATH))
                Directory.Delete(ODIN_ANDROID_X8664_OBJ_TEMP_DIR_PATH, true);
            Directory.CreateDirectory(ODIN_ANDROID_X8664_OBJ_TEMP_DIR_PATH);

            var ndkPath = AndroidExternalToolsSettings.ndkRootPath;

            var archs = PlayerSettings.Android.targetArchitectures;
            return (!archs.HasFlag(AndroidArchitecture.ARMv7) || CompileForArch(AndroidArchitecture.ARMv7, isRelease, ndkPath)) &&
                   (!archs.HasFlag(AndroidArchitecture.ARM64) || CompileForArch(AndroidArchitecture.ARM64, isRelease, ndkPath)) &&
                   (!archs.HasFlag(AndroidArchitecture.X86_64) || CompileForArch(AndroidArchitecture.X86_64, isRelease, ndkPath));

            static bool CompileForArch(AndroidArchitecture arch, bool isRelease, string ndkPath)
            {
                string objPath, outPath, androidNdkPathThing, odnPlatform;
                switch (arch)
                {
                    case AndroidArchitecture.ARMv7:
                        objPath = ODIN_ANDROID_ARMv7_OBJ_PATH;
                        outPath = ODIN_ANDROID_ARMv7_PLUGIN_PATH;
                        androidNdkPathThing = "armv7a";
                        odnPlatform = "linux_arm32";
                        break;
                    case AndroidArchitecture.ARM64:
                        objPath = ODIN_ANDROID_ARMv8_OBJ_PATH;
                        outPath = ODIN_ANDROID_ARMv8_PLUGIN_PATH;
                        androidNdkPathThing = "aarch64";
                        odnPlatform = "linux_arm64";
                        break;
                    case AndroidArchitecture.X86_64:
                        objPath = ODIN_ANDROID_X8664_OBJ_PATH;
                        outPath = ODIN_ANDROID_X8664_PLUGIN_PATH;
                        androidNdkPathThing = "x86_64";
                        odnPlatform = "linux_amd64";
                        break;
                    default:
                        throw new BuildFailedException($"Unsupported Android architecture for OdinInterop: {arch}");
                }

                var l = new List<string>
                {
                    $"-out:{objPath}",
                    $"-target:{odnPlatform}",
                    // "-subtarget:android", armv7/x64 aren't supported by odin so trying it out without that stuff
                    "-build-mode:object",
                    "-no-entry-point",
                };

                var d = new Dictionary<string, string>
                {
                    { "ODIN_ANDROID_NDK", ndkPath }
                };

                var compilationSuccess = RunOdinCompiler(l, extraEnv: d, isRelease: isRelease);
                if (!compilationSuccess) return false;

                const string PLT_TOOLCHAIN_NAME =
#if UNITY_EDITOR_WIN
                    "windows-x86_64";
#elif UNITY_EDITOR_OSX
                    "darwin-x86_64";
#elif UNITY_EDITOR_LINUX
                    "linux-x86_64";
#else
                    "unknown";  
#endif

                // now link to a shared lib
                return RunProcess(
                    "Odin Android Linker",
                    Path.Combine(
                        ndkPath,
                        "toolchains",
                        "llvm",
                        "prebuilt",
                        PLT_TOOLCHAIN_NAME,
                        "bin",
                        "clang"),
                    new List<string>
                    {
                        $"--target={androidNdkPathThing}-linux-android{GetAndroidVersionStr()}",
                        "-shared",
                        "-o", outPath,
                        objPath,
                        $"-L{Path.Combine(ndkPath, "toolchains", "llvm", "prebuilt", PLT_TOOLCHAIN_NAME, "sysroot", "usr", "lib", $"{androidNdkPathThing}-linux-android", GetAndroidVersionStr())}",
                        "-landroid", "-llog",
                        $"--sysroot={Path.Combine(ndkPath, "toolchains", "llvm", "prebuilt", PLT_TOOLCHAIN_NAME, "sysroot")}",
                        "-lm", "-lc",
                        "-Wl,-init,'_odin_entry_point'",
                        "-Wl,-fini,'_odin_exit_point'",
                        "-Wl,-z,max-page-size=16384", // 16kb pages
                    },
                    ODIN_LIB_INPUT_PATH,
                    null
                );
            }

            static string GetAndroidVersionStr()
            {
                switch (PlayerSettings.Android.minSdkVersion)
                {
                    case AndroidSdkVersions.AndroidApiLevelAuto: return "35";
                    case AndroidSdkVersions.AndroidApiLevel23: return "23";
                    case AndroidSdkVersions.AndroidApiLevel24: return "24";
                    case AndroidSdkVersions.AndroidApiLevel25: return "25";
                    case AndroidSdkVersions.AndroidApiLevel26: return "26";
                    case AndroidSdkVersions.AndroidApiLevel27: return "27";
                    case AndroidSdkVersions.AndroidApiLevel28: return "28";
                    case AndroidSdkVersions.AndroidApiLevel29: return "29";
                    case AndroidSdkVersions.AndroidApiLevel30: return "30";
                    case AndroidSdkVersions.AndroidApiLevel31: return "31";
                    case AndroidSdkVersions.AndroidApiLevel32: return "32";
                    case AndroidSdkVersions.AndroidApiLevel33: return "33";
                    case AndroidSdkVersions.AndroidApiLevel34: return "34";
                    case AndroidSdkVersions.AndroidApiLevel35: return "35";
                    case AndroidSdkVersions.AndroidApiLevel36: return "36";
                    default: return "21"; // default to 21 if unknown
                }
            }
        }

        internal static bool CompileOdinInteropLibraryForLinux(bool isRelease)
        {
            if (!Directory.Exists(ODIN_LINUX_PLUGIN_DIR_PATH))
                Directory.CreateDirectory(ODIN_LINUX_PLUGIN_DIR_PATH);

            if (Directory.Exists(ODIN_LINUX_OBJ_TEMP_DIR_PATH))
                Directory.Delete(ODIN_LINUX_OBJ_TEMP_DIR_PATH, true);
            Directory.CreateDirectory(ODIN_LINUX_OBJ_TEMP_DIR_PATH);

            var l = new List<string>
            {
                $"-out:{ODIN_LINUX_OBJ_PATH}",
                $"-target:linux_amd64",
                "-build-mode:object",
                "-no-entry-point",
            };

            var compilationSuccess = RunOdinCompiler(l, isRelease: isRelease);
            if (!compilationSuccess) return false;
            return true;
        }

        private static bool RunOdinCompiler(List<string> args, Dictionary<string, string> extraEnv = null, bool isRelease = false)
        {
            args.Insert(0, "build");
            args.Insert(1, ".");

            args.Add("-min-link-libs");
            args.Add("-use-single-module");
            args.Add("-reloc-mode:pic");
            if (!isRelease)
            {
                args.Add("-debug");
            }
            return RunProcess("OdinCompiler", "odin", args, ODIN_LIB_INPUT_PATH, extraEnv);
        }

        private static bool RunProcess(string sn, string tgt, List<string> args, string wd, Dictionary<string, string> extraEnv)
        {
            var psi = new System.Diagnostics.ProcessStartInfo(tgt);
            psi.WorkingDirectory = wd;

            if (extraEnv != null)
            {
                foreach (var (k, v) in extraEnv)
                {
                    psi.Environment[k] = v;
                }
            }

            psi.ArgumentList.Clear();
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
                Debug.LogFormat(success ? LogType.Log : LogType.Error, LogOption.NoStacktrace, null, "[0]: {1}", sn, s);

            return success;
        }
    }

    internal class OdinCompilerBuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            var isRelease = !report.summary.options.HasFlag(BuildOptions.Development);

            if (report.summary.platform == BuildTarget.StandaloneWindows64)
            {
                if (!OdinCompiler.CompileOdinInteropLibraryForWindows(isRelease))
                {
                    throw new BuildFailedException("Failed to compile OdinInterop library for Windows build.");
                }
            }
            else if (report.summary.platform == BuildTarget.Android)
            {
                if (!OdinCompiler.CompileOdinInteropLibraryForAndroid(isRelease))
                {
                    throw new BuildFailedException("Failed to compile OdinInterop library for Android build.");
                }
            }
        }
    }

    internal class OdinCompilerBuildPostprocessor : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            var l = new List<string>(32);

            if (report.summary.platform == BuildTarget.StandaloneWindows64)
            {
                l.Add(OdinCompiler.ODIN_WINDOWS_PLUGIN_PATH);
                l.Add(OdinCompiler.ODIN_WINDOWS_PLUGIN_EXP_PATH);
                l.Add(OdinCompiler.ODIN_WINDOWS_PLUGIN_PDB_PATH);
                l.Add(OdinCompiler.ODIN_WINDOWS_PLUGIN_STAT_PATH);
            }
            else if (report.summary.platform == BuildTarget.Android)
            {
                l.Add(OdinCompiler.ODIN_ANDROID_ARMv7_PLUGIN_PATH);
                l.Add(OdinCompiler.ODIN_ANDROID_ARMv8_PLUGIN_PATH);
                l.Add(OdinCompiler.ODIN_ANDROID_X8664_PLUGIN_PATH);
            }

            foreach (var path in l)
            {
                if (File.Exists(path))
                    File.Delete(path);

                var meta = path + ".meta";
                if (File.Exists(meta))
                    File.Delete(meta);
            }
        }
    }
}
