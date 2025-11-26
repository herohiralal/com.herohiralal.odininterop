namespace OdinInterop.Editor
{
	internal static class InteropGeneratorInbuiltFiles
	{
		internal const string ENGINE_BINDINGS_APPEND = @"
MemCopy :: UnityOdnTropInternalMemCopy
MemMove :: UnityOdnTropInternalMemMove
MemSet  :: UnityOdnTropInternalMemSet
MemClr  :: UnityOdnTropInternalMemClear

UNITY_DEFAULT_PERSISTENT_ALLOCATOR: runtime.Allocator : {procedure = OdnTrop_Internal_UnityScriptsPersistentAllocatorFunc, data = nil}
UNITY_DEFAULT_TEMP_ALLOCATOR: runtime.Allocator : {procedure = OdnTrop_Internal_UnityScriptsTempAllocatorFunc, data = nil}

@(private = ""file"")
OdnTrop_Internal_UnityScriptsPersistentAllocatorFunc :: proc(
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
	return OdnTrop_Internal_UnityScriptsAllocatorFunc(
		allocatorData,
		mode,
		size, alignment,
		oldMemory,
		oldSize,
		.Persistent,
		loc,
	);
}

@(private = ""file"")
OdnTrop_Internal_UnityScriptsTempAllocatorFunc :: proc(
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
	return OdnTrop_Internal_UnityScriptsAllocatorFunc(
		allocatorData,
		mode,
		size, alignment,
		oldMemory,
		oldSize,
		.Temp,
		loc,
	);
}

@(private = ""file"")
OdnTrop_Internal_UnityScriptsAllocatorFunc :: proc(
	allocatorData: rawptr,
	mode: runtime.Allocator_Mode,
	size, alignment: int,
	oldMemory: rawptr,
	oldSize: int,
	unityAllocator: Unity_Collections_Allocator,
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

	HeapResize :: proc(ptr: rawptr, oldSize: int, newSize: int, alignment: u32, unityAllocator: Unity_Collections_Allocator) -> rawptr {
		if newSize == 0 {
			UnityOdnTropInternalFree(ptr, unityAllocator)
			return nil
		}

		if ptr == nil {
			return UnityOdnTropInternalMalloc(auto_cast newSize, auto_cast alignment, unityAllocator)
		}

		newPtr := UnityOdnTropInternalMalloc(auto_cast newSize, auto_cast alignment, unityAllocator)
		if newPtr == nil {
			return nil
		}
		UnityOdnTropInternalMemCopy(newPtr, ptr, (oldSize < newSize ? auto_cast oldSize : auto_cast newSize))
		UnityOdnTropInternalFree(ptr, unityAllocator)
		return newPtr
	}

	AlignedAlloc :: proc(
		size, alignment: int,
		oldPtr: rawptr,
		oldSize: int,
		unityAllocator: Unity_Collections_Allocator,
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
			allocatedMem = HeapResize(originalOldPtr, oldSize, space, auto_cast a, unityAllocator)
		} else {
			allocatedMem = UnityOdnTropInternalMalloc(auto_cast space, auto_cast a, unityAllocator)
			if zeroMem {
				UnityOdnTropInternalMemClear(allocatedMem, auto_cast space)
			}
		}
		alignedMem := rawptr(([^]u8)(allocatedMem)[size_of(rawptr):])

		ptr := uintptr(alignedMem)
		alignedPtr := (ptr - 1 + uintptr(a)) & ~(uintptr(a) - 1)
		if allocatedMem == nil {
			AlignedFree(oldPtr, unityAllocator)
			AlignedFree(allocatedMem, unityAllocator)
			return nil, .Out_Of_Memory
		}

		alignedMem = rawptr(alignedPtr)
		([^]rawptr)(alignedMem)[-1] = allocatedMem

		if forceCopy {
			UnityOdnTropInternalMemCopy(alignedMem, oldPtr, auto_cast (oldSize > size ? size : oldSize))
			AlignedFree(oldPtr, unityAllocator)
		}

		return ([^]byte)(alignedMem)[:(size > 0 ? size : 0)], nil
	}

	AlignedFree :: proc(p: rawptr, unityAllocator: Unity_Collections_Allocator) {
		if p != nil {
			toFree := ([^]rawptr)(p)[-1]
			UnityOdnTropInternalFree(toFree, unityAllocator)
		}
	}

	AlignedResize :: proc(
		p: rawptr,
		oldSize: int,
		newSize: int,
		newAlignment: int,
		unityAllocator: Unity_Collections_Allocator,
		zeroMem := true,
	) -> (
		newMemory: []byte,
		err: runtime.Allocator_Error,
	) {
		if p == nil {
			return AlignedAlloc(newSize, newAlignment, nil, oldSize, unityAllocator, zeroMem)
		}

		newMemory = AlignedAlloc(newSize, newAlignment, p, oldSize, unityAllocator, zeroMem) or_return

		// NOTE: heap_resize does not zero the new memory, so we do it
		if zeroMem && newSize > oldSize {
			newRegion := raw_data(newMemory[oldSize:])

			UnityOdnTropInternalMemClear(newRegion, auto_cast (newSize - oldSize))
		}
		return
	}

	switch mode {
	case .Alloc, .Alloc_Non_Zeroed:
		return AlignedAlloc(size, alignment, nil, 0, unityAllocator, mode == .Alloc)

	case .Free:
		AlignedFree(oldMemory, unityAllocator)

	case .Free_All:
		return nil, .Mode_Not_Implemented

	case .Resize, .Resize_Non_Zeroed:
		return AlignedResize(oldMemory, oldSize, size, alignment, unityAllocator, mode == .Resize)

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

UnityPanic :: proc(prefix, message: string, loc := #caller_location) -> ! {
	UnityOdnTropInternalPanic(prefix, message, loc.procedure, loc.file_path, loc.line, loc.column)
	panic(message)
}

UNITY_DEFAULT_RANDOM_NUMBER_GENERATOR: runtime.Random_Generator : {procedure = OdnTrop_Internal_GenerateRandomNumber, data = nil}

@(private = ""file"")
OdnTrop_Internal_GenerateRandomNumber :: proc(data: rawptr, mode: runtime.Random_Generator_Mode, p: []byte) {
	rd := (^RandomState)(data)

	switch mode {
	case .Read:
		orSt: RandomState = ---
		if rd != nil {
			UnityOdnTropInternalRandomGetState(&orSt.a, &orSt.b, &orSt.c, &orSt.d) // save original state
			UnityOdnTropInternalRandomSetState(rd.a, rd.b, rd.c, rd.d) // apply custom state
		}

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

		if rd != nil {
			UnityOdnTropInternalRandomGetState(&rd.a, &rd.b, &rd.c, &rd.d) // store new state for the custom one
			UnityOdnTropInternalRandomSetState(orSt.a, orSt.b, orSt.c, orSt.d) // restore original state for the default one
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

		orSt: RandomState = ---
		if rd != nil {
			UnityOdnTropInternalRandomGetState(&orSt.a, &orSt.b, &orSt.c, &orSt.d) // save original state
			UnityOdnTropInternalRandomSetState(rd.a, rd.b, rd.c, rd.d) // apply custom state
		}

		UnityOdnTropInternalRandomInitState(seed)
		if rd != nil {
			UnityOdnTropInternalRandomGetState(&rd.a, &rd.b, &rd.c, &rd.d) // store new state for the custom one
			UnityOdnTropInternalRandomSetState(orSt.a, orSt.b, orSt.c, orSt.d) // restore original state for the default one
		}

	case .Query_Info:
		if len(p) != size_of(runtime.Random_Generator_Query_Info) {
			return
		}

		info := cast(^runtime.Random_Generator_Query_Info)(raw_data(p))
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