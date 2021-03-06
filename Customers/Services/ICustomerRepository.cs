﻿using System.Collections.Generic;
using Models.Customers.Models;

namespace Customers.Services
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetCustomers();

        Customer GetCustomerById(int id);

        void AddCustomer(Customer customer);

        void UpdateCustomer(int id, Customer customer);

        void DeleteCustomer(int id);
    }
}
