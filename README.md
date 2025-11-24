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

- Add to your project.
- `Ctrl`+`Alt`+`R` to recompile the Odin interop library.
- `Tools/Odin Interop/Generate Interop Code` to regenerate bindings.

## To-Do

Below are the currently supported platforms:

- [ ] Editor Experience
  - [x] Hot reloading outside PlayMode (with rebinding)
  - [x] Hot reloading inside PlayMode (with dynamic rebinding)
  - [x] Unity domain-reload compatibility
  - [ ] BindGen* (current version supports a manual `MenuItem` based trigger; would like to replace it with Roslyn Source Generators)
- [ ] Platform Support
  - [x] Windows
  - [ ] macOS
  - [x] Linux
  - [ ] iOS
  - [x] Android
- [ ] Bindings
  - [ ] GameObject
  - [ ] Transform
  - [ ] Rendering
  - [ ] VFX
  - [ ] Audio
  - [ ] Physics
  - [ ] Input*
  - [ ] Animations
  - [ ] Timeline

> \* - maybe