
#define LS_SCRIPT_MODE

using UnityEngine;
using System.Collections.Generic;
using Ghost.LSharp;
#if LS_SCRIPT_MODE
using System.IO;
using Ghost.Config;
using Ghost.Utils;
using CLRSharp;
#endif

public class TestLSharp : MonoBehaviour {

	private ILSharpClass testClass{get;set;}

#if LS_SCRIPT_MODE
	private IEnumerator<WWW> LoadTestClass()
	{
		var rootPath = StringUtils.ConnectToString(PathConfig.LOCAL_URL_PREFIX, Application.streamingAssetsPath);
		var www = new WWW(Path.Combine(rootPath, "LSharpScript.dll.bytes"));
		yield return www;

		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.LogError("www.error: "+www.error);
			yield break;
		}

		var msDll = new System.IO.MemoryStream(www.bytes);

		www = new WWW(Path.Combine(rootPath, "LSharpScript.mdb.bytes"));
		yield return www;

		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.LogError("www.error: "+www.error);
			yield break;
		}

		var msMdb = new System.IO.MemoryStream(www.bytes);

		var enviroment = new LSharpEnviroment();
		enviroment.LoadModule(msDll, msMdb, new Mono.Cecil.Mdb.MdbReaderProvider());
		testClass = new LSharpClassScriptMode(enviroment, "LSharpScript");

		Debug.Log("[ghost]load ok");
	}
#endif

	private ILSharpClass CreateTestCLass()
	{
#if LS_SCRIPT_MODE
//		StartCoroutine(LoadTestClass());
//		return null;
		var dll = Resources.Load("LSharpScript.dll") as TextAsset;
		var mdb = Resources.Load("LSharpScript.mdb") as TextAsset;
		var msDll = new System.IO.MemoryStream(dll.bytes);
		var msMdb = new System.IO.MemoryStream(mdb.bytes);
		var enviroment = new LSharpEnviroment();
		enviroment.LoadModule(msDll, msMdb, new Mono.Cecil.Mdb.MdbReaderProvider());
		return new LSharpClassScriptMode(enviroment, "LSharpScript");
#else
		return new LSharpClassDebugMode("LSharpScript");
#endif
	}
	
	// Use this for initialization
	void Start () {
		testClass = CreateTestCLass();
	}
	
	// Update is called once per frame
	void Update () {
		if (null != testClass)
		{
			testClass.CallStaticMethod("StaticMethod");
			
			var testInstance = testClass.CreateInstance();
			testInstance.CallMethod("Method", 1, "abc");
			testInstance.CallMethod("CallAPI_Test");
			testClass = null;
		}
	}

}
