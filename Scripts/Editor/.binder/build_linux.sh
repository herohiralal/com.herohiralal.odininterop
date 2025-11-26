#!/bin/bash

set -e

gcc -shared -o ../../../Plugins/Editor/libOdinInteropBinder.so -fPIC Binder.c -std=c11
