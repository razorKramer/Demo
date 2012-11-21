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
    public sealed class ExceptionHandlingAttribute : OnMethodBoundaryAspect
    {
        private string methodName;

        public override void CompileTimeInitialize(System.Reflection.MethodBase method, AspectInfo aspectInfo)
        {
            methodName = method.DeclaringType.FullName + "." + method.Name;
        }

        public override void OnException(MethodExecutionArgs args)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(this.methodName);
            stringBuilder.Append('(');

            object instance = args.Instance;

            if (instance != null)
            {
                stringBuilder.Append(instance);
            }
                        
            // Write the exception message.
            stringBuilder.AppendFormat(": Exception ");
            stringBuilder.Append(args.Exception.GetType().Name);
            stringBuilder.Append(": ");
            stringBuilder.Append(args.Exception.Message);

            Logger.Write(string.Format("An error occured --  {0}", stringBuilder), "Trace", 1);

            throw new BusinessException("Sorry for the inconvienence, please try again later.");
        }
    }
}
