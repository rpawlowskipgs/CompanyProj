using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customers.Models;

namespace Basket.Services
{
    public interface ICustomerDetailsRepository
    {
        public Task<Customer> GetCustomer(int customerId);
    }
}
