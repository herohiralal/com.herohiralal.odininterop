using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OdinInterop.Editor
{
    internal static class OdinCompiler
    {
        private static readonly string ODIN_LIB_INPUT_PATH = InteropGenerator.ODIN_INTEROP_OUT_DIR;
        private static readonly string ODIN_LIB_OUTPUT_DIR_PATH = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Library", "OdinInterop"));

        // editor stuff
        private static readonly string ODIN_LIB_EDITOR_OUTPUT_DIR_PATH = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, "Editor");

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
        public static readonly string ODIN_ANDROID_RUNTIME_RELOAD_PLUGIN_DIR_PATH = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, "AndroidRuntime");
        public static readonly string ODIN_ANDROID_RUNTIME_RELOAD_ARMv8_PLUGIN_DIR_PATH = Path.Combine(ODIN_ANDROID_RUNTIME_RELOAD_PLUGIN_DIR_PATH, "aarch64");
        public static readonly string ODIN_ANDROID_RUNTIME_RELOAD_ARMv8_PLUGIN_PATH = Path.Combine(ODIN_ANDROID_RUNTIME_RELOAD_ARMv8_PLUGIN_DIR_PATH, "libOdinInterop.so");

        // linux
        public static readonly string ODIN_LINUX_OBJ_TEMP_DIR_PATH = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, "Linux");
        public static readonly string ODIN_LINUX_OBJ_PATH = Path.Combine(ODIN_LINUX_OBJ_TEMP_DIR_PATH, "OdinInterop.o");
        public static readonly string ODIN_LINUX_PLUGIN_DIR_PATH = Path.Combine(ODIN_PLUGIN_DIR_PATH, "Linux", "x86_64");
        public static readonly string ODIN_LINUX_PLUGIN_PATH = Path.Combine(ODIN_LINUX_PLUGIN_DIR_PATH, "libOdinInterop.so");
        public static readonly string ODIN_LINUX_SYSROOT_EXTRACT_PATH = Path.Combine(ODIN_LINUX_OBJ_TEMP_DIR_PATH, "sysroot");
        public static readonly string ODIN_LINUX_EXECS_EXTRACT_PATH = Path.Combine(ODIN_LINUX_OBJ_TEMP_DIR_PATH, "execs");

        // macos
        public static readonly string ODIN_OSX_SO_TEMP_DIR_PATH = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, "OSX");
        public static readonly string ODIN_OSX_ARM64_SO_TEMP_DIR_PATH = Path.Combine(ODIN_OSX_SO_TEMP_DIR_PATH, "arm64");
        public static readonly string ODIN_OSX_X8664_SO_TEMP_DIR_PATH = Path.Combine(ODIN_OSX_SO_TEMP_DIR_PATH, "x86_64");
        public static readonly string ODIN_OSX_ARM64_SO_PATH = Path.Combine(ODIN_OSX_ARM64_SO_TEMP_DIR_PATH, "libOdinInterop.dylib");
        public static readonly string ODIN_OSX_X8664_SO_PATH = Path.Combine(ODIN_OSX_X8664_SO_TEMP_DIR_PATH, "libOdinInterop.dylib");
        public static readonly string ODIN_OSX_PLUGIN_DIR_PATH = Path.Combine(ODIN_PLUGIN_DIR_PATH, "macOS");
        public static readonly string ODIN_OSX_FAT_PLUGIN_PATH = Path.Combine(ODIN_OSX_PLUGIN_DIR_PATH, "libOdinInterop.dylib");

        // ios
        public static readonly string ODIN_IOS_OBJ_TEMP_DIR_PATH = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, "iOS");
        public static readonly string ODIN_IOS_OBJ_PATH = Path.Combine(ODIN_IOS_OBJ_TEMP_DIR_PATH, "OdinInterop.o");
        public static readonly string ODIN_IOS_LIB_PATH = Path.Combine(ODIN_IOS_OBJ_TEMP_DIR_PATH, "libOdinInterop.a");
        public static readonly string ODIN_IOS_SIM_OBJ_TEMP_DIR_PATH = Path.Combine(ODIN_LIB_OUTPUT_DIR_PATH, "iOS_Simulator");
        public static readonly string ODIN_IOS_SIM_ARM64_OBJ_TEMP_DIR_PATH = Path.Combine(ODIN_IOS_SIM_OBJ_TEMP_DIR_PATH, "arm64");
        public static readonly string ODIN_IOS_SIM_ARM64_OBJ_PATH = Path.Combine(ODIN_IOS_SIM_ARM64_OBJ_TEMP_DIR_PATH, "OdinInterop.o");
        public static readonly string ODIN_IOS_SIM_ARM64_LIB_PATH = Path.Combine(ODIN_IOS_SIM_ARM64_OBJ_TEMP_DIR_PATH, "libOdinInterop.a");
        public static readonly string ODIN_IOS_SIM_X8664_OBJ_TEMP_DIR_PATH = Path.Combine(ODIN_IOS_SIM_OBJ_TEMP_DIR_PATH, "x86_64");
        public static readonly string ODIN_IOS_SIM_X8664_OBJ_PATH = Path.Combine(ODIN_IOS_SIM_X8664_OBJ_TEMP_DIR_PATH, "OdinInterop.o");
        public static readonly string ODIN_IOS_SIM_X8664_LIB_PATH = Path.Combine(ODIN_IOS_SIM_X8664_OBJ_TEMP_DIR_PATH, "libOdinInterop.a");
        public static readonly string ODIN_IOS_PLUGIN_DIR_PATH = Path.Combine(ODIN_PLUGIN_DIR_PATH, "iOS");
        public static readonly string ODIN_IOS_PLUGIN_PATH = Path.Combine(ODIN_IOS_PLUGIN_DIR_PATH, "libOdinInterop.a");

        private unsafe struct RawStoredState
        {
            public fixed ulong data[128]; // 1KiB of state
        }

        private unsafe struct StoredState
        {
            /**
             * any change to this must be accompanied with restarting the editor
             * because this is stored in a weird way inside the binder C code
             * Also KEEP IN SYNC WITH `UnityInterfaces.odin` AND `Binder.c`!!!!!
             */

            public IntPtr unityInterfaces;
            public IntPtr libHandle;

            static StoredState()
            {
                if (sizeof(StoredState) > sizeof(RawStoredState))
                {
                    throw new Exception("StoredState size exceeds RawStoredState size. Please update accordingly.");
                }
            }

            [DllImport("OdinInteropBinder", CallingConvention = CallingConvention.Cdecl, EntryPoint = "OdinInteropBinder_GetStoredState")]
            public static extern RawStoredState* GetPtr();

            public static ref StoredState value => ref *(StoredState*)GetPtr();
        }

#if !ODININTEROP_DISABLED
        [InitializeOnLoadMethod]
#endif
        private static void InitialiseEditor()
        {
            // remove if there's a loaded library from before
            if (StoredState.value.libHandle != IntPtr.Zero)
            {
                LibraryUtils.CloseLibrary(StoredState.value.libHandle);
            }

            HotReload();
        }

        [MenuItem("Tools/Odin Interop/Hot Reload %&R")]
        public static unsafe void HotReload()
        {
            if (OdinCompilerUtils.libraryHandle != IntPtr.Zero)
            {
                LibraryUtils.CloseLibrary(OdinCompilerUtils.libraryHandle);
                OdinCompilerUtils.RaiseHotReloadEvt(IntPtr.Zero);
            }

            InteropGenerator.GenerateInteropCode();
            if (!CompileOdinInteropLibraryForEditor(out var libPath))
            {
                Debug.LogError("[OdinCompiler]: Failed to compile OdinInteropEditor library. No active library present.");
                return;
            }

            var libraryHandle = LibraryUtils.OpenLibrary(libPath);
            StoredState.value.libHandle = libraryHandle;
            if (libraryHandle == IntPtr.Zero)
            {
                Debug.LogError("[OdinCompiler]: Failed to load compiled OdinInteropEditor library. No active library present.");
                return;
            }

            LibraryUtils.GetDelegate<SetUnityInterfacesPtrDelegate>(libraryHandle, "UnityOdnTropInternalInitialiseForEditor")?.Invoke(StoredState.GetPtr());
            OdinCompilerUtils.RaiseHotReloadEvt(libraryHandle);
        }

        private unsafe delegate void SetUnityInterfacesPtrDelegate(RawStoredState* ptr);


        internal static bool CompileOdinInteropLibraryForEditor(out string path)
        {
            if (!Directory.Exists(ODIN_LIB_EDITOR_OUTPUT_DIR_PATH))
                Directory.CreateDirectory(ODIN_LIB_EDITOR_OUTPUT_DIR_PATH);

            const string FILE_PREFIX =
#if UNITY_EDITOR_WIN
                "OdinInteropEditor";
#else
                "libOdinInteropEditor";
#endif

            const string FILE_EXTENSION =
#if UNITY_EDITOR_WIN
                ".dll";
#elif UNITY_EDITOR_OSX
                ".dylib";
#elif UNITY_EDITOR_LINUX
                ".so";
#endif

            // delete all previous builds
            // but not just limited to the target extension because windows also outputs .exp/.lib/.pdb files
            foreach (var f in Directory.GetFiles(ODIN_LIB_EDITOR_OUTPUT_DIR_PATH, $"{FILE_PREFIX}*"))
                try { File.Delete(f); } catch { /* ignore */ }

            // make a path with timestamp
            path = Path.Combine(ODIN_LIB_EDITOR_OUTPUT_DIR_PATH, $"{FILE_PREFIX}_{DateTime.Now:yyyyMMddHHmmss}{FILE_EXTENSION}");

            var compilerSuccess = RunOdinCompiler(new List<string>
            {
                $"-out:{path}",
                "-build-mode:dynamic",
#if UNITY_EDITOR
                "-define:UNITY_EDITOR=true",
#if false
#elif UNITY_EDITOR_WIN
                "-define:UNITY_EDITOR_WIN=true",
#elif UNITY_EDITOR_OSX
                "-define:UNITY_EDITOR_OSX=true",
#elif UNITY_EDITOR_LINUX
                "-define:UNITY_EDITOR_LINUX=true",
#endif

#endif

#if false
#elif UNITY_ANDROID
                "-define:UNITY_ANDROID=true",
#elif UNITY_IOS
                "-define:UNITY_IOS=true",
#elif UNITY_STANDALONE
                "-define:UNITY_STANDALONE=true",

#if false
#elif UNITY_STANDALONE_WIN
                "-define:UNITY_STANDALONE_WIN=true",
#elif UNITY_STANDALONE_OSX
                "-define:UNITY_STANDALONE_OSX=true",
#elif UNITY_STANDALONE_LINUX
                "-define:UNITY_STANDALONE_LINUX=true",
#endif

#endif
            });

            return compilerSuccess;
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
                "-define:UNITY_STANDALONE=true",
                "-define:UNITY_STANDALONE_WIN=true",
            };

            return RunOdinCompiler(l, isRelease: isRelease);
        }

        internal static bool CompileOdinInteropLibraryForMacOS(bool isRelease)
        {
            if (!Directory.Exists(ODIN_OSX_PLUGIN_DIR_PATH))
                Directory.CreateDirectory(ODIN_OSX_PLUGIN_DIR_PATH);

            if (Directory.Exists(ODIN_OSX_ARM64_SO_TEMP_DIR_PATH))
                Directory.Delete(ODIN_OSX_ARM64_SO_TEMP_DIR_PATH, true);
            Directory.CreateDirectory(ODIN_OSX_ARM64_SO_TEMP_DIR_PATH);

            if (Directory.Exists(ODIN_OSX_X8664_SO_TEMP_DIR_PATH))
                Directory.Delete(ODIN_OSX_X8664_SO_TEMP_DIR_PATH, true);
            Directory.CreateDirectory(ODIN_OSX_X8664_SO_TEMP_DIR_PATH);

            var arm64Success = RunOdinCompiler(new List<string>
            {
                $"-out:{ODIN_OSX_ARM64_SO_PATH}",
                "-target:darwin_arm64",
                "-build-mode:dynamic",
                "-define:UNITY_STANDALONE=true",
                "-define:UNITY_STANDALONE_OSX=true",
            }, isRelease: isRelease);

            var x8664Success = RunOdinCompiler(new List<string>
            {
                $"-out:{ODIN_OSX_X8664_SO_PATH}",
                "-target:darwin_amd64",
                "-build-mode:dynamic",
                "-define:UNITY_STANDALONE=true",
                "-define:UNITY_STANDALONE_OSX=true",
            }, isRelease: isRelease);

            if (!arm64Success || !x8664Success) return false;

            // now create fat binary
            return RunProcess(
                "Odin macOS Fat Binary Creator",
                "lipo",
                new List<string>
                {
                    "-create",
                    "-output", ODIN_OSX_FAT_PLUGIN_PATH,
                    ODIN_OSX_ARM64_SO_PATH,
                    ODIN_OSX_X8664_SO_PATH,
                },
                null,
                ODIN_OSX_SO_TEMP_DIR_PATH,
                null
            );
        }

        internal static bool CompileOdinInteropLibraryForIOS(bool isRelease)
        {
            if (!Directory.Exists(ODIN_IOS_PLUGIN_DIR_PATH))
                Directory.CreateDirectory(ODIN_IOS_PLUGIN_DIR_PATH);

            var isSimulator = PlayerSettings.iOS.sdkVersion == iOSSdkVersion.SimulatorSDK;
            if (!isSimulator) // actual device
            {
                if (Directory.Exists(ODIN_IOS_OBJ_TEMP_DIR_PATH))
                    Directory.Delete(ODIN_IOS_OBJ_TEMP_DIR_PATH, true);
                Directory.CreateDirectory(ODIN_IOS_OBJ_TEMP_DIR_PATH);

                var l = new List<string>
                {
                    $"-out:{ODIN_IOS_OBJ_PATH}",
                    "-target:darwin_arm64",
                    "-build-mode:object",
                    "-subtarget:iphone",
                    "-define:UNITY_IOS=true",
                };

                var compilationSuccess = RunOdinCompiler(l, isRelease: isRelease);
                if (!compilationSuccess) return false;

                var archival = RunProcess(
                    "Odin iOS Archiver",
                    "ar",
                    new List<string>
                    {
                        "rcs",
                        ODIN_IOS_LIB_PATH,
                        ODIN_IOS_OBJ_PATH,
                    },
                    null,
                    ODIN_IOS_OBJ_TEMP_DIR_PATH,
                    null
                );

                if (!archival) return false;

                return RunProcess(
                    "Odin iOS Lipo",
                    "lipo",
                    new List<string>
                    {
                        "-create",
                        "-output", ODIN_IOS_PLUGIN_PATH,
                        ODIN_IOS_LIB_PATH,
                    },
                    null, ODIN_IOS_OBJ_TEMP_DIR_PATH,
                    null
                );
            }
            else
            {
                if (Directory.Exists(ODIN_IOS_SIM_ARM64_OBJ_TEMP_DIR_PATH))
                    Directory.Delete(ODIN_IOS_SIM_ARM64_OBJ_TEMP_DIR_PATH, true);
                Directory.CreateDirectory(ODIN_IOS_SIM_ARM64_OBJ_TEMP_DIR_PATH);

                if (Directory.Exists(ODIN_IOS_SIM_X8664_OBJ_TEMP_DIR_PATH))
                    Directory.Delete(ODIN_IOS_SIM_X8664_OBJ_TEMP_DIR_PATH, true);
                Directory.CreateDirectory(ODIN_IOS_SIM_X8664_OBJ_TEMP_DIR_PATH);

                var arm64L = new List<string>
                {
                    $"-out:{ODIN_IOS_SIM_ARM64_OBJ_PATH}",
                    "-target:darwin_arm64",
                    "-build-mode:object",
                    "-subtarget:iphonesimulator",
                    "-define:UNITY_IOS=true",
                };

                var arm64CompilationSuccess = RunOdinCompiler(arm64L, isRelease: isRelease);
                if (!arm64CompilationSuccess) return false;

                var x8664L = new List<string>
                {
                    $"-out:{ODIN_IOS_SIM_X8664_OBJ_PATH}",
                    "-target:darwin_amd64",
                    "-build-mode:object",
                    "-subtarget:iphonesimulator",
                    "-define:UNITY_IOS=true",
                };

                var x8664CompilationSuccess = RunOdinCompiler(x8664L, isRelease: isRelease);
                if (!x8664CompilationSuccess) return false;

                var arm64Archival = RunProcess(
                    "Odin iOS Simulator ARM64 Archiver",
                    "ar",
                    new List<string>
                    {
                        "rcs",
                        ODIN_IOS_SIM_ARM64_LIB_PATH,
                        ODIN_IOS_SIM_ARM64_OBJ_PATH,
                    },
                    null,
                    ODIN_IOS_SIM_ARM64_OBJ_TEMP_DIR_PATH,
                    null
                );

                if (!arm64Archival) return false;

                var x8664Archival = RunProcess(
                    "Odin iOS Simulator x86_64 Archiver",
                    "ar",
                    new List<string>
                    {
                        "rcs",
                        ODIN_IOS_SIM_X8664_LIB_PATH,
                        ODIN_IOS_SIM_X8664_OBJ_PATH,
                    },
                    null,
                    ODIN_IOS_SIM_X8664_OBJ_TEMP_DIR_PATH,
                    null
                );

                if (!x8664Archival) return false;

                return RunProcess(
                    "Odin iOS Simulator Lipo",
                    "lipo",
                    new List<string>
                    {
                        "-create",
                        "-output", ODIN_IOS_PLUGIN_PATH,
                        ODIN_IOS_SIM_ARM64_LIB_PATH,
                        ODIN_IOS_SIM_X8664_LIB_PATH,
                    },
                    null,
                    ODIN_IOS_SIM_OBJ_TEMP_DIR_PATH,
                    null
                );
            }
        }

        [MenuItem("Tools/Odin Interop/Push ARM64 Library to Android Device")]
        private static void PushFileForAndroid() => PushFileForAndroidInternal();

        private static bool PushFileForAndroidInternal()
        {
#if UNITY_ANDROID_API
            var sdkPath = AndroidExternalToolsSettings.sdkRootPath;
#else
            var sdkPath = "";
#endif

            if (string.IsNullOrWhiteSpace(sdkPath))
            {
                Debug.LogError("[OdinCompiler]: Android SDK path could not be determined. Please ensure the Android Build Support module is installed via the Unity Hub.");
                return false;
            }

            if (Directory.Exists(ODIN_ANDROID_RUNTIME_RELOAD_ARMv8_PLUGIN_DIR_PATH))
                Directory.Delete(ODIN_ANDROID_RUNTIME_RELOAD_ARMv8_PLUGIN_DIR_PATH, true);

            Directory.CreateDirectory(ODIN_ANDROID_RUNTIME_RELOAD_ARMv8_PLUGIN_DIR_PATH);

            InteropGenerator.GenerateInteropCode();
            if (!CompileOdinInteropLibraryForAndroid(false, true))
            {
                Debug.LogError("[OdinCompiler]: Failed to compile OdinInterop library for Android. Cannot push file to device.");
                return false;
            }

            var bundleId = PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.Android);

            if (!File.Exists(ODIN_ANDROID_RUNTIME_RELOAD_ARMv8_PLUGIN_PATH))
            {
                Debug.LogError($"[OdinCompiler]: Expected compiled Android library not found at path: {ODIN_ANDROID_RUNTIME_RELOAD_ARMv8_PLUGIN_PATH}. Cannot push file to device.");
                return false;
            }

            var adbPath = Path.Combine(sdkPath, "platform-tools", "adb");
            var copyToTmp = RunProcess(
                "Odin Android ADB Push to /data/local/tmp",
                adbPath,
                new List<string>
                {
                    "push",
                    ODIN_ANDROID_RUNTIME_RELOAD_ARMv8_PLUGIN_PATH,
                    "/data/local/tmp/libOdinInterop.so"
                },
                null,
                ODIN_LIB_INPUT_PATH,
                null
            );


            if (!copyToTmp)
            {
                Debug.LogError($"[OdinCompiler]: Failed to push {ODIN_ANDROID_RUNTIME_RELOAD_ARMv8_PLUGIN_PATH} to Android device /data/local/tmp. Ensure device is connected and adb is authorized.");
                return false;
            }

            var mkHotLibDir = RunProcess(
                "Odin Android ADB Make app hot lib dir",
                adbPath,
                new List<string>
                {
                    "shell",
                    "run-as",
                    bundleId,
                    "mkdir",
                    "-p",
                    "files/hotlibs"
                },
                null,
                ODIN_LIB_INPUT_PATH,
                null
            );

            if (!mkHotLibDir)
            {
                Debug.LogError($"[OdinCompiler]: Failed to create hotlib directory on Android device for app {bundleId}.");
                return false;
            }

            var copyToHotLibDir = RunProcess(
                "Odin Android ADB Move to app hot lib dir",
                adbPath,
                new List<string>
                {
                    "shell",
                    "run-as",
                    bundleId,
                    "cp",
                    "/data/local/tmp/libOdinInterop.so",
                    $"files/hotlibs/libOdinInterop_{DateTime.Now:yyyyMMddHHmmss}.so"
                },
                null,
                ODIN_LIB_INPUT_PATH,
                null
            );

            if (!copyToHotLibDir)
            {
                Debug.LogError($"[OdinCompiler]: Failed to move cool.txt to hotlib directory on Android device for app {bundleId}.");
                return false;
            }

            return true;
        }

        internal static bool CompileOdinInteropLibraryForAndroid(bool isRelease, bool forRuntimeReloading)
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

#if UNITY_ANDROID_API
            var ndkPath = AndroidExternalToolsSettings.ndkRootPath;
#else
            var ndkPath = "";
#endif

            if (string.IsNullOrWhiteSpace(ndkPath))
                throw new BuildFailedException("Android NDK path could not be determined. Please ensure the Android Build Support module is installed via the Unity Hub.");

            var archs = PlayerSettings.Android.targetArchitectures;
            return (!archs.HasFlag(AndroidArchitecture.ARMv7) || forRuntimeReloading || CompileForArch(AndroidArchitecture.ARMv7, isRelease, ndkPath, false)) &&
                   (!(archs.HasFlag(AndroidArchitecture.ARM64) || forRuntimeReloading) || CompileForArch(AndroidArchitecture.ARM64, isRelease, ndkPath, forRuntimeReloading)) &&
                   (!archs.HasFlag(AndroidArchitecture.X86_64) || forRuntimeReloading || CompileForArch(AndroidArchitecture.X86_64, isRelease, ndkPath, false));

            static bool CompileForArch(AndroidArchitecture arch, bool isRelease, string ndkPath, bool forRuntimeReloading)
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
                        outPath = forRuntimeReloading ? ODIN_ANDROID_RUNTIME_RELOAD_ARMv8_PLUGIN_PATH : ODIN_ANDROID_ARMv8_PLUGIN_PATH;
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
                    "-define:UNITY_ANDROID=true",
                };

                if (forRuntimeReloading)
                {
                    l.Add("-define:ODNTROP_INTERNAL_RUNTIME_RELOADING=true");
                }

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
                    null,
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
            Directory.CreateDirectory(ODIN_LINUX_SYSROOT_EXTRACT_PATH);
            Directory.CreateDirectory(ODIN_LINUX_EXECS_EXTRACT_PATH);

            var l = new List<string>
            {
                $"-out:{ODIN_LINUX_OBJ_PATH}",
                $"-target:linux_amd64",
                "-build-mode:object",
                "-no-entry-point",
                "-define:UNITY_STANDALONE=true",
                "-define:UNITY_STANDALONE_LINUX=true",
            };

            var compilationSuccess = RunOdinCompiler(l, isRelease: isRelease);
            if (!compilationSuccess) return false;

            if (!SetupLinuxSysroot(out var sysrootPath, out var execsPath))
            {
                Debug.LogError("[OdinCompiler]: Failed to setup Linux sysroot and execs for OdinInterop compilation.");
                return false;
            }

            return RunProcess(
                "Odin Linux Linker",
                Path.Combine(execsPath, "bin", "clang++"),
                new List<string>
                {
                    $"--sysroot={sysrootPath}",
                    $"--target=x86_64-glibc2.17-linux-gnu",
                    "-shared",
                    "-o", ODIN_LINUX_PLUGIN_PATH,
                    ODIN_LINUX_OBJ_PATH,
                    $"-fuse-ld={Path.Combine(execsPath, "bin", "ld.lld")}",
                    "-Wl,-init,'_odin_entry_point'",
                    "-Wl,-fini,'_odin_exit_point'",
                },
                null,
                ODIN_LIB_INPUT_PATH,
                null
            );
        }

        private static bool SetupLinuxSysroot(out string sysrootPath, out string execsPath)
        {
            sysrootPath = ODIN_LINUX_SYSROOT_EXTRACT_PATH;
            execsPath = ODIN_LINUX_EXECS_EXTRACT_PATH;

            var sysrootPkg = Path.GetFullPath($"Packages/com.unity.sysroot.linux-x86_64");
            var arch = RuntimeInformation.OSArchitecture;
            var isArm = (arch == Architecture.Arm64 || arch == Architecture.Arm);

#if UNITY_EDITOR_WIN
            var execsPkg = Path.GetFullPath($"Packages/{(isArm ? "com.unity.toolchain.win-arm64-linux-x86_64" : "com.unity.toolchain.win-x86_64-linux-x86_64")}");
#elif UNITY_EDITOR_LINUX
            var execsPkg = Path.GetFullPath($"Packages/com.unity.toolchain.linux-x86_64");
#elif UNITY_EDITOR_OSX
            var execsPkg = Path.GetFullPath($"Packages/{(isArm ? "com.unity.toolchain.macos-arm64-linux-x86_64" : "com.unity.toolchain.macos-x86_64-linux-x86_64")}");
#endif

            var sysrootTarBall = Path.Combine(sysrootPkg, "data~", "payload.tar.7z");
            var execsTarBall = Path.Combine(execsPkg, "data~", "payload.tar.7z");

            var srdecomp = RunTarballDecompressionProcess("Odin Linux Sysroot Decompressor", sysrootTarBall, sysrootPath);
            if (!srdecomp) return false;

            var exdecomp = RunTarballDecompressionProcess("Odin Linux Execs Decompressor", execsTarBall, execsPath);
            if (!exdecomp) return false;

            return true;
        }

        private static bool RunOdinCompiler(List<string> args, Dictionary<string, string> extraEnv = null, bool isRelease = false)
        {
            args.Insert(0, "build");
            args.Insert(1, ".");

            args.Add("-min-link-libs");
            args.Add("-use-single-module");
            args.Add("-reloc-mode:pic");
            args.Add("-error-pos-style:unix");
            if (!isRelease)
            {
                args.Add("-debug");
            }

            return RunProcess(
                "OdinCompiler",
#if UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX
                "odin",
#elif UNITY_EDITOR_OSX
                "/opt/homebrew/bin/odin",
#else
                "unknown",
#endif
                args, null, ODIN_LIB_INPUT_PATH, extraEnv);
        }

        private static bool RunTarballDecompressionProcess(string sn, string tarball, string extractPath)
        {
#if UNITY_EDITOR_WIN
            var sevenZipName = "7z.exe";
#else
            var sevenZipName = "7za";
#endif
            var _7zPath = Path.GetFullPath($"{EditorApplication.applicationContentsPath}/Tools/{sevenZipName}");
            if (_7zPath.EndsWith("/") || _7zPath.EndsWith("\\"))
                _7zPath = _7zPath.Substring(0, _7zPath.Length - 1);

            return RunProcess(
                sn,
#if UNITY_EDITOR_WIN
                "cmd",
                null,
                $"/c \"\"{_7zPath}\" x -y \"{tarball}\" -so | \"{_7zPath}\" x -y -aoa -ttar -si\"",

#else
                "/bin/sh",
                null,
                $"-c \'\"{_7zPath}\" x -y \"{tarball}\" -so | tar xf - --directory=\"{extractPath}\"\'",
#endif
                extractPath,
                null
            );
        }

        private static bool RunProcess(string sn, string tgt, List<string> args, string argsManual, string wd, Dictionary<string, string> extraEnv)
        {
            try
            {
                return RunProcessInternal(sn, tgt, args, argsManual, wd, extraEnv);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        private static bool RunProcessInternal(string sn, string tgt, List<string> args, string argsManual, string wd, Dictionary<string, string> extraEnv)
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
            if (args != null)
                foreach (var arg in args)
                    psi.ArgumentList.Add(arg);

            if (!string.IsNullOrWhiteSpace(argsManual))
                psi.Arguments = argsManual;

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
                Debug.LogFormat(success ? LogType.Log : LogType.Error, LogOption.NoStacktrace, null, "[{0}]: {1}", sn, s);

            return success;
        }

        [MenuItem("Tools/Odin Interop/Build Libraries for All Platforms (Debug)")]
        private static void BuildAllPlatformDebugDlls()
        {
#if UNITY_EDITOR_WIN
            CompileOdinInteropLibraryForWindows(isRelease: false);
#endif
            CompileOdinInteropLibraryForLinux(isRelease: false);
            CompileOdinInteropLibraryForAndroid(isRelease: false, forRuntimeReloading: false);
#if UNITY_EDITOR_OSX
            CompileOdinInteropLibraryForMacOS(isRelease: false);
            CompileOdinInteropLibraryForIOS(isRelease: false);
#endif
        }

        [MenuItem("Tools/Odin Interop/Build Libraries for All Platforms (Release)")]
        private static void BuildAllPlatformReleaseDlls()
        {
#if UNITY_EDITOR_WIN
            CompileOdinInteropLibraryForWindows(isRelease: true);
#endif
            CompileOdinInteropLibraryForLinux(isRelease: true);
            CompileOdinInteropLibraryForAndroid(isRelease: true, forRuntimeReloading: false);
#if UNITY_EDITOR_OSX
            CompileOdinInteropLibraryForMacOS(isRelease: true);
            CompileOdinInteropLibraryForIOS(isRelease: true);
#endif
        }

        [MenuItem("Tools/Odin Interop/Clean Built Libraries")]
        internal static void CleanBuiltLibraries()
        {
            var l = new List<string>(32)
            {
                // windows
                ODIN_WINDOWS_PLUGIN_PATH,
                ODIN_WINDOWS_PLUGIN_EXP_PATH,
                ODIN_WINDOWS_PLUGIN_PDB_PATH,
                ODIN_WINDOWS_PLUGIN_STAT_PATH,
                // android
                ODIN_ANDROID_ARMv7_PLUGIN_PATH,
                ODIN_ANDROID_ARMv8_PLUGIN_PATH,
                ODIN_ANDROID_X8664_PLUGIN_PATH,
                // linux
                ODIN_LINUX_PLUGIN_PATH,
                // macOS
                ODIN_OSX_FAT_PLUGIN_PATH,
                // iOS
                ODIN_IOS_PLUGIN_PATH
            };

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

    internal class OdinCompilerBuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            try
            {
#if !ODININTEROP_DISABLED
                PreprocessInternal(report);
#endif
            }
            catch (Exception)
            {
                // do something?
                throw;
            }
        }

        private void PreprocessInternal(BuildReport report)
        {
            var isRelease = !report.summary.options.HasFlag(BuildOptions.Development);

            if (report.summary.platform == BuildTarget.StandaloneWindows64)
            {
#if UNITY_EDITOR_WIN
                if (!OdinCompiler.CompileOdinInteropLibraryForWindows(isRelease))
                {
                    throw new BuildFailedException("Failed to compile OdinInterop library for Windows build.");
                }
#else
                throw new BuildFailedException("OdinInterop Windows builds can only be performed on Windows hosts.");
#endif
            }
            else if (report.summary.platform == BuildTarget.Android)
            {
                if (!OdinCompiler.CompileOdinInteropLibraryForAndroid(isRelease, false))
                {
                    throw new BuildFailedException("Failed to compile OdinInterop library for Android build.");
                }
            }
            else if (report.summary.platform == BuildTarget.StandaloneLinux64)
            {
                if (!OdinCompiler.CompileOdinInteropLibraryForLinux(isRelease))
                {
                    throw new BuildFailedException("Failed to compile OdinInterop library for Linux build.");
                }
            }
            else if (report.summary.platform == BuildTarget.StandaloneOSX)
            {
#if UNITY_EDITOR_OSX
                if (!OdinCompiler.CompileOdinInteropLibraryForMacOS(isRelease))
                {
                    throw new BuildFailedException("Failed to compile OdinInterop library for macOS build.");
                }
#else
                throw new BuildFailedException("OdinInterop macOS builds can only be performed on macOS hosts.");
#endif
            }
            else if (report.summary.platform == BuildTarget.iOS)
            {
#if UNITY_EDITOR_OSX
                if (!OdinCompiler.CompileOdinInteropLibraryForIOS(isRelease))
                {
                    throw new BuildFailedException("Failed to compile OdinInterop library for iOS build.");
                }
#else
                throw new BuildFailedException("OdinInterop iOS builds can only be performed on macOS hosts.");
#endif
            }
            else
            {
                throw new BuildFailedException($"OdinInterop does not support builds for platform: {report.summary.platform}.");
            }
        }
    }

    internal class OdinCompilerBuildPostprocessor : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;
        public void OnPostprocessBuild(BuildReport report)
        {
            try
            {
#if !ODININTEROP_DISABLED
                PostprocessInternal(report);
#endif
            }
            catch (Exception)
            {
                // do something?
                throw;
            }
        }

        private void PostprocessInternal(BuildReport report) => OdinCompiler.CleanBuiltLibraries();
    }
}
