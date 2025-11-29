using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using Object = UnityEngine.Object;

namespace OdinInterop
{
    public unsafe struct Allocator
    {
        public void* procedure;
        public void* data;
    }

    public unsafe struct Slice<T> : IDisposable where T : unmanaged
    {
        public T* ptr;
        public IntPtr len;

        public static implicit operator T[](Slice<T> slice)
        {
            var arr = new T[(int)slice.len];
            for (var i = 0; i < (int)slice.len; i++)
                arr[i] = slice.ptr[i];
            return arr;
        }

        public Slice(int length, Allocator allocator)
        {
            var s = EngineBindings.UnityOdnTropInternalAllocateUsingOdnAllocator(sizeof(T), UnsafeUtility.AlignOf<T>(), length, allocator);
            this = s.Reinterpret<T>();
        }

        public Slice(T[] arr, Allocator allocator) : this(arr.Length, allocator)
        {
            if (ptr == null)
                return;

            for (var i = 0; i < arr.Length; i++)
                ptr[i] = arr[i];
        }

        public Slice<U> Reinterpret<U>() where U : unmanaged
        {
            Slice<U> slice;
            slice.ptr = (U*)ptr;
            slice.len = (IntPtr)((len.ToInt64() * sizeof(T)) / sizeof(U));
            return slice;
        }

        public static implicit operator RawSlice(Slice<T> slice)
        {
            RawSlice rawSlice;
            rawSlice.ptr = slice.ptr;
            rawSlice.len = slice.len;
            return rawSlice;
        }

        public static implicit operator Slice<T>(RawSlice rawSlice)
        {
            Slice<T> slice;
            slice.ptr = (T*)rawSlice.ptr;
            slice.len = rawSlice.len;
            return slice;
        }

        public void Dispose() => Dispose(EngineBindings.UnityOdnTropInternalGetMainOdnAllocator());

        public void Dispose(Allocator allocator) => EngineBindings.UnityOdnTropInternalFreeUsingOdnAllocator(Reinterpret<byte>(), allocator);
    }

    public unsafe struct RawSlice // type-unknown slice
    {
        public void* ptr;
        public IntPtr len;
    }

    public unsafe struct String8 : IDisposable // reinterpretable as Slice<byte>
    {
        public byte* ptr; // utf-8
        public IntPtr len;

        public static implicit operator Slice<byte>(String8 str)
        {
            Slice<byte> slice;
            slice.ptr = str.ptr;
            slice.len = str.len;
            return slice;
        }

        public static implicit operator String8(Slice<byte> slice)
        {
            String8 str;
            str.ptr = slice.ptr;
            str.len = slice.len;
            return str;
        }

        public String8(string str, Allocator allocator)
        {
            if (string.IsNullOrEmpty(str))
            {
                this = default;
                return;
            }

            var b = System.Text.Encoding.UTF8.GetBytes(str);
            var s = EngineBindings.UnityOdnTropInternalAllocateUsingOdnAllocator(1, 1, b.Length, allocator);
            if (s.ptr == null)
            {
                this = default;
                return;
            }

            this = s;

            for (var i = 0; i < b.Length; i++)
                ptr[i] = b[i];
        }

        public Slice<byte> AsSlice() => new Slice<byte>() { ptr = ptr, len = len };

        public void Dispose() => AsSlice().Dispose();
        public void Dispose(Allocator allocator) => AsSlice().Dispose(allocator);

        public override readonly string ToString()
        {
            if (ptr == null || len == IntPtr.Zero)
                return string.Empty;

            return System.Text.Encoding.UTF8.GetString(ptr, (int)len);
        }
    }

    public unsafe struct String16 : IDisposable // reinterpretable as Slice<char>
    {
        public char* ptr; // utf-16
        public IntPtr len;

        public static implicit operator Slice<char>(String16 str)
        {
            Slice<char> slice;
            slice.ptr = str.ptr;
            slice.len = str.len;
            return slice;
        }

        public static implicit operator String16(Slice<char> slice)
        {
            String16 str;
            str.ptr = slice.ptr;
            str.len = slice.len;
            return str;
        }

        public String16(string str, Allocator allocator)
        {
            if (string.IsNullOrEmpty(str))
            {
                this = default;
                return;
            }

            var s2 = EngineBindings.UnityOdnTropInternalAllocateUsingOdnAllocator(2, 2, str.Length, allocator);
            if (s2.ptr == null)
            {
                this = default;
                return;
            }

            var s = new String16() { ptr = (char*)s2.ptr, len = s2.len };
            for (var i = 0; i < str.Length; i++)
                s.ptr[i] = str[i];
            this = s;
        }

        public Slice<char> AsSlice() => new Slice<char>() { ptr = ptr, len = len };
        public void Dispose() => AsSlice().Dispose();
        public void Dispose(Allocator allocator) => AsSlice().Dispose(allocator);

        public override string ToString()
        {
            return new string(ptr, 0, (int)len);
        }
    }

    public unsafe struct DynamicArray<T> : IDisposable where T : unmanaged // 'derived' from Slice<T>
    {
        public T* ptr;
        public IntPtr len;
        public IntPtr cap;
        public Allocator allocator;

        public static implicit operator List<T>(DynamicArray<T> arr)
        {
            var list = new List<T>((int)arr.len);
            for (var i = 0; i < (int)arr.len; i++)
            {
                list.Add(arr.ptr[i]);
            }
            return list;
        }

        public DynamicArray(int capacity, Allocator allocator)
        {
            var s = EngineBindings.UnityOdnTropInternalAllocateUsingOdnAllocator(sizeof(T), UnsafeUtility.AlignOf<T>(), capacity, allocator);
            var s2 = s.Reinterpret<T>();
            ptr = s2.ptr;
            len = IntPtr.Zero;
            cap = s2.len;
            this.allocator = allocator;
        }

        public DynamicArray(List<T> list, Allocator allocator)
        {
            var s = EngineBindings.UnityOdnTropInternalAllocateUsingOdnAllocator(sizeof(T), UnsafeUtility.AlignOf<T>(), list.Capacity, allocator);
            if (s.ptr == null)
            {
                this = default;
                return;
            }

            var alt = s.Reinterpret<T>();
            ptr = alt.ptr;
            len = (IntPtr)list.Count;
            cap = alt.len;
            this.allocator = allocator;

            for (var i = 0; i < list.Count; i++)
                ptr[i] = list[i];
        }

        public void Dispose() => Dispose(allocator);
        public void Dispose(Allocator allocator) => new Slice<T>() { ptr = ptr, len = cap }.Dispose(allocator);

        public static implicit operator RawDynamicArray(DynamicArray<T> arr)
        {
            RawDynamicArray rawArr;
            rawArr.ptr = arr.ptr;
            rawArr.len = arr.len;
            rawArr.cap = arr.cap;
            rawArr.allocator = arr.allocator;
            return rawArr;
        }

        public static implicit operator DynamicArray<T>(RawDynamicArray rawArr)
        {
            DynamicArray<T> arr;
            arr.ptr = (T*)rawArr.ptr;
            arr.len = rawArr.len;
            arr.cap = rawArr.cap;
            arr.allocator = rawArr.allocator;
            return arr;
        }
    }

    public unsafe struct RawDynamicArray // type-unknown dynamic array
    {
        public void* ptr;
        public IntPtr len;
        public IntPtr cap;
        public Allocator allocator;
    }

    public struct ObjectHandle<T> where T : Object
    {
        public int id;

        public static implicit operator bool(ObjectHandle<T> handle)
        {
            return handle.id != 0;
        }

        public T value => (T)this;

        public static implicit operator T(ObjectHandle<T> handle)
        {
            return (T)Resources.InstanceIDToObject(handle.id);
        }

        public static implicit operator ObjectHandle<T>(T obj)
        {
            ObjectHandle<T> handle;
            handle.id = obj ? obj.GetInstanceID() : 0;
            return handle;
        }

        public static implicit operator RawObjectHandle(ObjectHandle<T> handle)
        {
            RawObjectHandle rawHandle;
            rawHandle.id = handle.id;
            return rawHandle;
        }

        public static implicit operator ObjectHandle<T>(RawObjectHandle rawHandle)
        {
            ObjectHandle<T> handle;
            handle.id = rawHandle.id;
            return handle;
        }
    }

    public struct RawObjectHandle
    {
        public int id;
    }

    public static unsafe class InteropConversions
    {
    }

    public static class EmptyRefReturn<T>
    {
        private static T s_InternalValue;
        public static ref T corruptedValue => ref s_InternalValue;

        public static void Fill(out T value) => value = default;
    }
}
