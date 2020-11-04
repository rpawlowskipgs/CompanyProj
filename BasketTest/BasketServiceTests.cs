using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            // Act
            basketService.AddProductsToBasket(1, 2, 3);

            // Assert
            basketMock.Verify(x => x.UpdateBasket(It.Is<List<ProductsInBasket>>(x => x[1].Quantity == 3), customerId));
        }
    }
}
