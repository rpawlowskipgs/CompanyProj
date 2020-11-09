using System.Collections.Generic;
using System.Linq;
using Basket.Models;
using Basket.Services;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BasketTest
{
    public class BasketRepositoryTests
    {
        private IBasketRepository _basket = new BasketRepository();

        [Fact]
        public void AddToBasket_ShouldAddProductsToBasket()
        {
            // Arrange
            var basket = new BasketWithGoods
            {
                CustomerId = 1,
                ProductIds = new List<ProductsInBasket>
                {
                    new ProductsInBasket
                    {
                        ProductId = 1,
                        Quantity = 1
                    }
                }
            };

            // Act
            _basket.AddToBasket(basket);
            var expected = _basket.GetBasket(1);

            // Assert
            expected.Should().BeEquivalentTo(basket);
        }

        [Fact]
        public void GetBasket_ShouldReturnAllProductsInBasket()
        {
            // Arrange
            var basket = new BasketWithGoods
            {
                CustomerId = 1,
                ProductIds = new List<ProductsInBasket>
                {
                    new ProductsInBasket
                    {
                        ProductId = 1,
                        Quantity = 1
                    }
                }
            };

            // Act
            _basket.AddToBasket(basket);
            var expected = _basket.GetBasket(1);

            // Assert
            expected.Should().BeEquivalentTo(basket);
        }

        [Fact]
        public void UpdateBasket_ShouldUpdateProductsInBasket()
        {
            // Arrange
            var customerId = 1;
            var basket = new BasketWithGoods
            {
                CustomerId = 1,
                ProductIds = new List<ProductsInBasket>
                {
                    new ProductsInBasket
                    {
                        ProductId = 1,
                        Quantity = 1
                    }
                }
            };

            var products = new List<ProductsInBasket>
            {
                new ProductsInBasket
                {
                    ProductId = 2,
                    Quantity = 2
                }
            };

            // Act
            _basket.AddToBasket(basket);
            _basket.UpdateBasket(products, customerId);
            var expected = _basket.GetBasket(customerId);

            // Assert
            expected.Should().BeEquivalentTo(basket);
            expected.ProductIds.First().As<ProductsInBasket>().ProductId.Should().Be(2);
            expected.ProductIds.First().As<ProductsInBasket>().Quantity.Should().Be(2);
        }
    }
}