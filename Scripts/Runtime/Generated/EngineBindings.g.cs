using OdinInterop;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace OdinInterop
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

		[InitializeOnLoadMethod]
		private static void odntrop_EditorInit()
		{
			OdinCompilerUtils.onHotReload += odntrop_OnHotReload;
			if (OdinCompilerUtils.initialisedAfterDomainReload) odntrop_OnHotReload(OdinCompilerUtils.libraryHandle);
		}

		private static void odntrop_OnHotReload(IntPtr libraryHandle)
		{

		}

#else

#endif
	}
}
