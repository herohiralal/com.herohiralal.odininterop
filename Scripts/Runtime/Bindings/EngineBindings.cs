using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OdinInterop
{
    [GenerateOdinInterop]
    internal static partial class EngineBindings
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
    }
}
