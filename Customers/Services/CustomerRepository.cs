using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Models.Customers.Models;

namespace Customers.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private static object _customersLock = new object();
        private ConcurrentBag<Customer> _customers;

        public CustomerRepository()
        {
            // To be deleted soon :)
            _customers = new ConcurrentBag<Customer>
            {
                new Customer { CustomerId = 1, FirstName = "John", LastName = "Kowalskey", Age = 59 },
                new Customer { CustomerId = 2, FirstName = "Grażyna", LastName = "Nowak", Age = 21 },
                new Customer { CustomerId = 3, FirstName = "Janusz", LastName = "Andruszkiewicz", Age = 32 }
            };
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _customers;
        }

        public Customer GetCustomerById(int id)
        {
            return _customers.FirstOrDefault(c => c.CustomerId == id);
        }

        public void AddCustomer(Customer customer)
        {
            customer.CustomerId = _customers.Select(c => c.CustomerId).Max() + 1;
            _customers.Add(customer);
        }

        public void UpdateCustomer(int id, Customer customer)
        {
            var customerToUpdate = _customers.FirstOrDefault(c => c.CustomerId == id);

            if (customerToUpdate != null)
            {
                customerToUpdate.FirstName = customer.FirstName;
                customerToUpdate.LastName = customer.LastName;
                customerToUpdate.Age = customer.Age;
            }
        }

        public void DeleteCustomer(int id)
        {
            var customerToDelete = _customers.FirstOrDefault(c => c.CustomerId == id);

            if (customerToDelete != null)
                _customers.TryTake(out customerToDelete);
        }
    }
}
