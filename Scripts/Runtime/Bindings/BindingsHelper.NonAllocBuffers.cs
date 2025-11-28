using System.Collections.Generic;
using UnityEngine;

namespace OdinInterop
{
    internal static partial class BindingsHelper
    {
        internal static readonly RaycastHit[] raycastHits = new RaycastHit[256];
        internal static readonly Collider[] colliders = new Collider[256];
    }
}
