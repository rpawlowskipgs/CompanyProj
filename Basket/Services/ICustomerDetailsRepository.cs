using System.Threading.Tasks;
using Models.Customers.Models;

namespace Basket.Services
{
    public interface ICustomerDetailsRepository
    {
        public Task<Customer> GetCustomer(int customerId);
    }
}
