using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basket.Models;
using Basket.Services;
using CompanyProducts.Models;
using Customers.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace BasketTest
{
    public class BasketServiceTests
    {
        [Fact]
        public void AddProductsToBasket_ShouldAddProductsToBasketWhenCurrentBasketIsNull()
        {

            // Arrange 
            var customerId = 1;
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId)).Returns<BasketWithGoods>(null);

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());
  
            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProduct(customerId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            BasketWithGoods currentBasket = new BasketWithGoods
            {
                CustomerId = customerId,
                ProductIds = new List<ProductsInBasket>()
            };

            // Act

            basketService.AddProductsToBasket(1, 1, 1);


            // Assert
            basketMock.Verify(x => x.AddToBasket(It.Is<BasketWithGoods>(x => x.CustomerId == currentBasket.CustomerId)));
            basketMock.Verify(x => x.AddToBasket(It.Is<BasketWithGoods>(x => x.ProductIds != null)));
        }

        [Fact]
        public void AddProductsToBasket_ShouldOnlyUpdateQuantityOfProducts()
        {
            // Arrange 
            var customerId = 1;
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId)).Returns(new BasketWithGoods
            {
                CustomerId = customerId,
                ProductIds = new List<ProductsInBasket>
                {
                    new ProductsInBasket
                    {
                        ProductId = 1,
                        Quantity = 1
                    }
                }
            });

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProduct(customerId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            basketService.AddProductsToBasket(1, 1, 3);

            // Assert
            basketMock.Verify(x => x.UpdateBasket(It.Is<List<ProductsInBasket>>(x =>x[0].Quantity == 4), customerId));
        }

        [Fact]
        public void AddProductsToBasket_ShouldUpdateProductIdAndQuantityOfProducts()
        {
            // Arrange 
            var customerId = 1;
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId)).Returns(new BasketWithGoods
            {
                CustomerId = customerId,
                ProductIds = new List<ProductsInBasket>
                {
                    new ProductsInBasket
                    {
                        ProductId = 1,
                        Quantity = 1
                    }
                }
            });

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProduct(customerId)).ReturnsAsync(new Product());
            
            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            basketService.AddProductsToBasket(1, 2, 3);

            // Assert
            basketMock.Verify(x => x.UpdateBasket(It.Is<List<ProductsInBasket>>(x => x[1].Quantity == 3), customerId));
            basketMock.Verify(x => x.UpdateBasket(It.Is<List<ProductsInBasket>>(x => x[1].ProductId == 2), customerId));
        }

        [Fact]
        public async Task GetCurrentBasketProducts_ShouldReturnBasketResponse()
        {
            // Arrange
            var customerId = 1;
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId)).Returns(new BasketWithGoods
            {
                CustomerId = customerId,
                ProductIds = new List<ProductsInBasket>
                {
                    new ProductsInBasket
                    {
                        ProductId = 1,
                        Quantity = 1
                    }
                }
            });

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            var customer = new Customer
            {
                FirstName = "Jan",
                LastName = "Nowak",
                Age = 23,
                CustomerId = 1
            };
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer
            {
                FirstName = "Jan",
                LastName = "Nowak",
                Age = 23,
                CustomerId = 1
            });

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProduct(customerId)).ReturnsAsync(new Product
            {
                Name = "Abecadlo",
                Price = 200,
                ProductId = 1
            });

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);
            
            BasketResponse basketResponse = new BasketResponse();

            // Act
            var expected = await basketService.GetCurrentBasketProducts(customerId);

            // Assert
            expected.Customer.Should().BeEquivalentTo(customer);
            expected.Product.Should().BeEquivalentTo(new List<ProductResponse>
            {
                new ProductResponse
                    {Quantity = 1, Details = new Product {Name = "Abecadlo", Price = 200, ProductId = 1}}
            });
        }

        [Fact]
        public void UpdateQuantityOfProductsInBasket_ShouldOnlyUpdateQuantityOfProductsInBasket()
        {
            var customerId = 1;
            var basketMock = new Mock<IBasketRepository>();
            var basketWithGoods = new BasketWithGoods
            {
                CustomerId = customerId,
                ProductIds = new List<ProductsInBasket>
                {
                    new ProductsInBasket
                    {
                        ProductId = 1,
                        Quantity = 1
                    }
                }
            };
            basketMock.Setup(x => x.GetBasket(customerId)).Returns(basketWithGoods);

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProduct(customerId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            basketService.UpdateQuantityOfProductsInBasket(1, 1, 3);

            // Assert
            basketWithGoods.ProductIds[0].Quantity.Should().Be(3);
        }

        [Fact]
        public void UpdateQuantityOfProductsInBasket_ShouldUpdateProductsAndQuantityOfProductsInBasket()
        {
            var customerId = 1;
            var basketMock = new Mock<IBasketRepository>();
            var basketWithGoods = new BasketWithGoods
            {
                CustomerId = customerId,
                ProductIds = new List<ProductsInBasket>
                {
                    new ProductsInBasket
                    {
                        ProductId = 1,
                        Quantity = 1
                    }
                }
            };
            basketMock.Setup(x => x.GetBasket(customerId)).Returns(basketWithGoods);

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProduct(customerId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            basketService.UpdateQuantityOfProductsInBasket(1, 2, 4);

            // Assert
            basketWithGoods.ProductIds.Should().HaveCount(2);
            basketWithGoods.ProductIds[1].Quantity.Should().Be(4);
            basketWithGoods.ProductIds[1].ProductId.Should().Be(2);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public void RemoveFromBasket_ShouldRemoveProductFromBasket(int productId, bool result)
        {
            // Arrange
            var customerId = 1;

            var basketMock = new Mock<IBasketRepository>();
            var basketWithGoods = new BasketWithGoods
            {
                CustomerId = customerId,
                ProductIds = new List<ProductsInBasket>
                {
                    new ProductsInBasket
                    {
                        ProductId = 1,
                        Quantity = 1
                    }
                }
            };
            basketMock.Setup(x => x.GetBasket(customerId)).Returns(basketWithGoods);

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProduct(customerId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            var expected = basketService.RemoveFromBasket(customerId, productId);

            // Assert
            expected.Should().Be(result);
        }
    }
}
