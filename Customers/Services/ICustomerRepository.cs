using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customers.Models;

namespace Customers.Services
{
    public interface ICustomerRepository
    {
        List<Customer> GetCustomers();

        Customer GetCustomerById(int id);

        void AddCustomer(Customer customer);

        void UpdateCustomer(int id, Customer customer);

        void DeleteCustomer(int id);
    }
}
