#if defined(__CYGWIN32__)
    #define INTERFACE_API __stdcall
    #define EXPORT_API __declspec(dllexport)
#elif defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
    #define INTERFACE_API __stdcall
    #define EXPORT_API __declspec(dllexport)
#elif defined(__MACH__) || defined(__ANDROID__) || defined(__linux__) || defined(LUMIN)
    #define INTERFACE_API
    #define EXPORT_API __attribute__ ((visibility ("default")))
#else
    #define INTERFACE_API
    #define EXPORT_API
#endif

void* G_UnityInterfaces = 0;

#ifdef __cplusplus
extern "C" {
#endif

void EXPORT_API INTERFACE_API UnityPluginLoad(void* unityInterfaces) { G_UnityInterfaces = unityInterfaces; }

void EXPORT_API INTERFACE_API UnityPluginUnload() { G_UnityInterfaces = 0; }

unsigned long long EXPORT_API OdinInteropBinder_GetUnityInterfaces(void) { return (unsigned long long) G_UnityInterfaces; }

#ifdef __cplusplus
}
#endif
