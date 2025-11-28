using UnityEngine;
using System;

namespace OdinInterop
{
    internal static unsafe partial class EngineBindings
    {
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
    }
}