using UnityEngine;
using System.Collections.Generic;
using CLRSharp;

namespace Ghost.LSharp
{
	public interface ILSharpClass
	{
		ILSharpClass CreateInstance();
		ILSharpClass CreateInstance(params object[] parameters);

		object CallMethod(string methodName);
		object CallMethod(string methodName, params object[] parameters);

		object CallStaticMethod(string methodName);
		object CallStaticMethod(string methodName, params object[] parameters);
	}
		
	public class LSharpClassScriptMode : ILSharpClass
	{
		private const string CONSTRUCTOR_METHOD_NAME = ".ctor";

		public LSharpEnviroment enviroment {get; private set;}
		public ICLRType classType {get; private set;}
		public object instance{get; private set;}
		
		public LSharpClassScriptMode(LSharpEnviroment env, string classFullName)
		{
			enviroment = env;
			classType = enviroment.GetType(classFullName);
		}

		private LSharpClassScriptMode(){}
		public LSharpClassScriptMode Clone()
		{
			var obj = new LSharpClassScriptMode();
			obj.enviroment = enviroment;
			obj.classType = classType;
			obj.instance = instance;
			return obj;
		}

		// do call method
		private object DoCallMethod(string methodName, object inst)
		{
			var method = classType.GetMethod(methodName, MethodParamList.constEmpty());
			return method.Invoke(enviroment.context, inst, null);
		}
		private object DoCallMethod(string methodName, object inst, params object[] parameters)
		{
			MethodParamList paramTypeList = null;
			if (null != parameters)
			{
				paramTypeList = MethodParamList.Make();
				foreach (var param in parameters)
				{
					paramTypeList.Add(enviroment.GetType(param.GetType()));
				}
			}
			else
			{
				paramTypeList = MethodParamList.constEmpty();
			}
			var method = classType.GetMethod(methodName, paramTypeList);
			return method.Invoke(enviroment.context, inst, parameters);
		}

		// call instance method
		public object CallMethod(string methodName)
		{
			return DoCallMethod(methodName, instance);
		}
		public object CallMethod(string methodName, params object[] parameters)
		{
			return DoCallMethod(methodName, instance, parameters);
		}

		// call static mothd
		public object CallStaticMethod(string methodName)
		{
			return DoCallMethod(methodName, null);
		}
		public object CallStaticMethod(string methodName, params object[] parameters)
		{
			return DoCallMethod(methodName, null, parameters);
		}

		// create instance
		public ILSharpClass CreateInstance()
		{
			var obj = Clone();
			obj.instance = CallStaticMethod(CONSTRUCTOR_METHOD_NAME);
			return obj;
		}
		public ILSharpClass CreateInstance(params object[] parameters)
		{
			var obj = Clone();
			obj.instance = CallStaticMethod(CONSTRUCTOR_METHOD_NAME, parameters);
			return obj;
		}
		
	}

	public class LSharpClassDebugMode : ILSharpClass
	{
		public System.Type classType{get; private set;}
		public object instance{get; private set;}
		
		public LSharpClassDebugMode(string classFullName)
		{
			classType = System.Type.GetType(classFullName);
		}
		
		private LSharpClassDebugMode(){}
		public LSharpClassDebugMode Clone()
		{
			var obj = new LSharpClassDebugMode();
			obj.classType = classType;
			obj.instance = instance;
			return obj;
		}

		// do call method
		private object DoCallMethod(string methodName, object inst)
		{
			var method = classType.GetMethod(methodName);
			return method.Invoke(inst, null);
		}
		private object DoCallMethod(string methodName, object inst, params object[] parameters)
		{
			System.Type[] paramTypes = null;
			if (null != parameters)
			{
				paramTypes = new System.Type[parameters.Length];
				for (int i = 0; i < paramTypes.Length; ++i)
				{
					paramTypes[i] = parameters[i].GetType();
				}
			}
			var method = classType.GetMethod(methodName, paramTypes);
			return method.Invoke(inst, parameters);
		}

		// call instance method
		public object CallMethod(string methodName)
		{
			return DoCallMethod(methodName, instance);
		}
		public object CallMethod(string methodName, params object[] parameters)
		{
			return DoCallMethod(methodName, instance, parameters);
		}

		// call static method
		public object CallStaticMethod(string methodName)
		{
			return DoCallMethod(methodName, null);
		}
		public object CallStaticMethod(string methodName, params object[] parameters)
		{
			return DoCallMethod(methodName, null, parameters);
		}

		// create instance
		public ILSharpClass CreateInstance()
		{
			var obj = Clone();
			obj.instance = System.Activator.CreateInstance(classType);
			return obj;
		}
		public ILSharpClass CreateInstance(params object[] parameters)
		{
			var obj = Clone();
			obj.instance = System.Activator.CreateInstance(classType, parameters);
			return obj;
		}
		
	}

} // namespace Ghost.LSharp
