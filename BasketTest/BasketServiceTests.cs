using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.Models;
using Basket.Services;
using FluentAssertions;
using Models.Customers.Models;
using Models.Products.Models;
using Moq;
using Xunit;

namespace BasketTest
{
    public class BasketServiceTests
    {
        [Fact]
        public async Task AddProductsToBasket_ShouldAddProductsToBasketWhenCurrentBasketIsNull()
        {
            // Arrange 
            int customerId = 1;
            int productId = 1;
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId)).Returns<BasketWithGoods>(null);

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());
  
            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            BasketWithGoods currentBasket = new BasketWithGoods
            {
                CustomerId = customerId,
                ProductIds = new List<ProductsInBasket>()
            };

            // Act
            await basketService.AddProductsToBasket(1, 1, 1);

            // Assert
            basketMock.Verify(x => x.AddToBasket(It.Is<BasketWithGoods>(x => x.CustomerId == currentBasket.CustomerId)));
            basketMock.Verify(x => x.AddToBasket(It.Is<BasketWithGoods>(x => x.ProductIds != null)));
        }

        [Theory]
        [InlineData(0, Status.BadRequest)]
        [InlineData(-1, Status.BadRequest)]
        public async Task AddProductsToBasket_ShouldReturnBadRequestWhenQuantityIsLessThenOrEqualZero(int quantity, Status result)
        {
            // Arrange 
            int customerId = 1;
            int productId = 1;
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId)).Returns<BasketWithGoods>(null);

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            var expected = await basketService.AddProductsToBasket(customerId, productId, quantity);

            // Assert
            expected.Status.Should().Be(result);
        }

        [Fact]
        public async Task AddProductsToBasket_ShouldReturnNotFoundWhenCustomerIsNotFound()
        {
            // Arrange 
            int quantity = 1;
            int customerId = 1;
            int productId = 1;
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId)).Returns<BasketWithGoods>(null);

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(default(Customer));

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);
            
            // Act
            var expected = await basketService.AddProductsToBasket(customerId, productId, quantity);

            // Assert
            expected.Status.Should().Be(Status.NotFound);
        }

        [Fact]
        public async Task AddProductsToBasket_ShouldReturnOkWhenProductIsNull()
        {
            // Arrange 
            int quantity = 1;
            int customerId = 1;
            int productId = 1;
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId)).Returns<BasketWithGoods>(null);

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(default (Product));

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            var expected = await basketService.AddProductsToBasket(customerId, productId, quantity);

            // Assert
            expected.Status.Should().Be(Status.BadRequest);
        }

        [Fact]
        public async Task AddProductsToBasket_ShouldOnlyUpdateQuantityOfProducts()
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
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(customerId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            await basketService.AddProductsToBasket(1, 1, 3);

            // Assert
            basketMock.Verify(x => x.UpdateBasket(It.Is<List<ProductsInBasket>>(x =>x[0].Quantity == 4), customerId));
        }

        [Fact]
        public async Task AddProductsToBasket_ShouldReturnOk()
        {
            // Arrange 
            int quantity = 1;
            int customerId = 1;
            int productId = 1;
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId)).Returns<BasketWithGoods>(null);

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            var expected = await basketService.AddProductsToBasket(customerId, productId, quantity);

            // Assert
            expected.Status.Should().Be(Status.Ok);
        }

        [Fact]
        public async Task AddProductsToBasket_ShouldUpdateProductIdAndQuantityOfProducts()
        {
            // Arrange 
            int customerId = 1;
            int productId = 2;
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
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(new Product());
            
            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            await basketService.AddProductsToBasket(1, 2, 3);

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
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(customerId)).ReturnsAsync(new Product
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
            expected.Value.Customer.Should().BeEquivalentTo(customer);
            expected.Value.Product.Should().BeEquivalentTo(new List<ProductResponse>
            {
                new ProductResponse
                    {Quantity = 1, Details = new Product {Name = "Abecadlo", Price = 200, ProductId = 1}}
            });
        }

        [Fact]
        public async Task GetCurrentBasketProducts_ShouldReturnStatusNotFoundWhenBasketIsNull()
        {
            // Arrange
            var customerId = 1;
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId));

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
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(customerId)).ReturnsAsync(new Product
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
            expected.Status.Should().Be(Status.NotFound);
        }

        [Theory]
        [InlineData(0, Status.BadRequest)]
        [InlineData(-1, Status.BadRequest)]
        public async Task UpdateQuantityOfProductsInBasket_ShouldReturnBadRequestWhenQuantityIsLessThenOrEqualZero(int quantity, Status result)
        {

            // Arrange
            int customerId = 1;
            int productId = 1;
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
            
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId)).Returns(basketWithGoods);

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            var expected = await basketService.UpdateQuantityOfProductsInBasket(customerId, productId, quantity);

            // Assert
            expected.Status.Should().Be(result);
        }

        [Fact]
        public async Task UpdateQuantityOfProductsInBasket_ShouldReturnNotFoundWhenBasketIsNull()
        {
            // Arrange
            int quantity = 1;
            int customerId = 1;
            int productId = 1;
            
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId));

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            var expected = await basketService.UpdateQuantityOfProductsInBasket(customerId, productId, quantity);

            // Assert
            expected.Status.Should().Be(Status.NotFound);
        }

        [Fact]
        public async Task UpdateQuantityOfProductsInBasket_ShouldReturnNotFoundWhenProductIsNotFound()
        {
            // Arrange
            int quantity = 1;
            int customerId = 1;
            int productId = 1;
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
            
            var basketMock = new Mock<IBasketRepository>();
            basketMock.Setup(x => x.GetBasket(customerId)).Returns(basketWithGoods);

            var customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
            customerDetailsRepositoryMock.Setup(x => x.GetCustomer(customerId)).ReturnsAsync(new Customer());

            var productDetailsRepositoryMock = new Mock<IProductDetailsRepository>();
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(default (Product));

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            var expected = await basketService.UpdateQuantityOfProductsInBasket(customerId, productId, quantity);

            // Assert
            expected.Status.Should().Be(Status.NotFound);
        }

        [Fact]
        public async Task UpdateQuantityOfProductsInBasket_ShouldOnlyUpdateQuantityOfProductsInBasket()
        {
            // Arrange
            int customerId = 1;
            int productId = 1;
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
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            await basketService.UpdateQuantityOfProductsInBasket(1, 1, 3);

            // Assert
            basketWithGoods.ProductIds[0].Quantity.Should().Be(4);
        }

        [Fact]
        public async Task UpdateQuantityOfProductsInBasket_ShouldUpdateProductsAndQuantityOfProductsInBasket()
        {
            // Arrange
            var customerId = 1;
            var productId = 1;
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
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(new Product
            {
                Name = "test",
                Price = 20,
                ProductId = 1
            });

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            await basketService.UpdateQuantityOfProductsInBasket(1, 1, 4);

            // Assert
            basketWithGoods.ProductIds.Should().HaveCount(1);
            basketWithGoods.ProductIds[0].Quantity.Should().Be(5);
            basketWithGoods.ProductIds[0].ProductId.Should().Be(1);
        }

        [Theory]
        [InlineData(1, Status.Ok)]
        [InlineData(2, Status.BadRequest)]
        public void RemoveFromBasket_ShouldRemoveProductFromBasket(int productId, Status result)
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
            productDetailsRepositoryMock.Setup(x => x.GetProductAsync(customerId)).ReturnsAsync(new Product());

            var basketService = new BasketService(basketMock.Object, customerDetailsRepositoryMock.Object, productDetailsRepositoryMock.Object);

            // Act
            var expected = basketService.RemoveFromBasket(customerId, productId);

            // Assert
            expected.Status.Should().Be(result);
        }
    }
}
