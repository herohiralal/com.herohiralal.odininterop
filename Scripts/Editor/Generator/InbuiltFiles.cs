namespace OdinInterop.Editor
{
	internal static class InteropGeneratorInbuiltFiles
	{
		internal static readonly (string, string)[] files = new (string, string)[]
		{
		};

		internal const string ENGINE_BINDINGS_APPEND = @"
UNITY_EDITOR :: #config(UNITY_EDITOR, false)
UNITY_EDITOR_WIN :: #config(UNITY_EDITOR_WIN, false)
UNITY_EDITOR_OSX :: #config(UNITY_EDITOR_OSX, false)
UNITY_EDITOR_LINUX :: #config(UNITY_EDITOR_LINUX, false)

UNITY_STANDALONE :: #config(UNITY_STANDALONE, false)
UNITY_STANDALONE_WIN :: #config(UNITY_STANDALONE_WIN, false)
UNITY_STANDALONE_OSX :: #config(UNITY_STANDALONE_OSX, false)
UNITY_STANDALONE_LINUX :: #config(UNITY_STANDALONE_LINUX, false)

UNITY_IOS :: #config(UNITY_IOS, false)
UNITY_ANDROID :: #config(UNITY_ANDROID, false)

#assert((1 when UNITY_STANDALONE else 0) + (1 when UNITY_IOS else 0) + (1 when UNITY_ANDROID else 0) == 1)
#assert((1 when UNITY_EDITOR_WIN else 0) + (1 when UNITY_EDITOR_OSX else 0) + (1 when UNITY_EDITOR_LINUX else 0) == (1 when UNITY_EDITOR else 0))
#assert((1 when UNITY_STANDALONE_WIN else 0) + (1 when UNITY_STANDALONE_OSX else 0) + (1 when UNITY_STANDALONE_LINUX else 0) == (1 when UNITY_STANDALONE else 0))

@thread_local @private G_OdnTrop_Internal_Ctx: runtime.Context
@thread_local @private G_OdnTrop_Internal_CtxNesting: uint

@(private = ""file"")
G_UnityInterfacesPtr: rawptr

when UNITY_EDITOR {
	@export @(private = ""file"")
	UnityOdnTropInternalSetUnityInterfacesPtr :: proc ""c"" (ptr: rawptr) {
		G_UnityInterfacesPtr = ptr
	}
} else {
	when UNITY_STANDALONE_WIN {
		@export @(private = ""file"")
		UnityPluginLoad :: proc ""std"" (ptr: rawptr) {
			G_UnityInterfacesPtr = ptr
		}

		@export @(private = ""file"")
		UnityPluginUnload :: proc ""std"" () {
			G_UnityInterfacesPtr = nil
		}
	} else {
		@export @(private = ""file"")
		UnityPluginLoad :: proc ""c"" (ptr: rawptr) {
			G_UnityInterfacesPtr = ptr
		}

		@export @(private = ""file"")
		UnityPluginUnload :: proc ""c"" () {
			G_UnityInterfacesPtr = nil
		}
	}
}

CreateUnityContext :: proc() -> runtime.Context {
	return {
		allocator = {procedure = OdnTrop_Internal_DefaultHeapAllocatorFunc, data = nil},
		temp_allocator = {procedure = OdnTrop_Internal_DefaultNilAllocatorFunc, data = nil},
		assertion_failure_proc = OdnTrop_Internal_HandleAssertionFailure,
		logger = {
			procedure = OdnTrop_Internal_Log,
			lowest_level = .Info,
			options = {.Date, .Short_File_Path, .Level, .Date, .Time, .Procedure},
			data = nil,
		},
		random_generator = {
			procedure = OdnTrop_Internal_GenerateRandomNumber,
			data = nil,
		},
	}
}

RandomState :: struct {
	a, b, c, d: i32
}

@(private = ""file"")
OdnTrop_Internal_DefaultNilAllocatorFunc :: proc(
	allocator_data: rawptr,
	mode: runtime.Allocator_Mode,
	size, alignment: int,
	old_memory: rawptr,
	old_size: int,
	loc := #caller_location,
) -> (
	[]byte,
	runtime.Allocator_Error,
) {
	switch mode {
	case .Alloc, .Alloc_Non_Zeroed:
		return nil, .Out_Of_Memory
	case .Free:
		return nil, .None
	case .Free_All:
		return nil, .Mode_Not_Implemented
	case .Resize, .Resize_Non_Zeroed:
		if size == 0 {
			return nil, .None
		}
		return nil, .Out_Of_Memory
	case .Query_Features:
		return nil, .Mode_Not_Implemented
	case .Query_Info:
		return nil, .Mode_Not_Implemented
	}
	return nil, .None
}

@(private = ""file"")
OdnTrop_Internal_DefaultHeapAllocatorFunc :: proc(
	allocatorData: rawptr,
	mode: runtime.Allocator_Mode,
	size, alignment: int,
	oldMemory: rawptr,
	oldSize: int,
	loc := #caller_location,
) -> (
	[]byte,
	runtime.Allocator_Error,
) {
	// majorly from - https://github.com/odin-lang/Odin/blob/fd442b8678baa63be60c3c555d6063386e1d7453/base/runtime/heap_allocator.odin
	// The heap doesn't respect alignment.
	// Instead, we overallocate by `alignment + size_of(rawptr) - 1`, and insert
	// padding. We also store the original pointer returned by heap_alloc right before
	// the pointer we return to the user.

	HeapResize :: proc(ptr: rawptr, oldSize: int, newSize: int, alignment: u32) -> rawptr {
		if newSize == 0 {
			UnityOdnTropInternalFree(ptr, .Persistent)
			return nil
		}

		if ptr == nil {
			return UnityOdnTropInternalMalloc(auto_cast newSize, auto_cast alignment, .Persistent)
		}

		newPtr := UnityOdnTropInternalMalloc(auto_cast newSize, auto_cast alignment, .Persistent)
		if newPtr == nil {
			return nil
		}
		UnityOdnTropInternalMemCopy(newPtr, ptr, (oldSize < newSize ? auto_cast oldSize : auto_cast newSize))
		UnityOdnTropInternalFree(ptr, .Persistent)
		return newPtr
	}

	AlignedAlloc :: proc(
		size, alignment: int,
		oldPtr: rawptr,
		oldSize: int,
		zeroMem := true,
	) -> (
		[]byte,
		runtime.Allocator_Error,
	) {
		a := alignment > align_of(rawptr) ? alignment : align_of(rawptr)
		space := size + a - 1 + size_of(rawptr)

		allocatedMem: rawptr

		forceCopy := oldPtr != nil && alignment > align_of(rawptr)

		if !forceCopy && oldPtr != nil {
			originalOldPtr := ([^]rawptr)(oldPtr)[-1]
			allocatedMem = HeapResize(originalOldPtr, oldSize, space, auto_cast a)
		} else {
			allocatedMem = UnityOdnTropInternalMalloc(auto_cast space, auto_cast a, .Persistent)
			if zeroMem {
				UnityOdnTropInternalMemClear(allocatedMem, auto_cast space)
			}
		}
		alignedMem := rawptr(([^]u8)(allocatedMem)[size_of(rawptr):])

		ptr := uintptr(alignedMem)
		alignedPtr := (ptr - 1 + uintptr(a)) & ~(uintptr(a) - 1)
		if allocatedMem == nil {
			AlignedFree(oldPtr)
			AlignedFree(allocatedMem)
			return nil, .Out_Of_Memory
		}

		alignedMem = rawptr(alignedPtr)
		([^]rawptr)(alignedMem)[-1] = allocatedMem

		if forceCopy {
			UnityOdnTropInternalMemCopy(alignedMem, oldPtr, auto_cast (oldSize > size ? size : oldSize))
			AlignedFree(oldPtr)
		}

		return ([^]byte)(alignedMem)[:(size > 0 ? size : 0)], nil
	}

	AlignedFree :: proc(p: rawptr) {
		if p != nil {
			toFree := ([^]rawptr)(p)[-1]
			UnityOdnTropInternalFree(toFree, .Persistent)
		}
	}

	AlignedResize :: proc(
		p: rawptr,
		oldSize: int,
		newSize: int,
		newAlignment: int,
		zeroMem := true,
	) -> (
		newMemory: []byte,
		err: runtime.Allocator_Error,
	) {
		if p == nil {
			return AlignedAlloc(newSize, newAlignment, nil, oldSize, zeroMem)
		}

		newMemory = AlignedAlloc(newSize, newAlignment, p, oldSize, zeroMem) or_return

		// NOTE: heap_resize does not zero the new memory, so we do it
		if zeroMem && newSize > oldSize {
			newRegion := raw_data(newMemory[oldSize:])

			UnityOdnTropInternalMemClear(newRegion, auto_cast (newSize - oldSize))
		}
		return
	}

	switch mode {
	case .Alloc, .Alloc_Non_Zeroed:
		return AlignedAlloc(size, alignment, nil, 0, mode == .Alloc)

	case .Free:
		AlignedFree(oldMemory)

	case .Free_All:
		return nil, .Mode_Not_Implemented

	case .Resize, .Resize_Non_Zeroed:
		return AlignedResize(oldMemory, oldSize, size, alignment, mode == .Resize)

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

@(private = ""file"")
OdnTrop_Internal_HandleAssertionFailure :: proc(prefix, message: string, loc := #caller_location) -> ! {
	UnityOdnTropInternalPanic(prefix, message, loc.procedure, loc.file_path, loc.line, loc.column)
	panic(message)
}

@(private = ""file"")
OdnTrop_Internal_Log :: proc(
	data: rawptr,
	level: runtime.Logger_Level,
	text: string,
	options: runtime.Logger_Options,
	loc := #caller_location,
) {
	if context.logger.lowest_level > level {
		return
	}

	lt: LogType
	switch level {
	case .Debug, .Info:
		lt = .Log
	case .Warning:
		lt = .Warning
	case .Error:
		lt = .Error
	case .Fatal:
		lt = .Exception
	}

	UnityOdnTropInternalLog(lt, text, loc.procedure, loc.file_path, loc.line, loc.column)
}

@(private = ""file"")
OdnTrop_Internal_GenerateRandomNumber :: proc(data: rawptr, mode: runtime.Random_Generator_Mode, p: []byte) {
	rd := (^RandomState)(data)
	orSt: RandomState = ---
	UnityOdnTropInternalRandomGetState(&orSt.a, &orSt.b, &orSt.c, &orSt.d)
	if rd != nil {
		UnityOdnTropInternalRandomSetState(rd.a, rd.b, rd.c, rd.d)
	}
	defer UnityOdnTropInternalRandomSetState(orSt.a, orSt.b, orSt.c, orSt.d) // reset to original state

	switch mode {
	case .Read:
		if data != nil {
			switch len(p) {
			case size_of(u32):
				val := cast(u32)UnityOdnTropInternalRandomGetNextInt()
				((^u32)(raw_data(p)))^ = val
			case size_of(u64):
				valFirst: u32 = cast(u32)UnityOdnTropInternalRandomGetNextInt()
				valSecond: u32 = cast(u32)UnityOdnTropInternalRandomGetNextInt()
				val: u64 = u64(valFirst) | (u64(valSecond) << 32)
				((^u64)(raw_data(p)))^ = val
			case size_of([2]u64):
				valFirstQ: u32 = cast(u32)UnityOdnTropInternalRandomGetNextInt()
				valSecondQ: u32 = cast(u32)UnityOdnTropInternalRandomGetNextInt()
				valThirdQ: u32 = cast(u32)UnityOdnTropInternalRandomGetNextInt()
				valFourthQ: u32 = cast(u32)UnityOdnTropInternalRandomGetNextInt()
				valFirstH: u64 = u64(valFirstQ) | (u64(valSecondQ) << 32)
				valSecondH: u64 = u64(valThirdQ) | (u64(valFourthQ) << 32)
				val: [2]u64 = {valFirstH, valSecondH}
				((^[2]u64)(raw_data(p)))^ = val
			case:
				pos := i8(0)
				val := u32(0)
				for &v in p {
					if pos == 0 {
						val = cast(u32)UnityOdnTropInternalRandomGetNextInt()
						pos = 3
					}
					v = byte(val)
					val >>= 8
					pos -= 1
				}
			}
		}
	case .Reset:
		seed: i32 = 0
		switch len(p) {
		case 0:
			seed = 0
		case 1:
			seed = i32(p[0])
		case 2:
			seed = i32(p[0]) | i32(p[1]) << 8
		case 3:
			seed = i32(p[0]) | i32(p[1]) << 8 | i32(p[2]) << 16
		case:
			seed = i32(p[0]) | i32(p[1]) << 8 | i32(p[2]) << 16 | i32(p[3]) << 24
		}

		UnityOdnTropInternalRandomInitState(seed)
		if rd != nil {
			UnityOdnTropInternalRandomGetState(&rd.a, &rd.b, &rd.c, &rd.d) // store state
		}

	case .Query_Info:
		if len(p) != size_of(runtime.Random_Generator_Query_Info) {
			return
		}

		info := (^runtime.Random_Generator_Query_Info)(raw_data(p))
		info^ = {.Uniform, .Resettable}
	}
}

@(private = ""file"")
UnityOdnTropInternalAllocateString8: UnityOdnTropInternalAllocateString8Delegate : proc(length: i32) -> string {
	x := make([]u8, auto_cast length)
	return transmute(string)x
}

@(private = ""file"")
UnityOdnTropInternalAllocateString16: UnityOdnTropInternalAllocateString16Delegate : proc(length: i32) -> string16 {
	x := make([]u16, auto_cast length)
	return transmute(string16)x
}

InstantiateObject :: proc {
	InstantiateObjectWithoutTransform,
	InstantiateObjectWithTransform,
}
";
	}
}