using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace Aspects
{
    [Serializable]
    public abstract class PerformanceCounterAttribute : OnMethodBoundaryAspect
    {
        // Serialized fields: set at build time, used at run time.
        private readonly string categoryName;
        private readonly string counterName;

        // Not serialized because used at rutime only.
        [NonSerialized]
        private PerformanceCounter performanceCounter;

        protected PerformanceCounterAttribute(string categoryName, string counterName)
        {
            this.categoryName = categoryName;
            this.counterName = counterName;
        }

        /// <summary>
        /// Gets the performance counter (can be invoked at runtime).
        /// </summary>
        protected PerformanceCounter PerformanceCounter
        {
            get { return this.performanceCounter; }
        }

        /// <summary>
        /// Method executed at run time just after the aspect is deserialized.
        /// </summary>
        /// <param name="method">>Method to which the current aspect instance 
        /// has been applied.</param>
        public override void RuntimeInitialize(MethodBase method)
        {
            try
            {
                //SetupCategory();

                this.performanceCounter = new PerformanceCounter(this.categoryName, this.counterName, false);
            }
            catch (Exception ex)
            {
                bool test = true;
            }
        }

        private bool SetupCategory()
        {
          // PerformanceCounterCategory.Delete("DCC2012");

            if (!PerformanceCounterCategory.Exists(this.categoryName))
            {
                CounterCreationDataCollection counterDataCollection = new CounterCreationDataCollection();

                // Add the counter.
                CounterCreationData averageCount64 = new CounterCreationData();
                averageCount64.CounterType = PerformanceCounterType.NumberOfItems32;
                averageCount64.CounterName = "GetData Call Count";
                counterDataCollection.Add(averageCount64);

                CounterCreationData averageCount641 = new CounterCreationData();
                averageCount641.CounterType = PerformanceCounterType.CounterTimer;
                averageCount641.CounterName = "GetData Execution Time";
                counterDataCollection.Add(averageCount641);

                // Create the category.
                PerformanceCounterCategory.Create("DCC2012", "Desert Code Camp.",
                    PerformanceCounterCategoryType.SingleInstance, counterDataCollection);

                return (true);
            }
            else
            {
                Console.WriteLine("Category exists - AverageCounter64SampleCategory");
                return (false);
            }
        }

    }
}
