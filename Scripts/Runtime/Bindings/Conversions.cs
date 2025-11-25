using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OdinInterop
{
    public unsafe struct Allocator
    {
        public void* procedure;
        public void* data;
    }

    public unsafe struct Slice<T> where T : unmanaged
    {
        public T* ptr;
        public IntPtr len;
    }

    public unsafe struct RawSlice // type-unknown slice
    {
        public void* ptr;
        public IntPtr len;
    }

    public unsafe struct String8 // reinterpretable as Slice<byte>
    {
        public byte* ptr; // utf-8
        public IntPtr len;
    }

    public unsafe struct String16 // reinterpretable as Slice<char>
    {
        public char* ptr; // utf-16
        public IntPtr len;
    }

    public unsafe struct DynamicArray<T> where T : unmanaged // 'derived' from Slice<T>
    {
        public T* ptr;
        public IntPtr len;
        public IntPtr cap;
        public Allocator allocator;

        public static implicit operator List<T>(DynamicArray<T> arr)
        {
            var list = new List<T>((int)arr.len);
            for (int i = 0; i < (int)arr.len; i++)
            {
                list.Add(arr.ptr[i]);
            }
            return list;
        }

        public static implicit operator DynamicArray<T>(List<T> list)
        {
            // TODO: need to export allocator functions and all that
            return default;
        }

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

    public static unsafe class InteropConversions
    {
        public static int ConvertToInteroperable(this Object obj) => obj ? obj.GetInstanceID() : 0;
        public static T ConvertFromInteroperable<T>(this int id) where T : Object => id == 0 ? null : Resources.InstanceIDToObject(id) as T;
    }

    public static class EmptyRefReturn<T>
    {
        private static T s_InternalValue;
        public static ref T corruptedValue => ref s_InternalValue;

        public static void Fill(out T value) => value = default;
    }
}
