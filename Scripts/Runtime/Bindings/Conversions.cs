using System;

namespace OdinInterop
{
    public unsafe struct Slice<T> where T : unmanaged
    {
        public T* ptr;
        public UIntPtr len;
    }

    public unsafe struct RawSlice // type-unknown slice
    {
        public void* ptr;
        public UIntPtr len;
    }

    public unsafe struct String // reinterpretable as Slice<byte>
    {
        public byte* ptr; // utf-8
        public UIntPtr len;
    }

    public static unsafe class InteropConversions
    {
        // public static uns
    }

    public static class EmptyRefReturn<T>
    {
        private static T s_InternalValue;
        public static ref T corruptedValue => ref s_InternalValue;

        public static void Fill(out T value) => value = default;
    }
}
