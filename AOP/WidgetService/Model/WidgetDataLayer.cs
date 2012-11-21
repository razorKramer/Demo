using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WidgetService.Model
{
    internal class WidgetDataLayer
    {
        internal Customer GetCustomer(int customerId)
        {
            // Simulate latency
            Thread.Sleep(2000);
            
            return new Customer { Id = customerId, FirstName = "Peter", LastName = "Smith" };
        }

        internal IList<Customer> GetCustomers()
        {
            // Simulate latency
            Thread.Sleep(8000);

            return new List<Customer> {
                new Customer { Id = 24, FirstName = "Peter", LastName = "Smith" },
                new Customer { Id = 46, FirstName = "Frank", LastName = "Zetta" },
                new Customer { Id = 98, FirstName = "Patty", LastName = "Lock" },
                new Customer { Id = 4, FirstName = "Fran", LastName = "San" },
                new Customer { Id = 10, FirstName = "Lucy", LastName = "Goose" }
            };
        }

        internal bool DeleteCustomer(int customerId)
        {
            throw new ApplicationException("The super secret password 'BetYouCant' can not be used since the secret user id of 'Ted' was not used.");
        }
    }
}