using UnityEngine;

namespace OdinInterop
{
    internal static partial class EngineBindings
    {
        private static String8 GetApplicationBuildGUID(Allocator allocator) => new String8(Application.buildGUID, allocator);
        private static String8 GetApplicationCompanyName(Allocator allocator) => new String8(Application.companyName, allocator);
        private static String8 GetApplicationDataPath(Allocator allocator) => new String8(Application.dataPath, allocator);
        private static String8 GetApplicationIdentifier(Allocator allocator) => new String8(Application.identifier, allocator);
        private static String8 GetApplicationInstallerName(Allocator allocator) => new String8(Application.installerName, allocator);
        private static String8 GetApplicationPersistentDataPath(Allocator allocator) => new String8(Application.persistentDataPath, allocator);
        private static String8 GetApplicationProductName(Allocator allocator) => new String8(Application.productName, allocator);
        private static String8 GetApplicationStreamingAssetsPath(Allocator allocator) => new String8(Application.streamingAssetsPath, allocator);
        private static String8 GetApplicationTemporaryCachePath(Allocator allocator) => new String8(Application.temporaryCachePath, allocator);
        private static String8 GetApplicationUnityVersion(Allocator allocator) => new String8(Application.unityVersion, allocator);
        private static String8 GetApplicationVersion(Allocator allocator) => new String8(Application.version, allocator);
        private static String8 GetApplicationConsoleLogPath(Allocator allocator) => new String8(Application.consoleLogPath, allocator);
        private static String8 GetApplicationAbsoluteURL(Allocator allocator) => new String8(Application.absoluteURL, allocator);
        private static int GetApplicationTargetFrameRate() => Application.targetFrameRate;
        private static void SetApplicationTargetFrameRate(int targetFrameRate) => Application.targetFrameRate = targetFrameRate;
        private static bool IsApplicationBatchMode() => Application.isBatchMode;
        private static bool IsApplicationEditor() => Application.isEditor;
        private static bool IsApplicationFocused() => Application.isFocused;
        private static bool IsApplicationPlaying() => Application.isPlaying;
        private static bool IsApplicationMobilePlatform() => Application.isMobilePlatform;
        private static bool IsApplicationConsolePlatform() => Application.isConsolePlatform;
        private static bool IsApplicationGenuine() => Application.genuine;
        private static bool IsApplicationGenuineCheckAvailable() => Application.genuineCheckAvailable;
        private static bool GetApplicationRunInBackground() => Application.runInBackground;
        private static void SetApplicationRunInBackground(bool runInBackground) => Application.runInBackground = runInBackground;
        private static bool CanApplicationStreamedLevelBeLoaded(int levelIndex) => Application.CanStreamedLevelBeLoaded(levelIndex);
        private static bool CanApplicationStreamedLevelBeLoadedByName(String8 levelName) => Application.CanStreamedLevelBeLoaded(levelName.ToString());
        private static void OpenApplicationURL(String8 url) => Application.OpenURL(url.ToString());
        private static void QuitApplication(int exitCode = default) => Application.Quit(exitCode);
        private static void UnloadApplication() => Application.Unload();
        private static RuntimePlatform GetApplicationPlatform() => Application.platform;
        private static SystemLanguage GetApplicationSystemLanguage() => Application.systemLanguage;
        private static ApplicationInstallMode GetApplicationInstallMode() => Application.installMode;
        private static ApplicationSandboxType GetApplicationSandboxType() => Application.sandboxType;
        private static NetworkReachability GetApplicationInternetReachability() => Application.internetReachability;
        private static ThreadPriority GetApplicationBackgroundLoadingPriority() => Application.backgroundLoadingPriority;
        private static void SetApplicationBackgroundLoadingPriority(ThreadPriority priority) => Application.backgroundLoadingPriority = priority;
        private static bool RequestApplicationUserAuthorization(UserAuthorization mode) => Application.RequestUserAuthorization(mode).isDone;
        private static bool HasApplicationUserAuthorization(UserAuthorization mode) => Application.HasUserAuthorization(mode);
    }
}