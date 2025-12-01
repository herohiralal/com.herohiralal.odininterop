# Odin Interop

Simple UPM-compatible package to use [Odin](https://github.com/odin-lang/Odin) language in Unity projects via P/Invoke interoperability.

## Motivation

From personal experience, I've found that it's more of a pain to use a garbage-collected language like C# for games, compared to a lower-level language with manual memory management.

You forgot to set a few references to `null` and there's a long reference-chain beginning from some static variable somewhere, and now you have memory leak. What's worse is that sometimes it keeps getting worse with every time you change the scene. In some contexts, this would be the definition of a memory leak.

It's also utterly painful to deal with GC-induced frametime spikes. To make it worse, these will usually happen at the worst possible times (not because of some cosmic luck, but because these are the statistically likely poitns where you ended up needing to allocate a few lists and GC decided to smooth things over for you).

Combined with the fact that as the project grows larger, the compile times (coupled with domain reloads) for C# scripts also grow longer. Big plus to not have to close the editor, but might as well have, because it takes 5-6 minutes to compile at times. Plus not being able to reload the code _while in play mode_ makes it even worse.

Would rather just use a compiled language instead. C/C++ are also a good option, but the compilation processes are a nightmare over there. And then the compile times are also insanely long. Plus in C++, very easy to shoot yourself in the foot with a bunch of unnecessary abstractions like auto-casts, template hell, macro unreadability, etc. On the other side, C is way too basic and lacks a lot of modern features like generics, slices, etc.

Something like Rust presents a lot of friction for development because of borrow-checker and the steep learning curve. Plus the compilation times are a nightmare there as well. Plus you never know whether the code you imported from a package is compatible with your target platforms or not.

This is where Odin makes for an ideal solution. Fast compile times, bunch of modern features but without syntax/semantic commplications, easy interoperability, etc. Plus the codebase is simple enough to add platform support if required.

## How To Use

### Basic usage

How to use at the most basic level:

- Add to your project using Unity Package Manager.
- Odin code lives in `Assets/.odinInterop/Source/` directory.
- Write your Odin code in `.odin` files.
  - [Odin Overview](https://odin-lang.org/docs/overview/)
- `Ctrl`+`Alt`+`R` to recompile the Odin interop library.

### Main Unity callbacks

If you implement these functions, they will automatically be called from a `DontDestroyOnLoad` MonoBehaviour that gets created on startup (`BeforeSceneLoad` in `RuntimeInitializeOnLoadMethod` terms).

```odin
OnGlobalAwake :: proc()
OnGlobalStart :: proc()
OnGlobalFixedUpdate :: proc(dt: f32)
OnGlobalUpdate :: proc(dt, unscaled: f32)
OnGlobalLateUpdate :: proc(dt, unscaled: f32)
OnGlobalDestroy :: proc()
```
### Exposing more C# functions

Very straightforward, but also maybe unnecessary. The main package comes with batteries included.

It contains bindings for most of the commonly used Unity APIs.

- Add a dependency to `com.herohiralal.odininterop` Assembly in your Assembly Definition file.
- Expose the functionality as a static class:

```csharp
using OdinInterop;
using UnityEngine;

namespace MyNs // can be in any namespace
{
    [GenerateGenerateOdinInterop]
    public static partial class ExposeToOdin // can be internal; needs to be static and partial
    {
        private static void ExampleFunction(int a, float b) // must be private
        {
            Debug.Log($"Called from Odin: {a}, {b}.");
        }
    }
}
```

- Call from Odin:

```odin
some_proc :: proc() {
    MyNs_ExposeToOdin_ExampleFunction(42, 3.14); // call as `namespace_classname_functionname`
                                                 // skip namespace if in global namespace
}
```

- The bindings also support default values for parameters.
  - For structs, using `default` as the default value means zero-initialised on Odin side.
  - Primitives (booleans, integers, floats) allow more expressivity with default values.
  - Allocators using `default` as the default value means `context.allocator` on Odin side.
  - Quaternions using `default` as the default value means `quaternion128(1)` (/Identity Quaternion) on Odin side.

### Exposing Odin functions

Slightly more complex.

- In the same static class from the previous section, add a partial function and implement in Odin.

```csharp
using OdinInterop;
using UnityEngine;

namespace MyNs // can be in any namespace
{
    [GenerateGenerateOdinInterop]
    public static partial class ExposeToOdin // can be internal; needs to be static and partial
    {
        public static partial int FunctionInOdin(float x); // must be public and partial
    }
}
```

- Implement in Odin:

```odin
import "core:log"

MyNs_ExposeToOdin_FunctionInOdin :: proc(x: f32) -> i32 { // declare as `namespace_classname_functionname`
    log.warnf("Called from C#: %f", x)
    return i32(x * 2)
}

/*
Can also declare as:

MyNs_ExposeToOdin_FunctionInOdin: MyNs_ExposeToOdin_FunctionInOdinDelegate : proc(...) -> ... {
    // implement
}

This will ensure that the signature of the function is checked at compile-time, and a
compiler error is raised at the line of declaration instead of at the line of usage in generated code.
*/
```

### Supported Interop Types

Because Odin uses C ABI for interoperability, only certain types are supported for interop.

Support for more C# shenanigans is WIP.

Here are the supported types:

- Primitives:
  - `bool` <-> `bool`
  - `byte` <-> `u8`
  - `sbyte` <-> `i8`
  - `short` <-> `i16`
  - `ushort` <-> `u16`
  - `int` <-> `i32`
  - `uint` <-> `u32`
  - `long` <-> `i64`
  - `ulong` <-> `u64`
  - `float` <-> `f32`
  - `double` <-> `f64`
- Enums:
  - `T` <-> `T` (gets recreated 1:1 in generated code)
- Strings:
  - `OdinInterop.String16` <-> `string16` (UTF-16; auto-converting from C# `string`)
  - `OdinInterop.String8` <-> `string8` (UTF-8 str; auto-converting from C# `string`)
- Collections:
  - `OdinInterop.Slice<T>` <-> `[]T` (Odin slices; auto-converting from C# `T[]`)
  - `OdinInterop.DynamicArray<T>` <-> `[dynamic]T` (Odin dynamic arrays; auto-converting from C# `List<T>`)
- Odin Internals:
  - `OdinInterop.Allocator` <-> `runtime.Allocator` (Odin memory allocator; useful for creating unmanaged Odin strings/collections in C#)
- Unity Objects:
  - `T` <-> `OdinInterop.ObjectHandle<T>` (unmanaged wrapper handle; auto-converting from C# Objects; all classes deriving from `UnityEngine.Object` are supported)

## To-Do

Below are the currently supported platforms:

- [x] Editor Experience
  - [x] Hot reloading outside PlayMode (with rebinding)
  - [x] Hot reloading inside PlayMode (with dynamic rebinding)
  - [x] Unity domain-reload compatibility
  - [x] Roslyn Source-Generator based BindGen
- [x] Platform Support
  - [ ] Windows
    - [x] Standalone Compilation
    - [x] Editor Reloading
    - [ ] Runtime Reloading
  - [ ] macOS
    - [x] Standalone Compilation
    - [x] Editor Reloading
    - [ ] Runtime Reloading
  - [ ] Linux
    - [x] Standalone Compilation
    - [x] Editor Reloading
    - [ ] Runtime Reloading
  - [x] iOS
    - [x] Standalone Compilation
  - [x] Android
    - [x] Standalone Compilation
    - [x] Runtime Reloading
- [ ] Bindings
  - [x] GameObject
  - [x] Transform
  - [x] Rendering
  - [ ] VFX
  - [x] Audio
  - [x] Physics
  - [ ] Input*
  - [x] Animations
  - [ ] Timeline
  - [ ] Camera
  - [ ] Playables
  - [ ] Time
  - [ ] Graphics

> \* - maybe
