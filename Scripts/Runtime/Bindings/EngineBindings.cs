using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityAllocator = Unity.Collections.Allocator;

namespace OdinInterop
{
    [GenerateOdinInterop]
    internal static unsafe partial class EngineBindings
    {
        static EngineBindings()
        {
#if UNINTY_EDITOR || DEVELOPMENT_BUILD
            var sb = new System.Text.StringBuilder();
            if (sizeof(UnityEngine.SceneManagement.Scene) != sizeof(int))
                sb.AppendLine("EngineBindings: Scene struct size mismatch!");

            if (UnsafeUtility.AlignOf<UnityEngine.SceneManagement.Scene>() != UnsafeUtility.AlignOf<int>())
                sb.AppendLine("EngineBindings: Scene struct alignment mismatch!");

            if (sizeof(Random.State) != (4 * sizeof(int)))
                sb.AppendLine("EngineBindings: Random.State size mismatch!");

            if (UnsafeUtility.AlignOf<Random.State>() != UnsafeUtility.AlignOf<int>())
                sb.AppendLine("EngineBindings: Random.State alignment mismatch!");

            if (sb.Length > 0)
            {
                throw new Exception(sb.ToString());
            }
#endif
        }

        private static HideFlags GetObjectHideFlags(ObjectHandle<Object> obj)
        {
            return obj ? obj.value.hideFlags : HideFlags.None;
        }

        private static void SetObjectHideFlags(ObjectHandle<Object> obj, HideFlags flags)
        {
            if (obj)
                obj.value.hideFlags = flags;
        }

        private static String8 GetObjectName(ObjectHandle<Object> obj, Allocator allocator)
        {
            if (!obj) return default;

            return new String8(obj.value.name, allocator);
        }

        private static void SetObjectName(ObjectHandle<Object> obj, String8 name)
        {
            if (obj)
                obj.value.name = name.ToString();
        }

        private static void DestroyObject(ObjectHandle<Object> obj)
        {
            if (obj)
                Object.Destroy(obj.value);
        }

        private static void DestroyObjectImmediate(ObjectHandle<Object> obj, bool allowDestroyingAssets = default)
        {
            if (obj)
                Object.DestroyImmediate(obj.value, allowDestroyingAssets);
        }

        private static void DontDestroyObjectOnLoad(ObjectHandle<Object> obj)
        {
            if (obj)
                Object.DontDestroyOnLoad(obj.value);
        }

        private static ObjectHandle<Object> InstantiateObjectWithoutTransform(
            ObjectHandle<Object> original,
            ObjectHandle<Transform> parent = default,
            bool instantiateInWorldSpace = default
        )
        {
            if (!original)
                return default;

            if (parent)
                return Object.Instantiate(original, parent, instantiateInWorldSpace);
            else
                return Object.Instantiate(original);
        }

        private static ObjectHandle<Object> InstantiateObjectWithTransform(
            ObjectHandle<Object> original,
            Vector3 position,
            Quaternion rotation,
            ObjectHandle<Transform> parent = default
        )
        {
            if (!original)
                return default;

            if (parent)
                return Object.Instantiate(original, position, rotation, parent);
            else
                return Object.Instantiate(original, position, rotation);
        }

        // scenes api

        private static int GetSceneBuildIndex(int sceneHandle)
        {
            var scene = *(Scene*)&sceneHandle;
            return scene.buildIndex;
        }

        private static bool IsSceneDirty(int sceneHandle)
        {
            var scene = *(Scene*)&sceneHandle;
            return scene.isDirty;
        }

        private static bool IsSceneLoaded(int sceneHandle)
        {
            var scene = *(Scene*)&sceneHandle;
            return scene.isLoaded;
        }

        private static bool IsSceneValid(int sceneHandle)
        {
            var scene = *(Scene*)&sceneHandle;
            return scene.IsValid();
        }

        private static String8 GetSceneName(int sceneHandle, Allocator allocator)
        {
            var scene = *(Scene*)&sceneHandle;
            return new String8(scene.name, allocator);
        }

        private static String8 GetScenePath(int sceneHandle, Allocator allocator)
        {
            var scene = *(Scene*)&sceneHandle;
            return new String8(scene.path, allocator);
        }

        private static int GetRootGameObjectsCount(int sceneHandle)
        {
            var scene = *(Scene*)&sceneHandle;
            return scene.rootCount;
        }

        private static Slice<ObjectHandle<GameObject>> GetRootGameObjects(int sceneHandle, Allocator allocator)
        {
            var scene = *(Scene*)&sceneHandle;
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
        private static void MergeScenes(int src, int dst) => SceneManager.MergeScenes(*(Scene*)&src, *(Scene*)&dst);
        private static void MoveGameObjectsToScene(Slice<ObjectHandle<GameObject>> gameObjects, int sceneHandle) =>
            SceneManager.MoveGameObjectsToScene(NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(gameObjects.ptr, gameObjects.len.ToInt32(), UnityAllocator.None), *(Scene*)&sceneHandle);
        private static void MoveGameObjectToScene(ObjectHandle<GameObject> gameObject, int sceneHandle) => SceneManager.MoveGameObjectToScene(gameObject.value, *(Scene*)&sceneHandle);
        private static bool SetActiveScene(int sceneHandle) => SceneManager.SetActiveScene(*(Scene*)&sceneHandle);
        private static uint LoadSceneAsyncByBuildIndex(int buildIndex, LoadSceneMode lsMode = default, LocalPhysicsMode phMode = default) =>
            BindingsHelper.RegisterAsyncOperation(SceneManager.LoadSceneAsync(buildIndex, new LoadSceneParameters(lsMode, phMode)));
        private static uint LoadSceneAsyncByName(String8 name, LoadSceneMode lsMode = default, LocalPhysicsMode phMode = default) =>
            BindingsHelper.RegisterAsyncOperation(SceneManager.LoadSceneAsync(name.ToString(), new LoadSceneParameters(lsMode, phMode)));
        private static uint UnloadSceneAsyncByHandle(int sceneHandle, bool unloadEmbedded) =>
            BindingsHelper.RegisterAsyncOperation(SceneManager.UnloadSceneAsync(*(Scene*)&sceneHandle, unloadEmbedded ? UnloadSceneOptions.UnloadAllEmbeddedSceneObjects : UnloadSceneOptions.None));
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

        // gameobject api

        private static ObjectHandle<GameObject> CreateGameObject(String8 name, Slice<String8> componentTypeNames = default)
        {
            if (componentTypeNames.ptr == null)
                return new GameObject(name.ToString());

            var arr = new Type[componentTypeNames.len.ToInt32()];
            for (var i = 0; i < componentTypeNames.len.ToInt32(); i++)
                arr[i] = BindingsHelper.GetCachedType(componentTypeNames.ptr[i]) ?? typeof(DummyComponent);

            return new GameObject(name.ToString(), arr);
        }

        private static ObjectHandle<GameObject> CreatePrimitiveGameObject(PrimitiveType type)
        {
            return GameObject.CreatePrimitive(type);
        }

        private static ObjectHandle<GameObject> FindGameObjectByName(String8 name)
        {
            return GameObject.Find(name.ToString());
        }

        private static ObjectHandle<GameObject> FindGameObjectByTag(String8 tag)
        {
            return GameObject.FindGameObjectWithTag(tag.ToString());
        }

        private static Slice<ObjectHandle<GameObject>> FindGameObjectsByTag(String8 tag, Allocator allocator)
        {
            var gos = GameObject.FindGameObjectsWithTag(tag.ToString());
            var slice = new Slice<ObjectHandle<GameObject>>(gos.Length, allocator);
            for (var i = 0; i < gos.Length; i++)
                slice.ptr[i] = gos[i];
            return slice;
        }

        private static bool IsGameObjectActiveInHierarchy(ObjectHandle<GameObject> gameObject)
        {
            if (gameObject)
                return gameObject.value.activeInHierarchy;
            else
                return false;
        }

        private static bool IsGameObjectActive(ObjectHandle<GameObject> gameObject)
        {
            if (gameObject)
                return gameObject.value.activeSelf;
            else
                return false;
        }

        private static void SetGameObjectActive(ObjectHandle<GameObject> gameObject, bool active)
        {
            if (gameObject)
                gameObject.value.SetActive(active);
        }

        private static int UnityOdnTropInternalGetGameObjectLayer(ObjectHandle<GameObject> gameObject)
        {
            if (gameObject)
                return gameObject.value.layer;
            else
                return 0;
        }

        private static void UnityOdnTropInternalSetGameObjectLayer(ObjectHandle<GameObject> gameObject, int layer)
        {
            if (gameObject)
                gameObject.value.layer = layer;
        }

        private static int GetSceneFromGameObject(ObjectHandle<GameObject> gameObject)
        {
            if (!gameObject) return default;

            var sc = GameObject.GetScene(gameObject.id);
            return sc.handle;
        }

        private static ObjectHandle<Transform> GetTransformFromGameObject(ObjectHandle<GameObject> gameObject)
        {
            if (gameObject)
                return gameObject.value.transform;
            else
                return default;
        }

        private static ObjectHandle<Component> AddComponent(ObjectHandle<GameObject> gameObject, String8 typeName)
        {
            if (!gameObject) return default;

            var type = BindingsHelper.GetCachedType(typeName);
            if (type == null) return default;

            var comp = gameObject.value.AddComponent(type);
            if (comp == null) return default;

            return comp;
        }

        private static ObjectHandle<Component> GetComponent(ObjectHandle<GameObject> gameObject, String8 typeName)
        {
            if (!gameObject) return default;

            var type = BindingsHelper.GetCachedType(typeName);
            if (type == null) return default;

            var comp = gameObject.value.GetComponent(type);
            if (comp == null) return default;

            return comp;
        }

        private static ObjectHandle<Component> GetComponentInChildren(ObjectHandle<GameObject> gameObject, String8 typeName, bool includeInactive)
        {
            if (!gameObject) return default;

            var type = BindingsHelper.GetCachedType(typeName);
            if (type == null) return default;

            var comp = gameObject.value.GetComponentInChildren(type, includeInactive);
            if (comp == null) return default;

            return comp;
        }

        private static ObjectHandle<Component> GetComponentInParent(ObjectHandle<GameObject> gameObject, String8 typeName, bool includeInactive)
        {
            if (!gameObject) return default;

            var type = BindingsHelper.GetCachedType(typeName);
            if (type == null) return default;

            var comp = gameObject.value.GetComponentInParent(type, includeInactive);
            if (comp == null) return default;

            return comp;
        }

        private static Slice<ObjectHandle<Component>> GetComponents(ObjectHandle<GameObject> gameObject, String8 typeName, Allocator allocator)
        {
            if (!gameObject) return default;

            var type = BindingsHelper.GetCachedType(typeName);
            if (type == null) return default;

            var comps = gameObject.value.GetComponents(type);
            var slice = new Slice<ObjectHandle<Component>>(comps.Length, allocator);
            for (var i = 0; i < comps.Length; i++)
                slice.ptr[i] = comps[i];
            return slice;
        }

        private static Slice<ObjectHandle<Component>> GetComponentsInChildren(ObjectHandle<GameObject> gameObject, String8 typeName, bool includeInactive, Allocator allocator)
        {
            if (!gameObject) return default;

            var type = BindingsHelper.GetCachedType(typeName);
            if (type == null) return default;

            var comps = gameObject.value.GetComponentsInChildren(type, includeInactive);
            var slice = new Slice<ObjectHandle<Component>>(comps.Length, allocator);
            for (var i = 0; i < comps.Length; i++)
                slice.ptr[i] = comps[i];
            return slice;
        }

        private static Slice<ObjectHandle<Component>> GetComponentsInParent(ObjectHandle<GameObject> gameObject, String8 typeName, bool includeInactive, Allocator allocator)
        {
            if (!gameObject) return default;

            var type = BindingsHelper.GetCachedType(typeName);
            if (type == null) return default;

            var comps = gameObject.value.GetComponentsInParent(type, includeInactive);
            var slice = new Slice<ObjectHandle<Component>>(comps.Length, allocator);
            for (var i = 0; i < comps.Length; i++)
                slice.ptr[i] = comps[i];
            return slice;
        }

        private static bool TryGetComponent(ObjectHandle<GameObject> gameObject, String8 typeName, out ObjectHandle<Component> component)
        {
            component = default;

            if (!gameObject) return false;

            var type = BindingsHelper.GetCachedType(typeName);
            if (type == null) return false;

            var comp = gameObject.value.GetComponent(type);
            if (comp == null) return false;

            component = comp;
            return true;
        }

        private static bool CompareGameObjectTag(ObjectHandle<GameObject> gameObject, String8 tag)
        {
            if (gameObject)
                return gameObject.value.CompareTag(tag.ToString());
            else
                return false;
        }

        // component api

        private static ObjectHandle<GameObject> GetGameObject(ObjectHandle<Component> component)
        {
            if (component)
                return component.value.gameObject;
            else
                return default;
        }

        private static ObjectHandle<Transform> GetTransformFromComponent(ObjectHandle<Component> component)
        {
            if (component)
                return component.value.transform;
            else
                return default;
        }

        // behaviour api

        private static bool IsBehaviourEnabled(ObjectHandle<Behaviour> behaviour)
        {
            if (behaviour)
                return behaviour.value.enabled;
            else
                return false;
        }

        private static void SetBehaviourEnabled(ObjectHandle<Behaviour> behaviour, bool enabled)
        {
            if (behaviour)
                behaviour.value.enabled = enabled;
        }

        private static bool IsBehaviourActiveAndEnabled(ObjectHandle<Behaviour> behaviour)
        {
            if (behaviour)
                return behaviour.value.isActiveAndEnabled;
            else
                return false;
        }

        private static void MemCopy(void* destination, void* source, long size) => UnsafeUtility.MemCpy(destination, source, size);

        private static void MemMove(void* destination, void* source, long size) => UnsafeUtility.MemMove(destination, source, size);

        private static void MemSet(void* destination, byte value, long size) => UnsafeUtility.MemSet(destination, value, size);

        private static void MemClr(void* destination, long size) => UnsafeUtility.MemClear(destination, size);

        private static void* MemTmp(long size, int alignment) => UnsafeUtility.Malloc(size, alignment, UnityAllocator.Temp);

        private static void UnityOdnTropInternalPanic(String8 prefix, String8 message, String8 procedure, String8 file, int line, int column)
        {
            Debug.LogErrorFormat(
                "[Assertion Failure] {0}: {1} (in function {2} at {3}:{4}:{5})",
                prefix.ToString(),
                message.ToString(),
                procedure.ToString(),
                file.ToString(),
                line,
                column
            );
            Utils.ForceCrash(ForcedCrashCategory.FatalError);
        }

        private static void UnityOdnTropInternalRandomInitState(int seed) => Random.InitState(seed);

        private static void UnityOdnTropInternalRandomGetState(out int a, out int b, out int c, out int d)
        {
            var s = Random.state;
            int* statePtr = (int*)&s;
            a = statePtr[0];
            b = statePtr[1];
            c = statePtr[2];
            d = statePtr[3];
        }

        private static void UnityOdnTropInternalRandomSetState(int a, int b, int c, int d)
        {
            var s = new Random.State();
            int* statePtr = (int*)&s;
            statePtr[0] = a;
            statePtr[1] = b;
            statePtr[2] = c;
            statePtr[3] = d;

            Random.state = s;
        }

        private static int UnityOdnTropInternalRandomGetNextInt() => Random.Range(int.MinValue, int.MaxValue);

        public static partial Allocator UnityOdnTropInternalGetMainOdnAllocator();
        public static partial Allocator UnityOdnTropInternalGetTempOdnAllocator();
        public static partial Slice<byte> UnityOdnTropInternalAllocateUsingOdnAllocator(int tySize, int tyAlignment, int tyCount, Allocator allocator);
        public static partial void UnityOdnTropInternalFreeUsingOdnAllocator(Slice<byte> ptr, Allocator allocator);
    }
}
