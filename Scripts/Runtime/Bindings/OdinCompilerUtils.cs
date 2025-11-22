using System;

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
    }
}
