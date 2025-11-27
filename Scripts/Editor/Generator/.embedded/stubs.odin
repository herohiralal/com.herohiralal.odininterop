/*
this file includes stubs to satisfy the lsp because the files in this folder (except for this one)
are copied to the final destination where the actual compilation happens

there's a bunch of files there that have the actual implementations of these functions
with the same signatures, with real implementations
*/

package src

import "base:runtime"

UNITY_DEFAULT_TEMP_ALLOCATOR: runtime.Allocator : {}
UNITY_DEFAULT_RANDOM_NUMBER_GENERATOR: runtime.Random_Generator : {}
UnityPanic :: proc(prefix, message: string, loc := #caller_location) -> ! {for {}}

MemCopy :: proc(destination: rawptr, source: rawptr, size: i64) {}
MemMove :: proc(destination: rawptr, source: rawptr, size: i64) {}
MemSet :: proc(destination: rawptr, value: u8, size: i64) {}
MemClr :: proc(destination: rawptr, size: i64) {}
MemTmp :: proc(size: i64, alignment: i32) -> rawptr {return nil}
