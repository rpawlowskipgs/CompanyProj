using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Basket.Configuration;
using Basket.Services;
using CompanyProducts.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BasketTest
{
    public class ProductDetailsRepositoryTests
    {
        [Fact]
        public async Task GetProduct_ShouldReturnProductDetails()
        {
            // Arrange
            var product = new Product
            {
                ProductId = 1,
                Name = "Ball",
                Price = 20
            };
            var apiHelperMock = new Mock<IApiHelper>();
            apiHelperMock.Setup(x => x.Get<Product>(It.IsAny<Uri>())).ReturnsAsync(product);

            var optionsMock = new Mock<IOptions<ProductConfiguration>>();
            optionsMock.Setup(x => x.Value).Returns(new ProductConfiguration
            {
                ServiceUrlConfiguration = "https://trojmiasto.pl"
            });

            var productDetails = new ProductDetailsRepository(apiHelperMock.Object, optionsMock.Object);

            // Act
            var result = await productDetails.GetProduct(1);

            // Assert
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetCustomer_ShouldAddProductIdToUrl()
        {
            // Arrange
            var apiHelperMock = new Mock<IApiHelper>();
            apiHelperMock.Setup(x => x.Get<Product>(It.IsAny<Uri>()));

            var optionsMock = new Mock<IOptions<ProductConfiguration>>();
            optionsMock.Setup(x => x.Value).Returns(new ProductConfiguration
            {
                ServiceUrlConfiguration = "https://trojmiasto.pl/{0}"
            });

            var productDetails = new ProductDetailsRepository(apiHelperMock.Object, optionsMock.Object);

            // Act
            await productDetails.GetProduct(1);

            // Assert
            apiHelperMock.Verify(x => x.Get<Product>(new Uri("https://trojmiasto.pl/1")));
            apiHelperMock.Verify(x => x.Get<Product>(new Uri("https://trojmiasto.pl/2")), Times.Never);
        }
    }
}
