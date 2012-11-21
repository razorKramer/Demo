using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace Aspects
{
    [Serializable]
    public sealed class CacheAttribute : OnMethodBoundaryAspect
    {
        private string cacheKey;
        private static readonly Dictionary<string, Object> cache = new Dictionary<string, object>();

        public override bool CompileTimeValidate(MethodBase method)
        { 
            // Don't apply to constructors.
            if (method is ConstructorInfo)
            {
                Message.Write(method, SeverityType.Error, "CX0001", "Cannot cache constructors.");
                return false;
            }

            MethodInfo methodInfo = (MethodInfo)method;

            // Don't apply to void methods.
            if (methodInfo.ReturnType.Name == "Void")
            {
                Message.Write(method, SeverityType.Error, "CX0002", "Cannot cache void methods.");
                return false;
            }

            // Does not support out parameters.
            ParameterInfo[] parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].IsOut)
                {
                    Message.Write(method, SeverityType.Error, "CX0003", "Cannot cache methods with out parameters.");
                    return false;
                }
            }

            return true;
        }
        
        // At compile time, initialize the format string that will be
        // used to create the cache keys.
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            cacheKey = method.DeclaringType.FullName + "." + method.Name;
        }

        // Executed at runtime, before the method.
        public override void OnEntry(MethodExecutionArgs args)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Write the list of all arguments.
            for (int i = 0; i < args.Arguments.Count; i++)
            {
                if (i > 0) stringBuilder.Append(":");

                stringBuilder.Append(args.Arguments.GetArgument(i) ?? "null");
            }

            string qualifiedKey = cacheKey + stringBuilder.ToString();

            // Test whether the cache contains the current method call.
            lock (cache)
            {
                object value;

                if (!cache.TryGetValue(qualifiedKey, out value))
                {
                    // If not, we will continue the execution as normally.
                    // We store the key in a state variable to have it in the OnExit method.
                    args.MethodExecutionTag = qualifiedKey;
                }
                else
                {
                    // If it is in cache, we set the cached value as the return value
                    // and we force the method to return immediately.
                    args.ReturnValue = value;
                    args.FlowBehavior = FlowBehavior.Return;
                }
            }
        }

        // Executed at runtime, after the method.
        public override void OnSuccess(MethodExecutionArgs eventArgs)
        {
            // Retrieve the key that has been computed in OnEntry.
            string key = (string)eventArgs.MethodExecutionTag;

            // Put the return value in the cache.
            lock (cache)
            {
                cache[key] = eventArgs.ReturnValue;
            }
        }
    }
}
