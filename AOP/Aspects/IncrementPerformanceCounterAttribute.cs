using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace Aspects
{
    [Serializable]
    public class IncrementPerformanceCounterAttribute : PerformanceCounterAttribute
    {
        private long increment = 1;

        public IncrementPerformanceCounterAttribute(string categoryName, string counterName)
            : base(categoryName, counterName)
        {
        }

        /// <summary>
        /// Gets or sets the value of which the performance counter is incremented
        /// before each execution of target method. The default value is 1.
        /// </summary>
        public long Increment
        {
            get { return this.increment; }
            set { this.increment = value; }
        }

        /// <summary>
        /// Method invoked before the execution of the method to which the current
        /// aspect is applied.
        /// </summary>
        /// <param name="args">Unused.</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            this.PerformanceCounter.IncrementBy(this.increment);
            base.OnEntry(args);
        }
    }
}
