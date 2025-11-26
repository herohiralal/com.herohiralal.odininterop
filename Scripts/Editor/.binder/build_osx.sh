#!/bin/bash

set -e

clang -std=c11 -shared -fPIC -target arm64-apple-macos Binder.c -o libOdinInteropBinder_arm64.dylib
clang -std=c11 -shared -fPIC -target x86_64-apple-macos Binder.c -o libOdinInteropBinder_x64.dylib

lipo -create -output ../../../Plugins/Editor/libOdinInteropBinder.dylib libOdinInteropBinder_arm64.dylib libOdinInteropBinder_x64.dylib
