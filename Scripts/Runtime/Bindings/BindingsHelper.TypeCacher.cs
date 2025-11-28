using System.Collections.Generic;

namespace OdinInterop
{
    internal static partial class BindingsHelper
    {
        private static readonly Dictionary<String8, System.Type> s_TypeCache = new Dictionary<String8, System.Type>();

        internal static System.Type GetCachedType(String8 typeName)
        {
            if (s_TypeCache.TryGetValue(typeName, out var cachedType))
            {
                return cachedType;
            }

            var type = System.Type.GetType(typeName.ToString());
            s_TypeCache[typeName] = type;
            return type;
        }
    }
}
