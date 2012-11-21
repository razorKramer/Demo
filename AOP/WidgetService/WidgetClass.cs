using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WidgetService
{
    [Aspects.Trace]
    public class WidgetClass
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        
        public string GetDataWithException(int value)
        {
            throw new ArgumentException("An invalid number was provided!");
        }

        public string GetDataWithLongExecution()
        {
            Thread.Sleep(6000);

            return string.Format("You called me!");
        }
    }
}