package src

@(private = "file")
UnityInterfaceGUID :: struct {
	high, low: u64,
}

@(private = "file")
IUnityInterface :: struct {}

when UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN {
	@(private = "file")
	IUnityInterfaces :: struct {
		GetInterface:      proc "std" (guid: UnityInterfaceGUID) -> ^IUnityInterface,
		RegisterInterface: proc "std" (guid: UnityInterfaceGUID, ptr: ^IUnityInterface),
	}
} else {
	@(private = "file")
	IUnityInterfaces :: struct {
		GetInterface:      proc "c" (guid: UnityInterfaceGUID) -> ^IUnityInterface,
		RegisterInterface: proc "c" (guid: UnityInterfaceGUID, ptr: ^IUnityInterface),
	}
}

@(private = "file")
UnityLogType :: enum {
	Error     = 0,
	Warning   = 2,
	Log       = 3,
	Exception = 4,
}

when UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN {
	@(private = "file")
	IUnityLog :: struct {
		Log: proc "std" (ty: UnityLogType, message, fileName: cstring, line: i32),
	}
} else {
	@(private = "file")
	IUnityLog :: struct {
		Log: proc "c" (ty: UnityLogType, message, fileName: cstring, line: i32),
	}
}

@(private = "file")
IUnityLogGUID: UnityInterfaceGUID : {high = 0x9E7507fA5B444D5D, low = 0x92FB979515EA83FC}

// KEEP IN SYNC WITH `StoredState` IN `Binder.c` AND `OdinCompiler.cs`!!!!!
@(private = "file")
StoredState :: struct {
	unityInterfaces: ^IUnityInterfaces,
	libHandle:       rawptr, // handle of self loaded library

	// all the native stuff starts here
	initialised:     b64,
}

#assert(size_of(StoredState) <= 1024) // ensure we have enough space for future use
#assert(align_of(StoredState) == 8)

// all thet global state
@(private = "file")
G_StoredState: struct {
	interfaces: ^IUnityInterfaces,
} = {}

when UNITY_EDITOR {
	@(export)
	@(private = "file")
	UnityOdnTropInternalSetUnityInterfacesPtr :: proc "c" (ptr: ^StoredState) {
		G_StoredState.interfaces = ptr.unityInterfaces
	}
} else {
	when UNITY_STANDALONE_WIN {
		@(export)
		@(private = "file")
		UnityPluginLoad :: proc "std" (ptr: ^IUnityInterfaces) {
			G_StoredState.interfaces = ptr
		}

		@(export)
		@(private = "file")
		UnityPluginUnload :: proc "std" () {
			G_StoredState = {}
		}
	} else {
		@(export)
		@(private = "file")
		UnityPluginLoad :: proc "c" (ptr: ^IUnityInterfaces) {
			G_StoredState.interfaces = ptr
		}

		@(export)
		@(private = "file")
		UnityPluginUnload :: proc "c" () {
			G_StoredState = {}
		}
	}
}
