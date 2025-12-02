using UnityEngine;
using System.IO;
using System;
using System.Runtime.InteropServices;

namespace OdinInterop
{
    [AddComponentMenu("")] // hide from add component menu
    [DefaultExecutionOrder(int.MaxValue)] // ensure this runs last
    public class RuntimeReloader : MonoBehaviour
    {

        [SerializeField] private string currentPath = "";
        [SerializeField] private ulong currentTimeStamp = 0;

        private float timer = 0f;

        private void Awake() => HotReload();

        private void LateUpdate()
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= 1.5f) // 1.5s intervals
            {
                timer = 0f;
                HotReload();
            }
        }

        private const string k_FallbackLibraryName =
#if UNITY_ANDROID
            "libOdinInterop.so";
#else
            "Unknown";
#endif

#if !UNITY_EDITOR && ODININTEROP_RUNTIME_RELOADING

        private const string k_OdinInteropDllName =
#if UNITY_IOS
			"__Internal";
#else
            "OdinInterop";
#endif

        [DllImport(k_OdinInteropDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "UnityOdnTropInternalGetUnityInterfaces")]
        private static extern IntPtr GetUnityInterfaces();
#else
        private static IntPtr GetUnityInterfaces() => IntPtr.Zero;
#endif

        private delegate void UnityPluginLoadDelegate(IntPtr unityInterfaces);
        private delegate void UnityPluginUnloadDelegate();

        private void HotReload()
        {
#if !UNITY_EDITOR && ODININTEROP_RUNTIME_RELOADING
            var unsupported = false;
#else
            var unsupported = true;
#endif

            if (unsupported)
                throw new Exception("Runtime reloading is not supported in this build configuration!");

            string path = "";
            ulong timeStamp = 0;
            if (Application.platform == RuntimePlatform.Android)
                path = FindLatestLibraryForAndroid(out timeStamp);

            if (currentTimeStamp >= timeStamp && currentPath != "") // newer current timestamp and valid current path
                return; // no change

            if (OdinCompilerUtils.libraryHandle != IntPtr.Zero)
            {
                var currentPathIsFallback = currentPath == k_FallbackLibraryName;

                if (!currentPathIsFallback)
                {
                    // do not trigger unload callback for the fallback library
                    LibraryUtils.GetDelegate<UnityPluginUnloadDelegate>(OdinCompilerUtils.libraryHandle, "UnityPluginUnload")?.Invoke();
                    LibraryUtils.CloseLibrary(OdinCompilerUtils.libraryHandle);
                }

                OdinCompilerUtils.RaiseHotReloadEvt(IntPtr.Zero);

                try
                {
                    if (File.Exists(currentPath) && !currentPathIsFallback) // delete if not fallback lib
                    {
                        currentPath = "";
                        File.Delete(currentPath);
                        Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "Deleted previous hot-reload lib: {0}", currentPath);
                    }
                }
                catch { }
            }

            currentPath = path;
            currentTimeStamp = timeStamp;
            var libraryHandle = LibraryUtils.OpenLibrary(path);
            // TODO: do some kind of unity native interfaces initialisation here
            if (libraryHandle == IntPtr.Zero)
            {
                Debug.LogError($"Failed to open hot-reload library: {path}");
                return;
            }

            LibraryUtils.GetDelegate<UnityPluginLoadDelegate>(libraryHandle, "UnityPluginLoad")?.Invoke(GetUnityInterfaces());
            OdinCompilerUtils.RaiseHotReloadEvt(libraryHandle);
            Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "Hot-reloaded lib: {0}", path);
        }

        private static string FindLatestLibraryForAndroid(out ulong best)
        {
            best = 0;
            var searchDir = "/data/data/" + Application.identifier + "/files/hotlibs/";
            if (!Directory.Exists(searchDir))
                return k_FallbackLibraryName;

            var files = Directory.GetFiles(searchDir, "libOdinInterop_*.so");
            if (files.Length == 0)
                return k_FallbackLibraryName;

            string latest = k_FallbackLibraryName;

            foreach (var f in files)
            {
                var name = Path.GetFileNameWithoutExtension(f);
                var tsStr = name.Substring("libOdinInterop_".Length);

                if (ulong.TryParse(tsStr, out ulong ts) && ts > best)
                {
                    // delete the previous file
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(latest) && latest != k_FallbackLibraryName)
                        {
                            File.Delete(latest);
                            Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "Deleted: {0}", latest);
                        }
                    }
                    catch { }

                    // update current
                    best = ts;
                    latest = f;
                }
                else
                {
                    // delete the file
                    try
                    {
                        File.Delete(f);
                        Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "Deleted: {0}", f);
                    }
                    catch { }
                }
            }

            return latest;
        }
    }
}
