using UnityEngine;

namespace OdinInterop.Internal
{
    [GenerateOdinInterop(odinSrcAppend = InteropGeneratorInbuiltFiles.ODIN_INTEROP_HOOK_BINDINGS_APPEND)]
    internal static partial class OdinInteropHookBindings
    {
        public static partial void OnGlobalAwake();
        public static partial void OnGlobalStart();
        public static partial void OnGlobalFixedUpdate(float dt);
        public static partial void OnGlobalUpdate(float dt, float unscaledDt);
        public static partial void OnGlobalLateUpdate(float dt, float unscaledDt);
        public static partial void OnGlobalDestroy();
    }

    [AddComponentMenu("")]
    public class OdinInteropHook : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialise() => DontDestroyOnLoad(new GameObject("[OdinInteropHook]", typeof(OdinInteropHook)));

        private void Awake() => OdinInteropHookBindings.OnGlobalAwake();
        private void Start() => OdinInteropHookBindings.OnGlobalStart();
        private void FixedUpdate() => OdinInteropHookBindings.OnGlobalFixedUpdate(Time.fixedDeltaTime);
        private void Update() => OdinInteropHookBindings.OnGlobalUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        private void LateUpdate() => OdinInteropHookBindings.OnGlobalLateUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        private void OnDestroy() => OdinInteropHookBindings.OnGlobalDestroy();
    }
}
