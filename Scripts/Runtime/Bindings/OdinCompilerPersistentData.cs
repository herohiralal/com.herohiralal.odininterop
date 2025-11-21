using System;
using UnityEngine;

namespace OdinInterop
{
    public class OdinCompilerPersistentData : ScriptableObject
    {
        private static OdinCompilerPersistentData s_Instance;

        public ulong libraryHandle;

        public static OdinCompilerPersistentData LoadOrCreate()
        {
            if (s_Instance)
            {
                return s_Instance;
            }

            var objs = FindObjectsByType<OdinCompilerPersistentData>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            if (objs.Length > 0)
            {
                s_Instance = objs[0];
                return s_Instance;
            }

            var instance = CreateInstance<OdinCompilerPersistentData>();
            instance.hideFlags = HideFlags.HideAndDontSave;
            s_Instance = instance;
            return s_Instance;
        }

        public static ref ulong staticLibraryHandle => ref LoadOrCreate().libraryHandle;
    }

    public static class OdinCompilerUtils
    {
        private static bool s_InitialisedAfterDomainReload = false;
        public static bool initialisedAfterDomainReload => s_InitialisedAfterDomainReload;

        public static event Action<ulong> onHotReload = delegate { };

        public static void RaiseHotReloadEvt(ulong libH)
        {
            s_InitialisedAfterDomainReload = true;
            onHotReload.Invoke(libH);
        }
    }
}
