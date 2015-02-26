using UnityEngine;
using System.Collections.Generic;
using CLRSharp;

namespace Ghost.LSharp
{
	public class LSharpEnviroment : CLRSharp_Environment 
	{
		private class Logger : CLRSharp.ICLRSharp_Logger
		{
			public void Log(string str)
			{
				Debug.Log(str);
			}
			
			public void Log_Error(string str)
			{
				Debug.LogError(str);
			}
			
			public void Log_Warning(string str)
			{
				Debug.LogWarning(str);
			}
		}
		
		[System.ThreadStatic]
		private static CLRSharp.ThreadContext s_context = null;
		private static CLRSharp.ThreadContext GetContext(CLRSharp_Environment env)
		{
			if (null == s_context)
			{
				s_context = new CLRSharp.ThreadContext(env);
			}
			return s_context;
		}
		
		public LSharpEnviroment()
			: base(new Logger())
		{
			context = GetContext(this);
		}
		
		public CLRSharp.ThreadContext context {get; private set;}
		
	}
} // namespace Ghost.LSharp
