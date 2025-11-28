using UnityEngine;

namespace OdinInterop
{
    internal static partial class EngineBindings
    {
        // Object API

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
    }
}