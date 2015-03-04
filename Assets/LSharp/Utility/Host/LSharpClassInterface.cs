using UnityEngine;
using System.Collections.Generic;
using CLRSharp;

namespace Ghost.LSharp.Host
{
	public interface ILSharpClass
	{
		ILSharpClass CreateInstance();
		ILSharpClass CreateInstance(params object[] args);

		object CallMethod(string methodName);
		object CallMethod(string methodName, params object[] args);

		object CallStaticMethod(string methodName);
		object CallStaticMethod(string methodName, params object[] args);
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
		private object DoCallMethod(string methodName, object inst, object[] args)
		{
			MethodParamList paramTypeList = null;
			if (null != args)
			{
				paramTypeList = MethodParamList.Make();
				foreach (var param in args)
				{
					paramTypeList.Add(enviroment.GetType(param.GetType()));
				}
			}
			else
			{
				paramTypeList = MethodParamList.constEmpty();
			}
			var method = classType.GetMethod(methodName, paramTypeList);
			return method.Invoke(enviroment.context, inst, args);
		}

		// call instance method
		public object CallMethod(string methodName)
		{
			return DoCallMethod(methodName, instance);
		}
		public object CallMethod(string methodName, params object[] args)
		{
			return CallMethodWithArgs(methodName, args);
		}
		public object CallMethodWithArgs(string methodName, object[] args)
		{
			return DoCallMethod(methodName, instance, args);
		}

		// call static mothd
		public object CallStaticMethod(string methodName)
		{
			return DoCallMethod(methodName, null);
		}
		public object CallStaticMethod(string methodName, params object[] args)
		{
			return CallStaticMethodWithArgs(methodName, args);
		}
		public object CallStaticMethodWithArgs(string methodName, object[] args)
		{
			return DoCallMethod(methodName, null, args);
		}

		// create instance
		public ILSharpClass CreateInstance()
		{
			var obj = Clone();
			obj.instance = CallStaticMethod(CONSTRUCTOR_METHOD_NAME);
			return obj;
		}
		public ILSharpClass CreateInstance(params object[] args)
		{
			return CreateInstanceWithArgs(args);
		}
		public ILSharpClass CreateInstanceWithArgs(object[] args)
		{
			var obj = Clone();
			obj.instance = CallStaticMethod(CONSTRUCTOR_METHOD_NAME, args);
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
		private object DoCallMethod(string methodName, object inst, object[] args)
		{
			System.Type[] paramTypes = null;
			if (null != args)
			{
				paramTypes = new System.Type[args.Length];
				for (int i = 0; i < paramTypes.Length; ++i)
				{
					paramTypes[i] = args[i].GetType();
				}
			}
			var method = classType.GetMethod(methodName, paramTypes);
			return method.Invoke(inst, args);
		}

		// call instance method
		public object CallMethod(string methodName)
		{
			return DoCallMethod(methodName, instance);
		}
		public object CallMethod(string methodName, params object[] args)
		{
			return CallMethodWithArgs(methodName, args);
		}
		public object CallMethodWithArgs(string methodName, object[] args)
		{
			return DoCallMethod(methodName, instance, args);
		}

		// call static method
		public object CallStaticMethod(string methodName)
		{
			return DoCallMethod(methodName, null);
		}
		public object CallStaticMethod(string methodName, params object[] args)
		{
			return CallStaticMethodWithArgs(methodName, args);
		}
		public object CallStaticMethodWithArgs(string methodName, object[] args)
		{
			return DoCallMethod(methodName, null, args);
		}

		// create instance
		public ILSharpClass CreateInstance()
		{
			var obj = Clone();
			obj.instance = System.Activator.CreateInstance(classType);
			return obj;
		}
		public ILSharpClass CreateInstance(params object[] args)
		{
			return CreateInstanceWithArgs(args);
		}
		public ILSharpClass CreateInstanceWithArgs(object[] args)
		{
			var obj = Clone();
			obj.instance = System.Activator.CreateInstance(classType, args, new object[0]);
			return obj;
		}
		
	}

} // namespace Ghost.LSharp.Host
