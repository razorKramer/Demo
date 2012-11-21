using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using PostSharp.Extensibility;
using WidgetService.Mappers;
using WidgetService.Model;

[assembly: Aspects.DccTrace(AttributeTargetTypes="WidgetService.CustomerResource", AttributeTargetMemberAttributes=MulticastAttributes.Public)]
[assembly: Aspects.Performance(AttributeTargetTypes = "WidgetService.CustomerResource", AttributeTargetMemberAttributes = MulticastAttributes.Public)]
[assembly: Aspects.Performance(AttributeTargetTypes = "WidgetService.Model.WidgetDataLayer", AttributeTargetMemberAttributes = MulticastAttributes.Internal)]
[assembly: Aspects.Cache(AttributeTargetTypes = "WidgetService.Model.WidgetDataLayer", AttributeTargetMemberAttributes = MulticastAttributes.Internal)]
[assembly: Aspects.ExceptionHandling(AttributeTargetTypes = "WidgetService.CustomerResource", AttributeTargetMemberAttributes = MulticastAttributes.Public)]

namespace WidgetService
{
    public class CustomerResource : ICustomerResource
    {
        private WidgetDataLayer widgetDataLayer = new WidgetDataLayer();

        internal WidgetDataLayer WidgetDataLayer
        {
            get { return widgetDataLayer; }
            set { widgetDataLayer = value; }
        }


        public CustomerDto GetCustomer(int customerId)
        {
            return InternalGetCustomer(customerId);
        }


        public IList<CustomerDto> GetCustomers()
        {
            IList<Customer> customerList = WidgetDataLayer.GetCustomers();
            IList<CustomerDto> customerDtoList = new CustomerMapper().MapTo(customerList, new List<CustomerDto>());

            return customerDtoList;
        }

        public bool DeleteCustomer(int customerId)
        {
            return WidgetDataLayer.DeleteCustomer(customerId);
        }

        private CustomerDto InternalGetCustomer(int customerId)
        {
            Customer customer = WidgetDataLayer.GetCustomer(customerId);
            CustomerDto customerDto = new CustomerMapper().MapTo(customer, new CustomerDto());

            return customerDto;
        }
    }
}
