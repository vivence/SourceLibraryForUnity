using UnityEngine;
using System.Collections.Generic;

public class LSharpScript {

	public LSharpScript()
	{
		Debug.Log("LSharpScript.constructor");
	}

	public static void StaticMethod()
	{
		Debug.Log("LSharpScript.StaticMethod");
	}

	public void Method(int p1, string p2)
	{
		Debug.Log(string.Format("LSharpScript.Method({0}, {1})", p1, p2));
	}

	public void CallAPI_Test()
	{
		LSharpAPI.API_Test();
	}

}
