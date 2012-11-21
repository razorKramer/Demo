using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using PostSharp.Aspects;

namespace Aspects
{
    [Serializable]
    public class TraceAttribute : OnMethodBoundaryAspect
    {
        private string methodName;

        public TraceAttribute(LayerType traceType)
        {
        }

        public TraceAttribute()
        {
        }

        public override void CompileTimeInitialize(System.Reflection.MethodBase method, AspectInfo aspectInfo)
        {
            methodName = method.DeclaringType.FullName + "." + method.Name;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Write the list of all arguments.
            for (int i = 0; i < args.Arguments.Count; i++)
            {
                if (i > 0) stringBuilder.Append(", ");

                stringBuilder.Append(args.Arguments.GetArgument(i) ?? "null");
            }

            if (args.Arguments.Count == 0)
            {
                Logger.Write(string.Format("Entering {0}.", methodName), "Trace");
            }
            else
            {
                Logger.Write(string.Format("Entering {0} with the following argument values: {1}.", methodName, stringBuilder), "Trace");
            }
        }
                
        public override void OnExit(MethodExecutionArgs args)
        {
            Logger.Write(string.Format("Leaving {0}.", methodName), "Trace");
        }

        public enum LayerType
        {
            ServiceLayer,
            BusinessLayer
        }
    }
}
