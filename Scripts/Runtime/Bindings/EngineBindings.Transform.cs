using UnityEngine;

namespace OdinInterop
{
    internal static unsafe partial class EngineBindings
    {
        // Transform API

        private static Vector3 GetTransformPosition(ObjectHandle<Transform> transform) => transform ? transform.value.position : default;
        private static void SetTransformPosition(ObjectHandle<Transform> transform, Vector3 position)
        {
            if (transform)
                transform.value.position = position;
        }
        private static Vector3 GetTransformLocalPosition(ObjectHandle<Transform> transform) => transform ? transform.value.localPosition : default;
        private static void SetTransformLocalPosition(ObjectHandle<Transform> transform, Vector3 localPosition)
        {
            if (transform)
                transform.value.localPosition = localPosition;
        }
        private static Quaternion GetTransformRotation(ObjectHandle<Transform> transform) => transform ? transform.value.rotation : Quaternion.identity;
        private static void SetTransformRotation(ObjectHandle<Transform> transform, Quaternion rotation)
        {
            if (transform)
                transform.value.rotation = rotation;
        }
        private static Quaternion GetTransformLocalRotation(ObjectHandle<Transform> transform) => transform ? transform.value.localRotation : Quaternion.identity;
        private static void SetTransformLocalRotation(ObjectHandle<Transform> transform, Quaternion localRotation)
        {
            if (transform)
                transform.value.localRotation = localRotation;
        }
        private static Vector3 GetTransformEulerAngles(ObjectHandle<Transform> transform) => transform ? transform.value.eulerAngles : default;
        private static void SetTransformEulerAngles(ObjectHandle<Transform> transform, Vector3 eulerAngles)
        {
            if (transform)
                transform.value.eulerAngles = eulerAngles;
        }
        private static Vector3 GetTransformLocalEulerAngles(ObjectHandle<Transform> transform) => transform ? transform.value.localEulerAngles : default;
        private static void SetTransformLocalEulerAngles(ObjectHandle<Transform> transform, Vector3 localEulerAngles)
        {
            if (transform)
                transform.value.localEulerAngles = localEulerAngles;
        }
        private static Vector3 GetTransformLocalScale(ObjectHandle<Transform> transform) => transform ? transform.value.localScale : Vector3.one;
        private static void SetTransformLocalScale(ObjectHandle<Transform> transform, Vector3 localScale)
        {
            if (transform)
                transform.value.localScale = localScale;
        }
        private static Vector3 GetTransformLossyScale(ObjectHandle<Transform> transform) => transform ? transform.value.lossyScale : Vector3.one;
        private static Vector3 GetTransformRight(ObjectHandle<Transform> transform) => transform ? transform.value.right : Vector3.right;
        private static void SetTransformRight(ObjectHandle<Transform> transform, Vector3 right)
        {
            if (transform)
                transform.value.right = right;
        }
        private static Vector3 GetTransformUp(ObjectHandle<Transform> transform) => transform ? transform.value.up : Vector3.up;
        private static void SetTransformUp(ObjectHandle<Transform> transform, Vector3 up)
        {
            if (transform)
                transform.value.up = up;
        }
        private static Vector3 GetTransformForward(ObjectHandle<Transform> transform) => transform ? transform.value.forward : Vector3.forward;
        private static void SetTransformForward(ObjectHandle<Transform> transform, Vector3 forward)
        {
            if (transform)
                transform.value.forward = forward;
        }
        private static Matrix4x4 GetTransformLocalToWorldMatrix(ObjectHandle<Transform> transform) => transform ? transform.value.localToWorldMatrix : Matrix4x4.identity;
        private static Matrix4x4 GetTransformWorldToLocalMatrix(ObjectHandle<Transform> transform) => transform ? transform.value.worldToLocalMatrix : Matrix4x4.identity;
        private static ObjectHandle<Transform> GetTransformRoot(ObjectHandle<Transform> transform) => transform ? transform.value.root : default;
        private static ObjectHandle<Transform> GetTransformParent(ObjectHandle<Transform> transform) => transform ? transform.value.parent : default;
        private static void SetTransformParent(ObjectHandle<Transform> transform, ObjectHandle<Transform> parent, bool worldPositionStays = true)
        {
            if (transform)
                transform.value.SetParent(parent, worldPositionStays);
        }
        private static int GetTransformChildCount(ObjectHandle<Transform> transform) => transform ? transform.value.childCount : 0;
        private static int GetTransformHierarchyCount(ObjectHandle<Transform> transform) => transform ? transform.value.hierarchyCount : 0;
        private static bool GetTransformHasChanged(ObjectHandle<Transform> transform) => transform ? transform.value.hasChanged : false;
        private static void SetTransformHasChanged(ObjectHandle<Transform> transform, bool hasChanged)
        {
            if (transform)
                transform.value.hasChanged = hasChanged;
        }
        private static int GetTransformSiblingIndex(ObjectHandle<Transform> transform) => transform ? transform.value.GetSiblingIndex() : -1;
        private static void SetTransformSiblingIndex(ObjectHandle<Transform> transform, int index)
        {
            if (transform)
                transform.value.SetSiblingIndex(index);
        }
        private static void SetTransformAsFirstSibling(ObjectHandle<Transform> transform)
        {
            if (transform)
                transform.value.SetAsFirstSibling();
        }
        private static void SetTransformAsLastSibling(ObjectHandle<Transform> transform)
        {
            if (transform)
                transform.value.SetAsLastSibling();
        }
        private static ObjectHandle<Transform> GetTransformChild(ObjectHandle<Transform> transform, int index)
        {
            if (transform && index >= 0 && index < transform.value.childCount)
                return transform.value.GetChild(index);
            return default;
        }
        private static ObjectHandle<Transform> FindTransformChild(ObjectHandle<Transform> transform, String8 name)
        {
            if (!transform)
                return default;
            return transform.value.Find(name.ToString());
        }
        private static bool IsTransformChildOf(ObjectHandle<Transform> transform, ObjectHandle<Transform> parent) => transform && parent ? transform.value.IsChildOf(parent) : false;
        private static void DetachTransformChildren(ObjectHandle<Transform> transform)
        {
            if (transform)
                transform.value.DetachChildren();
        }
        private static void LookAtTransform(ObjectHandle<Transform> transform, ObjectHandle<Transform> target, Vector3 worldUp = default)
        {
            if (transform && target)
                transform.value.LookAt(target, worldUp == default ? Vector3.up : worldUp);
        }
        private static void LookAtPosition(ObjectHandle<Transform> transform, Vector3 worldPosition, Vector3 worldUp = default)
        {
            if (transform)
                transform.value.LookAt(worldPosition, worldUp == default ? Vector3.up : worldUp);
        }
        private static void TransformTranslate(ObjectHandle<Transform> transform, Vector3 translation, Space relativeTo = Space.Self)
        {
            if (transform)
                transform.value.Translate(translation, relativeTo);
        }
        private static void TransformRotate(ObjectHandle<Transform> transform, Vector3 eulers, Space relativeTo = Space.Self)
        {
            if (transform)
                transform.value.Rotate(eulers, relativeTo);
        }
        private static void TransformRotateAxis(ObjectHandle<Transform> transform, Vector3 axis, float angle, Space relativeTo = Space.Self)
        {
            if (transform)
                transform.value.Rotate(axis, angle, relativeTo);
        }
        private static void TransformRotateAround(ObjectHandle<Transform> transform, Vector3 point, Vector3 axis, float angle)
        {
            if (transform)
                transform.value.RotateAround(point, axis, angle);
        }
        private static Vector3 TransformPoint(ObjectHandle<Transform> transform, Vector3 position) => transform ? transform.value.TransformPoint(position) : position;
        private static Vector3 InverseTransformPoint(ObjectHandle<Transform> transform, Vector3 position) => transform ? transform.value.InverseTransformPoint(position) : position;
        private static Vector3 TransformDirection(ObjectHandle<Transform> transform, Vector3 direction) => transform ? transform.value.TransformDirection(direction) : direction;
        private static Vector3 InverseTransformDirection(ObjectHandle<Transform> transform, Vector3 direction) => transform ? transform.value.InverseTransformDirection(direction) : direction;
        private static Vector3 TransformVector(ObjectHandle<Transform> transform, Vector3 vector) => transform ? transform.value.TransformVector(vector) : vector;
        private static Vector3 InverseTransformVector(ObjectHandle<Transform> transform, Vector3 vector) => transform ? transform.value.InverseTransformVector(vector) : vector;
        private static void SetTransformPositionAndRotation(ObjectHandle<Transform> transform, Vector3 position, Quaternion rotation)
        {
            if (transform)
                transform.value.SetPositionAndRotation(position, rotation);
        }
        private static void SetTransformLocalPositionAndRotation(ObjectHandle<Transform> transform, Vector3 localPosition, Quaternion localRotation)
        {
            if (transform)
                transform.value.SetLocalPositionAndRotation(localPosition, localRotation);
        }
        private static void GetTransformPositionAndRotation(ObjectHandle<Transform> transform, out Vector3 position, out Quaternion rotation)
        {
            if (transform)
                transform.value.GetPositionAndRotation(out position, out rotation);
            else
            {
                position = default;
                rotation = Quaternion.identity;
            }
        }
        private static void GetTransformLocalPositionAndRotation(ObjectHandle<Transform> transform, out Vector3 localPosition, out Quaternion localRotation)
        {
            if (transform)
                transform.value.GetLocalPositionAndRotation(out localPosition, out localRotation);
            else
            {
                localPosition = default;
                localRotation = Quaternion.identity;
            }
        }
        private static Slice<ObjectHandle<Transform>> GetTransformChildren(ObjectHandle<Transform> transform, Allocator allocator)
        {
            if (!transform)
                return default;
            var count = transform.value.childCount;
            var slice = new Slice<ObjectHandle<Transform>>(count, allocator);
            for (var i = 0; i < count; i++)
                slice.ptr[i] = transform.value.GetChild(i);
            return slice;
        }

        // RectTransform specific (for UI)
        private static bool IsRectTransform(ObjectHandle<Transform> transform) => transform ? transform.value is RectTransform : false;

    }
}
