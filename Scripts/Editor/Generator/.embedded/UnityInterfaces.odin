package src

// IUnityInterface.h ================================================================================

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

// IUnityLog.h ======================================================================================

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

// IUnityMemoryManager.h ============================================================================

when UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN {
	@(private = "file")
	IUnityMemoryManager :: struct {
		CreateAllocator:  proc "std" (areaName, objectName: cstring) -> ^UnityAllocator,
		DestroyAllocator: proc "std" (allocator: ^UnityAllocator),
		Allocate:         proc "std" (allocator: ^UnityAllocator, size, align: uint, file: cstring, line: i32) -> rawptr,
		Deallocate:       proc "std" (allocator: ^UnityAllocator, ptr: rawptr, file: cstring, line: i32),
		Reallocate:       proc "std" (allocator: ^UnityAllocator, ptr: rawptr, size, align: uint, file: cstring, line: i32) -> rawptr,
	}
} else {
	@(private = "file")
	IUnityMemoryManager :: struct {
		CreateAllocator:  proc "c" (areaName, objectName: cstring) -> ^UnityAllocator,
		DestroyAllocator: proc "c" (allocator: ^UnityAllocator),
		Allocate:         proc "c" (allocator: ^UnityAllocator, size, align: uint, file: cstring, line: i32) -> rawptr,
		Deallocate:       proc "c" (allocator: ^UnityAllocator, ptr: rawptr, file: cstring, line: i32),
		Reallocate:       proc "c" (allocator: ^UnityAllocator, ptr: rawptr, size, align: uint, file: cstring, line: i32) -> rawptr,
	}
}

@(private = "file")
IUnityMemoryManagerGUID: UnityInterfaceGUID : {high = 0xBAF9E57C61A811EC, low = 0xC5A7CC7861A811EC}

// IUnityProfiler.h =================================================================================

@(private = "file")
UnityProfilerMarkerId :: distinct u32

@(private = "file")
UnityBuiltinProfilerCategory :: enum {
	Render            = 0,
	Scripts           = 1,
	ManagedJobs       = 2,
	BurstJobs         = 3,
	GUI               = 4,
	Physics           = 5,
	Animation         = 6,
	AI                = 7,
	Audio             = 8,
	AudioJob          = 9,
	AudioUpdateJob    = 10,
	Video             = 11,
	Particles         = 12,
	Gi                = 13,
	Network           = 14,
	Loading           = 15,
	Other             = 16,
	GC                = 17,
	VSync             = 18,
	Overhead          = 19,
	PlayerLoop        = 20,
	Director          = 21,
	VR                = 22,
	Allocation        = 23,
	Memory            = 23,
	Internal          = 24,
	FileIO            = 25,
	UISystemLayout    = 26,
	UISystemRender    = 27,
	VFX               = 28,
	BuildInterface    = 29,
	Input             = 30,
	VirtualTexturing  = 31,
	GPU               = 32,
	Physics2D         = 33,
	NetworkOperations = 34,
	UIDetails         = 35,
	Debug             = 36,
	Jobs              = 37,
	Text              = 38,
}

@(private = "file")
UnityProfilerCategoryId :: distinct u16

@(private = "file")
UnityProfilerCategoryDesc :: struct {
	id:        UnityProfilerCategoryId,
	reserved0: u16,
	rgbaColor: u32,
	name:      cstring,
}

@(private = "file")
UnityProfilerMarkerFlag :: enum u16 {
	ScriptUser         = 1,
	AvailabilityEditor = 2,
	AvailabilityNonDev = 3,
	Warning            = 4,
	ScriptInvoke       = 5,
	ScriptEnterLeave   = 6,
	Counter            = 7,
	VerbosityDebug     = 10,
	VerbosityInternal  = 11,
	VerbosityAdvanced  = 12,
}

@(private = "file")
UnityProfilerMarkerFlags :: distinct bit_set[UnityProfilerMarkerFlag;u16]

@(private = "file")
UnityProfilerMarkerEventType :: enum u16 {
	Begin  = 0,
	End    = 1,
	Single = 2,
}

@(private = "file")
UnityProfilerMarkerDesc :: struct {
	callback:     rawptr,
	id:           UnityProfilerMarkerId,
	flags:        UnityProfilerMarkerFlags,
	categoryId:   UnityProfilerCategoryId,
	name:         cstring,
	metaDataDesc: rawptr,
}

@(private = "file")
UnityProfilerMarkerDataType :: enum u8 {
	None = 0,
	InstanceId = 1,
	Int32 = 2,
	UInt32 = 3,
	Int64 = 4,
	UInt64 = 5,
	Float = 6,
	Double = 7,
	String = 8,
	String16 = 9,
	Blob8 = 11,
	GfxResourceId = 12,
	Count,
}

@(private = "file")
UnityProfilerMarkerDataUnit :: enum u8 {
	Undefined       = 0,
	TimeNanoseconds = 1,
	Bytes           = 2,
	Count           = 3,
	Percent         = 4,
	FrequencyHz     = 5,
}

@(private = "file")
UnityProfilerMarkerData :: struct {
	type:      UnityProfilerMarkerDataType,
	reserved0: u8,
	reserved1: u16,
	size:      u32,
	ptr:       rawptr,
}

@(private = "file")
UnityProfilerFlowEventType :: enum u8 {
	Begin        = 0,
	ParallelNext = 1,
	End          = 2,
	Next         = 3,
}

@(private = "file")
UnityProfilerCounterFlag :: enum u16 {
	FlushOnEndOfFrame  = 1,
	ResetToZeroOnFlush = 2,
	Atomic             = 3,
	Getter             = 4,
}

@(private = "file")
UnityProfilerCounterFlags :: distinct bit_set[UnityProfilerCounterFlag;u16]

@(private = "file")
UnityProfilerThreadId :: distinct u64

@(private = "file")
UnityProfilerCounterStatePtrCallback :: proc "c" (userData: rawptr)

when UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN {
	@(private = "file")
	IUnityProfilerV2 :: struct {
		EmitEvent:             proc "std" (markerDesc: ^UnityProfilerMarkerDesc, eventType: UnityProfilerMarkerEventType, eventDataCount: u16, eventData: ^UnityProfilerMarkerData),
		IsEnabled:             proc "std" () -> b32,
		IsAvailable:           proc "std" () -> b32,
		CreateMarker:          proc "std" (desc: ^^UnityProfilerMarkerDesc, name: cstring, category: UnityProfilerCategoryId, flags: UnityProfilerMarkerFlags, eventDataCount: i32) -> i32,
		SetMarkerMetadataName: proc "std" (desc: ^UnityProfilerMarkerDesc, index: i32, metadataName: cstring, metadataType: UnityProfilerMarkerDataType, metadataUnit: UnityProfilerMarkerDataUnit) -> i32,
		CreateCategory:        proc "std" (category: ^UnityProfilerCategoryId, name: cstring, unused: u32) -> i32,
		RegisterThread:        proc "std" (threadId: ^UnityProfilerThreadId, groupName: cstring, name: cstring) -> i32,
		UnregisterThread:      proc "std" (threadId: UnityProfilerThreadId) -> i32,
		CreateCounterValue:    proc "std" (category: UnityProfilerCategoryId, name: cstring, flags: UnityProfilerMarkerFlags, valueType: UnityProfilerMarkerDataType, valueUnit: UnityProfilerMarkerDataUnit, valueSize: uint, counterFlags: UnityProfilerCounterFlags, activateFunc: UnityProfilerCounterStatePtrCallback, deactivateFunc: UnityProfilerCounterStatePtrCallback, userData: rawptr) -> rawptr,
		FlushCounterValue:     proc "std" (counter: rawptr),
	}
} else {
	@(private = "file")
	IUnityProfilerV2 :: struct {
		EmitEvent:             proc "c" (markerDesc: ^UnityProfilerMarkerDesc, eventType: UnityProfilerMarkerEventType, eventDataCount: u16, eventData: ^UnityProfilerMarkerData),
		IsEnabled:             proc "c" () -> b32,
		IsAvailable:           proc "c" () -> b32,
		CreateMarker:          proc "c" (desc: ^^UnityProfilerMarkerDesc, name: cstring, category: UnityProfilerCategoryId, flags: UnityProfilerMarkerFlags, eventDataCount: i32) -> i32,
		SetMarkerMetadataName: proc "c" (desc: ^UnityProfilerMarkerDesc, index: i32, metadataName: cstring, metadataType: UnityProfilerMarkerDataType, metadataUnit: UnityProfilerMarkerDataUnit) -> i32,
		CreateCategory:        proc "c" (category: ^UnityProfilerCategoryId, name: cstring, unused: u32) -> i32,
		RegisterThread:        proc "c" (threadId: ^UnityProfilerThreadId, groupName: cstring, name: cstring) -> i32,
		UnregisterThread:      proc "c" (threadId: UnityProfilerThreadId) -> i32,
		CreateCounterValue:    proc "c" (category: UnityProfilerCategoryId, name: cstring, flags: UnityProfilerMarkerFlags, valueType: UnityProfilerMarkerDataType, valueUnit: UnityProfilerMarkerDataUnit, valueSize: uint, counterFlags: UnityProfilerCounterFlags, activateFunc: UnityProfilerCounterStatePtrCallback, deactivateFunc: UnityProfilerCounterStatePtrCallback, userData: rawptr) -> rawptr,
		FlushCounterValue:     proc "c" (counter: rawptr),
	}
}

@(private = "file")
IUnityProfilerV2GUID: UnityInterfaceGUID : {high = 0xB957E0189CB6A30B, low = 0x83CE589AE85B9068}


// UNITY INTERFACES END HERE ========================================================================

// all thet global state
@(private = "file")
G_GlobalState: struct {
	interfaces: ^IUnityInterfaces,
	logger:     ^IUnityLog,
	profiler:   ^IUnityProfilerV2,
	cached:     CachedState,
} = {}

CachedState :: struct {
	// all cached state goes here
	// on editor, this will be cached to maintain between hot reloads
	// stuff like created allocators/profiler markers can go here
	// ANY CHANGE TO THIS WILL REQUIRE EDITOR RESTART
}

when UNITY_EDITOR {

	// KEEP IN SYNC WITH `StoredState` IN `Binder.c` AND `OdinCompiler.cs`!!!!!
	// DO NOT CHANGE
	@(private = "file")
	EditorStoredState :: struct {
		unityInterfaces: ^IUnityInterfaces,
		libHandle:       rawptr, // handle of self loaded library

		// all the native stuff starts here
		initialised:     b64,
		cached:          CachedState,
	}

	#assert(size_of(EditorStoredState) <= 1024) // ensure we have enough space for future use
	#assert(align_of(EditorStoredState) == 8)

	@(export)
	@(private = "file")
	UnityOdnTropInternalInitialiseForEditor :: proc "c" (ptr: ^EditorStoredState) {
		G_GlobalState.interfaces = ptr.unityInterfaces
		UnityOdnTropInternalStaticInitialise()

		if !ptr.initialised {
			UnityOdnTropInternalInitialiseCachedState()
			ptr.cached = G_GlobalState.cached
			ptr.initialised = true
		}
	}
} else {
	when UNITY_STANDALONE_WIN {
		@(export)
		@(private = "file")
		UnityPluginLoad :: proc "std" (ptr: ^IUnityInterfaces) {
			G_GlobalState.interfaces = ptr
			UnityOdnTropInternalStaticInitialise()
			UnityOdnTropInternalInitialiseCachedState()
		}

		@(export)
		@(private = "file")
		UnityPluginUnload :: proc "std" () {
			UnityOdnTropInternalShutdownCachedState()
			G_GlobalState = {}
		}
	} else {
		@(export)
		@(private = "file")
		UnityPluginLoad :: proc "c" (ptr: ^IUnityInterfaces) {
			G_GlobalState.interfaces = ptr
			UnityOdnTropInternalInitialiseCachedState()
			UnityOdnTropInternalStaticInitialise()
		}

		@(export)
		@(private = "file")
		UnityPluginUnload :: proc "c" () {
			UnityOdnTropInternalShutdownCachedState()
			G_GlobalState = {}
		}
	}
}

@(private = "file")
UnityOdnTropInternalStaticInitialise :: proc "contextless" () {
	// only do here things that are safe and don't require any caching

	G_GlobalState.logger = cast(^IUnityLog)(G_GlobalState.interfaces.GetInterface(IUnityLogGUID))
	G_GlobalState.profiler = cast(^IUnityProfilerV2)(G_GlobalState.interfaces.GetInterface(IUnityProfilerV2GUID))
}

UnityOdnTropInternalInitialiseCachedState :: proc "contextless" () {
}

UnityOdnTropInternalShutdownCachedState :: proc "contextless" () {
}
