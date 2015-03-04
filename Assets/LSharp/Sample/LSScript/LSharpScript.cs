using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Ghost.LSharp.Script;

public class LSharpScript : LSharpMonoBehavior {

	public LSharpScript(GameObject go, string arg)
		: base(go)
	{
		Debug.Log(string.Format("LSharpScript.constructor({0})", arg));

		var obj = GameObject.FindWithTag("test_button");
		if (null != obj)
		{
			var button = obj.GetComponent<Button>();
			if (null != button)
			{
				button.onClick.AddListener(delegate {
					Debug.Log("test_button on click");
					this.OnTestButton();
				});
			}
			else
			{
				Debug.Log("test_button is not a button!");
			}
		}
		else
		{
			Debug.Log("test_button not found!");
		}

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

	private void OnTestButton()
	{
		Debug.Log("LSharpScript.OnTestButton");
	}

}
