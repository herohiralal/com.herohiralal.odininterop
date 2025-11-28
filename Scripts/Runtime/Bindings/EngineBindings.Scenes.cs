using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityAllocator = Unity.Collections.Allocator;

namespace OdinInterop
{
    internal static unsafe partial class EngineBindings
    {
        // scenes api

        private static int GetSceneBuildIndex(Scene scene) => scene.buildIndex;
        private static bool IsSceneDirty(Scene scene) => scene.isDirty;
        private static bool IsSceneLoaded(Scene scene) => scene.isLoaded;
        private static bool IsSceneValid(Scene scene) => scene.IsValid();
        private static String8 GetSceneName(Scene scene, Allocator allocator) => new String8(scene.name, allocator);
        private static String8 GetScenePath(Scene scene, Allocator allocator) => new String8(scene.path, allocator);
        private static int GetRootGameObjectsCount(Scene scene) => scene.rootCount;
        private static Slice<ObjectHandle<GameObject>> GetRootGameObjects(Scene scene, Allocator allocator)
        {
            var roots = scene.GetRootGameObjects();
            var slice = new Slice<ObjectHandle<GameObject>>(roots.Length, allocator);
            for (var i = 0; i < roots.Length; i++)
                slice.ptr[i] = roots[i];
            return slice;
        }

        // scene manager api

        private static int GetLoadedScenesCount() => SceneManager.loadedSceneCount;
        private static int GetScenesCount() => SceneManager.sceneCount;
        private static int GetScenesCountInBuildSettings() => SceneManager.sceneCountInBuildSettings;
        private static int CreateScene(String8 name, LocalPhysicsMode physicsMode = default) => SceneManager.CreateScene(name.ToString(), new CreateSceneParameters(physicsMode)).handle;
        private static int GetActiveScene() => SceneManager.GetActiveScene().handle;
        private static int GetSceneAtIndex(int index) => SceneManager.GetSceneAt(index).handle;
        private static int GetSceneByBuildIndex(int buildIndex) => SceneManager.GetSceneByBuildIndex(buildIndex).handle;
        private static int GetSceneByName(String8 name) => SceneManager.GetSceneByName(name.ToString()).handle;
        private static int GetSceneByPath(String8 path) => SceneManager.GetSceneByPath(path.ToString()).handle;
        private static void LoadSceneByBuildIndex(int buildIndex, LoadSceneMode mode = default) => SceneManager.LoadScene(buildIndex, mode);
        private static void LoadSceneByName(String8 name, LoadSceneMode mode = default) => SceneManager.LoadScene(name.ToString(), mode);
        private static void MergeScenes(Scene src, Scene dst) => SceneManager.MergeScenes(src, dst);
        private static void MoveGameObjectsToScene(Slice<ObjectHandle<GameObject>> gameObjects, Scene scene) =>
            SceneManager.MoveGameObjectsToScene(NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(gameObjects.ptr, gameObjects.len.ToInt32(), UnityAllocator.None), scene);
        private static void MoveGameObjectToScene(ObjectHandle<GameObject> gameObject, Scene scene) => SceneManager.MoveGameObjectToScene(gameObject.value, scene);
        private static bool SetActiveScene(Scene scene) => SceneManager.SetActiveScene(scene);
        private static uint LoadSceneAsyncByBuildIndex(int buildIndex, LoadSceneMode lsMode = default, LocalPhysicsMode phMode = default) =>
            BindingsHelper.RegisterAsyncOperation(SceneManager.LoadSceneAsync(buildIndex, new LoadSceneParameters(lsMode, phMode)));
        private static uint LoadSceneAsyncByName(String8 name, LoadSceneMode lsMode = default, LocalPhysicsMode phMode = default) =>
            BindingsHelper.RegisterAsyncOperation(SceneManager.LoadSceneAsync(name.ToString(), new LoadSceneParameters(lsMode, phMode)));
        private static uint UnloadSceneAsyncByHandle(Scene scene, bool unloadEmbedded) =>
            BindingsHelper.RegisterAsyncOperation(SceneManager.UnloadSceneAsync(scene, unloadEmbedded ? UnloadSceneOptions.UnloadAllEmbeddedSceneObjects : UnloadSceneOptions.None));
        private static uint UnloadSceneAsyncByBuildIndex(int buildIndex, bool unloadEmbedded) =>
            BindingsHelper.RegisterAsyncOperation(SceneManager.UnloadSceneAsync(buildIndex, unloadEmbedded ? UnloadSceneOptions.UnloadAllEmbeddedSceneObjects : UnloadSceneOptions.None));
        private static uint UnloadSceneAsyncByName(String8 name, bool unloadEmbedded) =>
            BindingsHelper.RegisterAsyncOperation(SceneManager.UnloadSceneAsync(name.ToString(), unloadEmbedded ? UnloadSceneOptions.UnloadAllEmbeddedSceneObjects : UnloadSceneOptions.None));
        private static float GetAsyncOperationProgress(uint asyncOpId) => BindingsHelper.GetAsyncOperationProgress(asyncOpId);
        private static bool IsAsyncOperationDone(uint asyncOpId) => BindingsHelper.IsAsyncOperationDone(asyncOpId);
        private static int GetAsyncOperationPriority(uint asyncOpId) => BindingsHelper.GetAsyncOperationPriority(asyncOpId);
        private static void SetAsyncOperationPriority(uint asyncOpId, int priority) => BindingsHelper.SetAsyncOperationPriority(asyncOpId, priority);
        private static bool DoesAsyncSceneOperationAllowActivation(uint asyncOpId) => BindingsHelper.DoesAsyncSceneOperationAllowActivation(asyncOpId);
        private static void SetAsyncSceneOperationAllowActivation(uint asyncOpId, bool allow) => BindingsHelper.SetAsyncSceneOperationAllowActivation(asyncOpId, allow);
    }
}