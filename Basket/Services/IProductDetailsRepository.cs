using System.Threading.Tasks;
using Models.Products.Models;

namespace Basket.Services
{
    public interface IProductDetailsRepository
    {
        public Task<Product> GetProductAsync(int productId);
    }
}
