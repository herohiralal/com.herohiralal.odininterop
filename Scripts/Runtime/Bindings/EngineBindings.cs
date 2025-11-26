using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Diagnostics;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityAllocator = Unity.Collections.Allocator;

namespace OdinInterop
{
    [GenerateOdinInterop]
    internal static unsafe partial class EngineBindings
    {
        private static HideFlags GetObjectHideFlags(ObjectHandle<Object> obj)
        {
            return obj ? obj.value.hideFlags : HideFlags.None;
        }

        private static void SetObjectHideFlags(ObjectHandle<Object> obj, HideFlags flags)
        {
            if (obj)
                obj.value.hideFlags = flags;
        }

        private static String8 GetObjectName(ObjectHandle<Object> obj)
        {
            if (obj)
                return obj.value.name;
            else
                return "";
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

        private static void* UnityOdnTropInternalMalloc(long size, int alignment, UnityAllocator allocator)
        {
            return UnsafeUtility.Malloc(size, alignment, allocator);
        }

        private static void UnityOdnTropInternalFree(void* ptr, UnityAllocator allocator)
        {
            UnsafeUtility.Free(ptr, allocator);
        }

        private static void UnityOdnTropInternalMemCopy(void* destination, void* source, long size)
        {
            UnsafeUtility.MemCpy(destination, source, size);
        }

        private static void UnityOdnTropInternalMemMove(void* destination, void* source, long size)
        {
            UnsafeUtility.MemMove(destination, source, size);
        }

        private static void UnityOdnTropInternalMemSet(void* destination, byte value, long size)
        {
            UnsafeUtility.MemSet(destination, value, size);
        }

        private static void UnityOdnTropInternalMemClear(void* destination, long size)
        {
            UnsafeUtility.MemClear(destination, size);
        }

        private static void UnityOdnTropInternalLog(
            LogType logTy,
            String8 msg,
            String8 procedure,
            String8 file,
            int line, int column
        )
        {
            Debug.LogFormat(logTy, LogOption.NoStacktrace, null,
                "{0} (in function {1} at {2}:{3}:{4})",
                msg.ToString(),
                procedure.ToString(),
                file.ToString(),
                line, column
            );
        }

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

        private static void UnityOdnTropInternalRandomInitState(int seed)
        {
            Random.InitState(seed);
        }

        private static void UnityOdnTropInternalRandomGetState(out int a, out int b, out int c, out int d)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (sizeof(Random.State) != (4 * sizeof(int)) || UnsafeUtility.AlignOf<Random.State>() != UnsafeUtility.AlignOf<int>())
            {
                Debug.LogError("UnityRandomGetState: Random.State layout mismatch!");
                a = b = c = d = 0;
                return;
            }
#endif

            var s = Random.state;
            int* statePtr = (int*)&s;
            a = statePtr[0];
            b = statePtr[1];
            c = statePtr[2];
            d = statePtr[3];
        }

        private static void UnityOdnTropInternalRandomSetState(int a, int b, int c, int d)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (sizeof(Random.State) != (4 * sizeof(int)) || UnsafeUtility.AlignOf<Random.State>() != UnsafeUtility.AlignOf<int>())
            {
                Debug.LogError("UnityRandomGetState: Random.State layout mismatch!");
                a = b = c = d = 0;
                return;
            }
#endif

            Random.State s = new Random.State();
            int* statePtr = (int*)&s;
            statePtr[0] = a;
            statePtr[1] = b;
            statePtr[2] = c;
            statePtr[3] = d;

            Random.state = s;
        }

        private static int UnityOdnTropInternalRandomGetNextInt()
        {
            return Random.Range(int.MinValue, int.MaxValue);
        }

        public static partial String8 UnityOdnTropInternalAllocateString8(int length);
        public static partial String16 UnityOdnTropInternalAllocateString16(int length);
    }
}
