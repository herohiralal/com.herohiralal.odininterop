package src

UNITY_EDITOR :: #config(UNITY_EDITOR, false)
UNITY_EDITOR_WIN :: #config(UNITY_EDITOR_WIN, false)
UNITY_EDITOR_OSX :: #config(UNITY_EDITOR_OSX, false)
UNITY_EDITOR_LINUX :: #config(UNITY_EDITOR_LINUX, false)

UNITY_STANDALONE :: #config(UNITY_STANDALONE, false)
UNITY_STANDALONE_WIN :: #config(UNITY_STANDALONE_WIN, false)
UNITY_STANDALONE_OSX :: #config(UNITY_STANDALONE_OSX, false)
UNITY_STANDALONE_LINUX :: #config(UNITY_STANDALONE_LINUX, false)

UNITY_IOS :: #config(UNITY_IOS, false)
UNITY_ANDROID :: #config(UNITY_ANDROID, false)

#assert((1 when UNITY_STANDALONE else 0) + (1 when UNITY_IOS else 0) + (1 when UNITY_ANDROID else 0) == 1)
#assert((1 when UNITY_EDITOR_WIN else 0) + (1 when UNITY_EDITOR_OSX else 0) + (1 when UNITY_EDITOR_LINUX else 0) == (1 when UNITY_EDITOR else 0))
#assert((1 when UNITY_STANDALONE_WIN else 0) + (1 when UNITY_STANDALONE_OSX else 0) + (1 when UNITY_STANDALONE_LINUX else 0) == (1 when UNITY_STANDALONE else 0))
