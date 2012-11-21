using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace Aspects
{
    [Serializable]
    [ProvideAspectRole(StandardRoles.PerformanceInstrumentation)]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.ExceptionHandling)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class PerformanceAttribute : OnMethodBoundaryAspect
    {
        private static readonly Stopwatch Stopwatch = InitializeStopWatch();
        private string methodName;

       public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            if (method == null)
            {
                return;
            }

            this.methodName = method.DeclaringType.FullName + "." + method.Name;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (args == null)
            {
                return;
            }

            args.MethodExecutionTag = Stopwatch.ElapsedMilliseconds;
            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            if (args == null)
            {
                return;
            }

            Logger.Write(string.Format("Method {0} took {1} milliseconds to execute.", methodName, (Stopwatch.ElapsedMilliseconds - (long)args.MethodExecutionTag)));

            base.OnExit(args);
        }

        private static Stopwatch InitializeStopWatch()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            return stopWatch;
        }
    }
}
