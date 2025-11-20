#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Runtime.InteropServices;

namespace OdinInterop
{
    public static class LibraryUtils
    {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX

        [DllImport("__Internal")]
        private static extern IntPtr dlopen(string path, int flag);

        [DllImport("__Internal")]
        private static extern IntPtr dlsym(IntPtr handle, string symbolName);

        [DllImport("__Internal")]
        private static extern int dlclose(IntPtr handle);

        public static IntPtr OpenLibrary(string path) => dlopen(path, 0);

        public static void CloseLibrary(IntPtr libraryHandle) => dlclose(libraryHandle);

        public static T GetDelegate<T>(IntPtr libraryHandle, string functionName) where T : class
        {
            var symbol = dlsym(libraryHandle, functionName);
            return IntPtr.Zero == symbol
                ? null
                : Marshal.GetDelegateForFunctionPointer(symbol, typeof(T)) as T;
        }

#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

        [DllImport("kernel32")]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("kernel32")]
        private static extern IntPtr GetProcAddress(IntPtr libraryHandle, string symbolName);

        [DllImport("kernel32")]
        private static extern bool FreeLibrary(IntPtr libraryHandle);

        public static IntPtr OpenLibrary(string path) => LoadLibrary(path);

        public static void CloseLibrary(IntPtr libraryHandle) => FreeLibrary(libraryHandle);

        public static T GetDelegate<T>(IntPtr libraryHandle, string functionName) where T : class
        {
            var symbol = GetProcAddress(libraryHandle, functionName);
            return IntPtr.Zero == symbol
                ? null
                : Marshal.GetDelegateForFunctionPointer(symbol, typeof(T)) as T;
        }

#endif
    }
}
#endif
