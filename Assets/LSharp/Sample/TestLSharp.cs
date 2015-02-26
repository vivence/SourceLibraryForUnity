
#define LS_SCRIPT_MODE

using UnityEngine;
using System.Collections.Generic;
using Ghost.LSharp;
#if LS_SCRIPT_MODE
using CLRSharp;
#endif

public class TestLSharp : MonoBehaviour {
	
	private ILSharpClass CreateTestCLass()
	{
#if LS_SCRIPT_MODE
		TextAsset dll = Resources.Load("LSharpScript.dll") as TextAsset;
		TextAsset mdb = Resources.Load("LSharpScript.mdb") as TextAsset;
		System.IO.MemoryStream msDll = new System.IO.MemoryStream(dll.bytes);
		System.IO.MemoryStream msMdb = new System.IO.MemoryStream(mdb.bytes);

		var enviroment = new LSharpEnviroment();
		enviroment.LoadModule(msDll, msMdb, new Mono.Cecil.Mdb.MdbReaderProvider());
		return new LSharpClassScriptMode(enviroment, "LSharpScript");
#else
		return new LSharpClassDebugMode("LSharpScript");
#endif
	}
	
	// Use this for initialization
	void Start () {
		var testClass = CreateTestCLass();
		testClass.CallStaticMethod("StaticMethod");

		var testInstance = testClass.CreateInstance();
		testInstance.CallMethod("Method", 1, "abc");
		testInstance.CallMethod("CallAPI_Test");
	}
	
	//	// Update is called once per frame
	//	void Update () {
	//	
	//	}
}
