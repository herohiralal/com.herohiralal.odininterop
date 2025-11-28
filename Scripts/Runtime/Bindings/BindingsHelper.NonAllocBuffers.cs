using System.Collections.Generic;
using UnityEngine;

namespace OdinInterop
{
    internal static partial class BindingsHelper
    {
        internal static readonly RaycastHit[] raycastHits = new RaycastHit[256];
        internal static readonly Collider[] colliders = new Collider[256];
        internal static readonly List<Material> tempMaterialsList = new List<Material>(256);
        internal static readonly List<Vector2> tempVector2List = new List<Vector2>(65536);
        internal static readonly List<Vector3> tempVector3List = new List<Vector3>(65536);
        internal static readonly List<int> tempIntList = new List<int>(65536);
    }
}
