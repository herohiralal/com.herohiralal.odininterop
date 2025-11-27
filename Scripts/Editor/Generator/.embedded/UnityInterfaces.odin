package src

import "base:runtime"
import "core:strings"

// IUnityInterface.h ================================================================================

@(private = "file")
UnityInterfaceGUID :: struct {
	high, low: u64,
}

@(private = "file")
IUnityInterface :: struct {}

// these interfaces have a couple empty fields because
// the real functions there use a copy constructor for the GUIDs
// which breaks the C ABI, so we instead use the other overloads
when UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN {
	@(private = "file")
	IUnityInterfaces :: struct {
		_:                 proc "std" (guid: UnityInterfaceGUID) -> ^IUnityInterface,
		_:                 proc "std" (guid: UnityInterfaceGUID, ptr: ^IUnityInterface),
		GetInterface:      proc "std" (h: u64, l: u64) -> ^IUnityInterface,
		RegisterInterface: proc "std" (h: u64, l: u64, ptr: ^IUnityInterface),
	}
} else {
	@(private = "file")
	IUnityInterfaces :: struct {
		_:                 proc "c" (guid: UnityInterfaceGUID) -> ^IUnityInterface,
		_:                 proc "c" (guid: UnityInterfaceGUID, ptr: ^IUnityInterface),
		GetInterface:      proc "c" (h: u64, l: u64) -> ^IUnityInterface,
		RegisterInterface: proc "c" (h: u64, l: u64, ptr: ^IUnityInterface),
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

// IUnityMemoryManager.h ============================================================================

@(private = "file")
UnityAllocator :: struct {}

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
	interfaces:    ^IUnityInterfaces,
	logger:        ^IUnityLog,
	profiler:      ^IUnityProfilerV2,
	memoryManager: ^IUnityMemoryManager,
	cached:        CachedState,
} = {}

CachedState :: struct {
	// all cached state goes here
	// on editor, this will be cached to maintain between hot reloads
	// stuff like created allocators/profiler markers can go here
	// ANY CHANGE TO THIS WILL REQUIRE EDITOR RESTART
	heapAllocator: ^UnityAllocator,
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
			UnityOdnTropInternalStaticInitialise()
			UnityOdnTropInternalInitialiseCachedState()
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

	GetInterface :: proc "contextless" ($T: typeid, g: UnityInterfaceGUID) -> ^T {
		interfaces := G_GlobalState.interfaces
		intf := interfaces.GetInterface(g.high, g.low)
		return cast(^T)(intf)
	}

	G_GlobalState.logger = GetInterface(IUnityLog, IUnityLogGUID)
	G_GlobalState.profiler = GetInterface(IUnityProfilerV2, IUnityProfilerV2GUID)
	G_GlobalState.memoryManager = GetInterface(IUnityMemoryManager, IUnityMemoryManagerGUID)
}

UnityOdnTropInternalInitialiseCachedState :: proc "contextless" () {
	G_GlobalState.cached.heapAllocator = G_GlobalState.memoryManager.CreateAllocator("OdinInterop", "HeapAllocator")
}

UnityOdnTropInternalShutdownCachedState :: proc "contextless" () {
	G_GlobalState.memoryManager.DestroyAllocator(G_GlobalState.cached.heapAllocator)
}

UNITY_MAIN_ALLOCATOR: runtime.Allocator : {procedure = OdnTrop_Internal_DefaultHeapAllocatorFunc, data = nil}

UNITY_MAIN_LOGGER: runtime.Logger : {procedure = OdnTrop_Internal_MainLogFunc, lowest_level = .Info, options = {.Date, .Short_File_Path, .Level, .Date, .Time, .Procedure}, data = nil}

@(private = "file")
OdnTrop_Internal_DefaultHeapAllocatorFunc :: proc(allocatorData: rawptr, mode: runtime.Allocator_Mode, size, alignment: int, oldMemory: rawptr, oldSize: int, loc := #caller_location) -> ([]byte, runtime.Allocator_Error) {
	// majorly from - https://github.com/odin-lang/Odin/blob/fd442b8678baa63be60c3c555d6063386e1d7453/base/runtime/heap_allocator.odin
	// The heap doesn't respect alignment.
	// Instead, we overallocate by `alignment + size_of(rawptr) - 1`, and insert
	// padding. We also store the original pointer returned by heap_alloc right before
	// the pointer we return to the user.

	fileCStr := strings.clone_to_cstring(loc.file_path, UNITY_DEFAULT_TEMP_ALLOCATOR)

	HeapResize :: proc(ptr: rawptr, oldSize: int, newSize: int, alignment: u32, fileCStr: cstring, line: i32) -> rawptr {
		if newSize == 0 {
			G_GlobalState.memoryManager.Deallocate(G_GlobalState.cached.heapAllocator, ptr, fileCStr, line)
			return nil
		}

		if ptr == nil {
			return G_GlobalState.memoryManager.Allocate(G_GlobalState.cached.heapAllocator, auto_cast newSize, auto_cast alignment, fileCStr, line)
		}

		return G_GlobalState.memoryManager.Reallocate(G_GlobalState.cached.heapAllocator, ptr, auto_cast newSize, auto_cast alignment, fileCStr, line)
	}

	AlignedAlloc :: proc(size, alignment: int, oldPtr: rawptr, oldSize: int, zeroMem: bool, fileCStr: cstring, line: i32) -> ([]byte, runtime.Allocator_Error) {
		a := alignment > align_of(rawptr) ? alignment : align_of(rawptr)
		space := size + a - 1 + size_of(rawptr)

		allocatedMem: rawptr

		forceCopy := oldPtr != nil && alignment > align_of(rawptr)

		if !forceCopy && oldPtr != nil {
			originalOldPtr := ([^]rawptr)(oldPtr)[-1]
			allocatedMem = HeapResize(originalOldPtr, oldSize, space, auto_cast a, fileCStr, line)
		} else {
			allocatedMem = G_GlobalState.memoryManager.Allocate(G_GlobalState.cached.heapAllocator, auto_cast space, auto_cast a, fileCStr, line)
			if zeroMem {
				MemClr(allocatedMem, auto_cast space)
			}
		}
		alignedMem := rawptr(([^]u8)(allocatedMem)[size_of(rawptr):])

		ptr := uintptr(alignedMem)
		alignedPtr := (ptr - 1 + uintptr(a)) & ~(uintptr(a) - 1)
		if allocatedMem == nil {
			AlignedFree(oldPtr, fileCStr, line)
			AlignedFree(allocatedMem, fileCStr, line)
			return nil, .Out_Of_Memory
		}

		alignedMem = rawptr(alignedPtr)
		([^]rawptr)(alignedMem)[-1] = allocatedMem

		if forceCopy {
			MemCopy(alignedMem, oldPtr, auto_cast (oldSize > size ? size : oldSize))
			AlignedFree(oldPtr, fileCStr, line)
		}

		return ([^]byte)(alignedMem)[:(size > 0 ? size : 0)], nil
	}

	AlignedFree :: proc(p: rawptr, fileCStr: cstring, line: i32) {
		if p != nil {
			toFree := ([^]rawptr)(p)[-1]
			G_GlobalState.memoryManager.Deallocate(G_GlobalState.cached.heapAllocator, toFree, fileCStr, line)
		}
	}

	AlignedResize :: proc(p: rawptr, oldSize: int, newSize: int, newAlignment: int, zeroMem: bool, fileCStr: cstring, line: i32) -> (newMemory: []byte, err: runtime.Allocator_Error) {
		if p == nil {
			return AlignedAlloc(newSize, newAlignment, nil, oldSize, zeroMem, fileCStr, line)
		}

		newMemory = AlignedAlloc(newSize, newAlignment, p, oldSize, zeroMem, fileCStr, line) or_return

		// NOTE: heap_resize does not zero the new memory, so we do it
		if zeroMem && newSize > oldSize {
			newRegion := raw_data(newMemory[oldSize:])

			MemClr(newRegion, auto_cast (newSize - oldSize))
		}
		return
	}

	switch mode {
	case .Alloc, .Alloc_Non_Zeroed:
		return AlignedAlloc(size, alignment, nil, 0, mode == .Alloc, fileCStr, loc.line)

	case .Free:
		AlignedFree(oldMemory, fileCStr, loc.line)

	case .Free_All:
		return nil, .Mode_Not_Implemented

	case .Resize, .Resize_Non_Zeroed:
		return AlignedResize(oldMemory, oldSize, size, alignment, mode == .Resize, fileCStr, loc.line)

	case .Query_Features:
		set := (^runtime.Allocator_Mode_Set)(oldMemory)
		if set != nil {
			set^ = {.Alloc, .Alloc_Non_Zeroed, .Free, .Resize, .Resize_Non_Zeroed, .Query_Features}
		}
		return nil, nil

	case .Query_Info:
		return nil, .Mode_Not_Implemented
	}

	return nil, nil
}

@(private = "file")
OdnTrop_Internal_MainLogFunc :: proc(data: rawptr, level: runtime.Logger_Level, text: string, options: runtime.Logger_Options, loc := #caller_location) {
	if context.logger.lowest_level > level {
		return
	}

	lt: UnityLogType
	switch level {
	case .Error, .Fatal:
		lt = .Error
	case .Warning:
		lt = .Warning
	case .Debug, .Info:
		lt = .Log
	}

	messageCStr := strings.clone_to_cstring(text, UNITY_DEFAULT_TEMP_ALLOCATOR, loc)
	fileCStr := strings.clone_to_cstring(loc.file_path, UNITY_DEFAULT_TEMP_ALLOCATOR, loc)

	G_GlobalState.logger.Log(lt, messageCStr, fileCStr, loc.line)
}
