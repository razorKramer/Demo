using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using PostSharp.Aspects;

namespace Aspects
{
    [Serializable]
    public class DccTraceAttribute : OnMethodBoundaryAspect
    {
        string methodName = string.Empty;

        public override void RuntimeInitialize(System.Reflection.MethodBase method)
        {
            methodName = method.DeclaringType.FullName + "." + method.Name;
            base.RuntimeInitialize(method);
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
            Logger.Write("Leaving " + methodName + " method!!");

            base.OnExit(args);
        }
    }
}
