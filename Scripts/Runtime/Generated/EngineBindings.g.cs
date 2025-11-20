// toimport found = True
using OdinInterop;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;

namespace OdinInterop.Editor
{
	public static partial class EngineBindings
	{
		private const string k_OdinInteropDllName = 
#if UNITY_IOS && !UNITY_EDITOR
			"__Internal";
#else
			"odininteropcode";
#endif
#if UNITY_EDITOR
#else
#endif
	}
}
