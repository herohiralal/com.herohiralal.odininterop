package src

import "base:runtime"

@(thread_local)
@(private)
G_OdnTrop_Internal_Ctx: runtime.Context
@(thread_local)
@(private)
G_OdnTrop_Internal_CtxNesting: uint

CreateUnityContext :: proc() -> runtime.Context {
	return {allocator = UNITY_MAIN_ALLOCATOR, temp_allocator = UNITY_MAIN_TEMP_ALLOCATOR, assertion_failure_proc = UnityPanic, logger = UNITY_MAIN_LOGGER, random_generator = UNITY_DEFAULT_RANDOM_NUMBER_GENERATOR}
}
