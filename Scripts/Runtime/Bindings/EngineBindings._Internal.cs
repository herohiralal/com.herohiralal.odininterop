using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Diagnostics;
using Random = UnityEngine.Random;
using UnityAllocator = Unity.Collections.Allocator;

namespace OdinInterop
{
    [GenerateOdinInterop(odinSrcAppend = InteropGeneratorInbuiltFiles.ENGINE_BINDINGS_APPEND)]
    internal static unsafe partial class EngineBindings
    {

        // mem apis

        private static void MemCopy(void* destination, void* source, long size) => UnsafeUtility.MemCpy(destination, source, size);

        private static void MemMove(void* destination, void* source, long size) => UnsafeUtility.MemMove(destination, source, size);

        private static void MemSet(void* destination, byte value, long size) => UnsafeUtility.MemSet(destination, value, size);

        private static void MemClr(void* destination, long size) => UnsafeUtility.MemClear(destination, size);

        private static void* MemTmp(long size, int alignment) => UnsafeUtility.Malloc(size, alignment, UnityAllocator.Temp);

        // panics

        private static void UnityOdnTropInternalPanic(String8 prefix, String8 message, String8 procedure, String8 file, int line, int column)
        {
            Debug.LogErrorFormat(
                "[Assertion Failure] {0}: {1} (in function {2} at {3}:{4}:{5})",
                prefix.ToString(),
                message.ToString(),
                procedure.ToString(),
                file.ToString(),
                line,
                column
            );
            Utils.ForceCrash(ForcedCrashCategory.FatalError);
        }

        // random api

        private static void UnityOdnTropInternalRandomInitState(int seed) => Random.InitState(seed);

        private static Random.State UnityOdnTropInternalRandomGetState() => Random.state;

        private static void UnityOdnTropInternalRandomSetState(Random.State state) => Random.state = state;

        private static int UnityOdnTropInternalRandomGetNextInt() => Random.Range(int.MinValue, int.MaxValue);

        // allocator functionality imports

        public static partial Allocator UnityOdnTropInternalGetMainOdnAllocator();
        public static partial Allocator UnityOdnTropInternalGetTempOdnAllocator();
        public static partial Slice<byte> UnityOdnTropInternalAllocateUsingOdnAllocator(int tySize, int tyAlignment, int tyCount, Allocator allocator);
        public static partial void UnityOdnTropInternalFreeUsingOdnAllocator(Slice<byte> ptr, Allocator allocator);
    }
}
