using System;
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

        public static implicit operator T[](Slice<T> slice)
        {
            var arr = new T[(int)slice.len];
            for (int i = 0; i < (int)slice.len; i++)
            {
                arr[i] = slice.ptr[i];
            }
            return arr;
        }

        public static implicit operator Slice<T>(T[] arr)
        {
            // TODO
            return default;
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

        public static implicit operator String8(string str)
        {
            // TODO
            return default;
        }

        public override string ToString()
        {
            return System.Text.Encoding.UTF8.GetString(ptr, (int)len);
        }
    }

    public unsafe struct String16 // reinterpretable as Slice<char>
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

        public static implicit operator String16(string str)
        {
            // TODO
            return default;
        }

        public override string ToString()
        {
            return new string(ptr, 0, (int)len);
        }
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

    public struct ObjectHandle<T> where T : Object
    {
        public int id;

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
