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

@(private = "file")
UnityAllocator :: struct {}

when UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN {
	@(private = "file")
	IUnityMemoryManager :: struct {
		CreateAllocator:  proc "std" (areaName, objectName: cstring) -> ^UnityAllocator,
		DestroyAllocator: proc "std" (allocator: ^UnityAllocator),
		Allocate:         proc "std" (
			allocator: ^UnityAllocator,
			size, align: uint,
			file: cstring,
			line: i32,
		) -> rawptr,
		Deallocate:       proc "std" (
			allocator: ^UnityAllocator,
			ptr: rawptr,
			file: cstring,
			line: i32,
		),
		Reallocate:       proc "std" (
			allocator: ^UnityAllocator,
			ptr: rawptr,
			size, align: uint,
			file: cstring,
			line: i32,
		) -> rawptr,
	}
} else {
	@(private = "file")
	IUnityMemoryManager :: struct {
		CreateAllocator:  proc "c" (areaName, objectName: cstring) -> ^UnityAllocator,
		DestroyAllocator: proc "c" (allocator: ^UnityAllocator),
		Allocate:         proc "c" (
			allocator: ^UnityAllocator,
			size, align: uint,
			file: cstring,
			line: i32,
		) -> rawptr,
		Deallocate:       proc "c" (
			allocator: ^UnityAllocator,
			ptr: rawptr,
			file: cstring,
			line: i32,
		),
		Reallocate:       proc "c" (
			allocator: ^UnityAllocator,
			ptr: rawptr,
			size, align: uint,
			file: cstring,
			line: i32,
		) -> rawptr,
	}
}

@(private = "file")
IUnityMemoryManagerGUID: UnityInterfaceGUID : {high = 0xBAF9E57C61A811EC, low = 0xC5A7CC7861A811EC}

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
G_GlobalState: struct {
	interfaces: ^IUnityInterfaces,
} = {}

when UNITY_EDITOR {
	@(export)
	@(private = "file")
	UnityOdnTropInternalSetUnityInterfacesPtr :: proc "c" (ptr: ^StoredState) {
		G_GlobalState.interfaces = ptr.unityInterfaces
	}
} else {
	when UNITY_STANDALONE_WIN {
		@(export)
		@(private = "file")
		UnityPluginLoad :: proc "std" (ptr: ^IUnityInterfaces) {
			G_GlobalState.interfaces = ptr
		}

		@(export)
		@(private = "file")
		UnityPluginUnload :: proc "std" () {
			G_GlobalState = {}
		}
	} else {
		@(export)
		@(private = "file")
		UnityPluginLoad :: proc "c" (ptr: ^IUnityInterfaces) {
			G_GlobalState.interfaces = ptr
		}

		@(export)
		@(private = "file")
		UnityPluginUnload :: proc "c" () {
			G_GlobalState = {}
		}
	}
}
