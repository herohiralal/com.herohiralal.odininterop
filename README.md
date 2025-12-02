# Odin Interop

Simple UPM-compatible package to use [Odin](https://github.com/odin-lang/Odin) language in Unity projects via P/Invoke interoperability.

Here's a short pitch:

- Making a mobile game? This lets you recompile your game logic _on a device while the the game is running_.
- Making a PC game? This lets you recompile your game logic _in the editor while in Play Mode_.

## Motivation

### Problems

Having worked on a large project in Unity, my personal experience has been less than stellar.

- Garbage Collection does not meaningfully protect your code against memory leaks.
  - Forgot to `.Clear()` a static `List<T>`/`Dictionary<K, V>`? Leaked memory.
  - Forgot to set a static reference to `null`? Leaked memory.
  - If your game requires a bunch of scene transitions, you might end up incrementally leaking memory every time.
  - The way Unity's `Resources.UnloadUnusedAssets()` works, it checks for static variables as well.
    - So, you might end up leaking assets as well.
    - If this comes from AssetBundles/Addressables, you're in a world of pain because of how it internally works.
  - The way 'memory usage' works on mobile devices is that the OS will kill your app if it uses too much memory.
    - HOWEVER, everything will be fine until you hit this level of tech debt.
    - So you might end up going months until you must fix the problem.
  - The bad news is, this is already too late a point to realistically fix your issues.
- Garbage Collection causes huge frame-rate spikes.
  - Almost all of C#'s standard library code is written without these considerations in mind.
  - Because of the way gameplay code, lazy-inits, lazy-loads, etc. work, this is statistically likelier to happen at the worst possible moments.
    - This leads to janky gameplay experiences.
  - You can try to mitigate this by using object-pools, avoiding allocations, etc., but if you're going that far, how is a managed language _really_ helping you?
- C# compile times and domain reloads are horrific.
  - And it only gets worse as the project grows larger.
  - Domain reloads are particularly painful because you can't reload code while in Play Mode.
    - This kills iteration times.
  - To 'recompile', you must go through: (stop play mode: 15-20s) -> (recompile + domain reload: 60-180s) -> (start play mode, which will reload the domain: 60-180s) -> (initialisation code executes: 5-10s) -> (reach desired state: 5-10s).
    - This easily adds up to multiple minutes of waiting time for every small code change.
- C# language features make it really easy to create a mess.
  - Implicit casts, boxing/unboxing, inheritance hell; it's all a mess and needs to go.
  - Even if your coding standards mandate not using these things, it's really easy for new team members, juniors or outsourced contractors to accidentally introduce these issues.

### A New Dawn

So, that brings up to the question - what could be the alternative. Ideally, we'd want a solution that:

- Compiles fast. Reloads fast. Lets you iterate fast.
- Allows for manual memory management techniques (Arenas, Pools, etc.).
- Has modern language features without unnecessary complexity.
- Easy interoperability with C# and Unity APIs.
- Allows faster iterations by recompiling during Play Mode, and in a build.

**And all of that, is what this package provides.** No other nonsense.

### Why Odin?

Let's look at some alternatives.

#### (Insert Any Interpreted Language Here)

- Does not work on AOT platforms like iOS or consoles.
- Unity C# only works because of IL2CPP.

#### Jai

- Is in closed beta. Can't rely on everyone having access to the compiler.
- At some point, might add support for Jai interop as well.

#### C/C++

- In C++, it's easy to shoot yourself in the foot with unnecessary abstractions. Arguably worse than C#.
  - C has the opposite problem. It has initialiser lists, but that's about the only important thing it has.
- Compile times are horrible. Even with precompiled headers and unity builds, it's still not great.
  - C is a bit better in this regard, but still not ideal.
- Setting up cross-compilation across different platforms is a nightmare.
- Macros are too unhygienic and lead to unreadable code.
- Requires too much work into creating platform-specific wrappers for standard library functions.
  - C is even worse in this regard, because Windows straight up does not support a lot of stuff.

#### Rust

- Steep learning curve.
- Borrow-checker leads to a lot of friction during development.
- Compile times are horrible. Even with incremental compilation, it's still not great.
- No reliability of imported packages working on your target platforms.
  - Too much volatility due to the package manager being integrated into the build system.

#### And Finally

This is where we come to Odin, which is a perfect middle-ground from all of the above.

- Great compilation times. Even large projects build very fast.
- Modern language features without unnecessary complexity.
- Easy interoperability with any language that supports C ABI (C#, C/C++, etc.).
- Great, and extremely readable standard library.
- Simple toolchain that is easy to set up across different platforms.
- It works. The existence of this package is the proof.

## How To Use

### Basic usage

How to use at the most basic level:

- Add to your project using Unity Package Manager.
- Odin code lives in `Assets/.odinInterop/Source/` directory.
- Write your Odin code in `.odin` files.
  - [Odin Overview](https://odin-lang.org/docs/overview/)
- `Ctrl`+`Alt`+`R` to recompile the Odin interop library.

This works even if you're in Play Mode. The bindings will be assigned automatically.

### Runtime Reloading

Runtime reloading means reloading Odin code in a standalone build while the application is running.

Currently, the only supported platform for this is Android ARM64. The rationale is - ARM64 is the most common Android architecture, it's not possible to do runtime reloading on iOS and just use the editor for desktop platforms (might add support for Windows/macOS/Linux later).

To use Runtime Reloading:
- Add `ODININTEROP_RUNTIME_RELOADING` scripting define.
- Build and deploy to an Android device.
  - This includes a build-time version of the Odin code inside the app package itself.
  - If no hot-reloadable version is available, this will be used as a fallback.
- Connect the device to your computer and enable ADB.
- Select `Tools/Odin Interop/Push ARM64 Library to Android Device` menu item.
  - This will push the latest compiled Odin library to the device.
- The application will pick this up and reload the library automatically.
  - ~1.5s delay is to be expected, because the polling for a new file happens every 1.5s.
- Note that some kinds of reloading might require restarting the application.
  - Still better than rebuilding the application though, obviously.

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

### Bindings Generation

This section is all about how to write new bindings.

#### Examples

You can find a lot of examples of generated bindings inside this package.

Check out [Components Bindings](./Scripts/Runtime/Bindings/EngineBindings.Component.cs) or [GameObject Bindings](./Scripts/Runtime/Bindings/EngineBindings.GameObject.cs) for examples of exposing Unity APIs to Odin.

Alternatively, check out [OdinInteropHook Bindings](./Scripts/Runtime/Bindings/OdinInteropHook.cs) for examples of exposing Odin functions to C#.

#### Exposing more C# functions

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

#### Exposing Odin functions

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

#### Supported Interop Types

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
