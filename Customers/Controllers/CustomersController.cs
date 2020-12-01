using System.Collections.Generic;
using Customers.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Customers.Models;

namespace Customers.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return _customerRepository.GetCustomers();
        }

        [HttpGet("{id}")]
        public Customer Get(int id)
        {
            return _customerRepository.GetCustomerById(id);
        }

        [HttpPost]
        public void Post([FromBody] Customer customer)
        {
            _customerRepository.AddCustomer(customer);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Customer customer)
        {
            _customerRepository.UpdateCustomer(id, customer);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _customerRepository.DeleteCustomer(id);
        }
    }
}
