using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WidgetService.Model;

namespace WidgetService.Mappers
{
    public class CustomerMapper
    {
        internal IList<CustomerDto> MapTo(IList<Customer> source, IList<CustomerDto> target)
        {
            foreach (Customer customer in source)
            {
                target.Add(MapTo(customer, new CustomerDto()));
            }

            return target;
        }

        internal CustomerDto MapTo(Customer source, CustomerDto target)
        {
            target.Id = source.Id;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;

            return target;
        }
    }
}