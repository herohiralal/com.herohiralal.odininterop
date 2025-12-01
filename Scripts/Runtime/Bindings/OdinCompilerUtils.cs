using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OdinInterop
{
    public static class OdinCompilerUtils
    {
        private static bool s_InitialisedAfterDomainReload = false;
        public static bool initialisedAfterDomainReload => s_InitialisedAfterDomainReload;

        public static event Action<IntPtr> onHotReload = delegate { };

        internal static void RaiseHotReloadEvt(IntPtr libH)
        {
            s_InitialisedAfterDomainReload = true;
            libraryHandle = libH;
            onHotReload.Invoke(libH);
        }

        public static IntPtr libraryHandle { get; private set; } = IntPtr.Zero;

#if !UNITY_EDITOR && ODININTEROP_RUNTIME_RELOADING
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeRuntime()
        {
            var go = new GameObject("[OdinInteropRuntimeReloader]", typeof(RuntimeReloader));
            Object.DontDestroyOnLoad(go);
        }
#endif
    }
}
