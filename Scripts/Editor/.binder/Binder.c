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

// KEEP IN SYNC WITH `StoredState` IN `UnityInterfaces.odin` AND `OdinCompiler.cs`!!!!!
typedef struct StoredState
{
    void* unityInterfaces;
    unsigned long long data[127]; // 1KiB of state
} StoredState;

_Static_assert(sizeof(StoredState) == 1024, "StoredState size must be 512 bytes plus pointer size");

StoredState G_StateToStore = {0};

void EXPORT_API INTERFACE_API UnityPluginLoad(void* unityInterfaces) { G_StateToStore.unityInterfaces = unityInterfaces; }

void EXPORT_API INTERFACE_API UnityPluginUnload(void) { G_StateToStore.unityInterfaces = 0; }

unsigned long long EXPORT_API OdinInteropBinder_GetStoredState(void) { return (unsigned long long) &G_StateToStore; }
