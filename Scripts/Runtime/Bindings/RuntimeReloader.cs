using UnityEngine;
using System.IO;
using System;

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
            if (timer >= 1f) // 1s intervals
            {
                timer = 0f;
                HotReload();
            }
        }

        private void HotReload()
        {
            string path;
            ulong timeStamp;
            if (Application.platform == RuntimePlatform.Android)
                path = FindLatestLibraryForAndroid(out timeStamp);
            else
                throw new Exception("Not a hot-reloadable platform!"); // UNSUPPORTED PLATFORM

            if (currentTimeStamp >= timeStamp)
                return; // no change

            if (OdinCompilerUtils.libraryHandle != IntPtr.Zero)
            {
                LibraryUtils.CloseLibrary(OdinCompilerUtils.libraryHandle);
                OdinCompilerUtils.RaiseHotReloadEvt(IntPtr.Zero);

                try
                {
                    if (File.Exists(currentPath))
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

            OdinCompilerUtils.RaiseHotReloadEvt(libraryHandle);
            Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "Hot-reloaded lib: {0}", path);
        }

        private static string FindLatestLibraryForAndroid(out ulong best)
        {
            best = 0;
            var searchDir = "/data/data/" + Application.identifier + "/files/hotlibs/";
            if (!Directory.Exists(searchDir))
                return null;

            var files = Directory.GetFiles(searchDir, "libOdinInterop_*.so");
            if (files.Length == 0)
                return null;

            string latest = null;

            foreach (var f in files)
            {
                var name = Path.GetFileNameWithoutExtension(f);
                var tsStr = name.Substring("libOdinInterop_".Length);

                if (ulong.TryParse(tsStr, out ulong ts) && ts > best)
                {
                    // delete the previous file
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(latest))
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
