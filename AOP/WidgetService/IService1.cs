using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WidgetService
{
    [ServiceContract]
    public interface ICustomerResource
    {
        [OperationContract]
        CustomerDto GetCustomer(int customerId);

        [OperationContract]
        IList<CustomerDto> GetCustomers();

        [OperationContract]
        bool DeleteCustomer(int customerId);
    }
}
