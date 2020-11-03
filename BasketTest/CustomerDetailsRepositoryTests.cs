using System;
using System.Threading.Tasks;
using Basket.Configuration;
using Basket.Services;
using Customers.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BasketTest
{
    public class CustomerDetailsRepositoryTests
    {

        [Fact]
        public async Task GetCustomer_ShouldReturnCustomerDetails()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerId = 1,
                FirstName = "John",
                LastName = "Smith",
                Age = 20
            };
            var apiHelperMock = new Mock<IApiHelper>();
            apiHelperMock.Setup(x => x.Get<Customer>(It.IsAny<Uri>())).ReturnsAsync(customer);

            var optionsMock = new Mock<IOptions<CustomerConfiguration>>();
            optionsMock.Setup(x => x.Value).Returns(new CustomerConfiguration
            {
                ServiceUrlConfiguration = "https://trojmiasto.pl"
            });

            var customerDetails = new CustomerDetailsRepository(apiHelperMock.Object, optionsMock.Object);

            // Act
            var result = await customerDetails.GetCustomer(1);

            // Assert
            result.Should().BeEquivalentTo(customer);
        }

        [Fact]
        public async Task GetCustomer_ShouldAddCustomerIdToUrl()
        {
            // Arrange
            var apiHelperMock = new Mock<IApiHelper>();
            apiHelperMock.Setup(x => x.Get<Customer>(It.IsAny<Uri>()));

            var optionsMock = new Mock<IOptions<CustomerConfiguration>>();
            optionsMock.Setup(x => x.Value).Returns(new CustomerConfiguration
            {
                ServiceUrlConfiguration = "https://trojmiasto.pl/{0}"
            });

            var customerDetails = new CustomerDetailsRepository(apiHelperMock.Object, optionsMock.Object);

            // Act
            await customerDetails.GetCustomer(1);

            // Assert
            apiHelperMock.Verify(x => x.Get<Customer>(new Uri("https://trojmiasto.pl/1")));
            apiHelperMock.Verify(x => x.Get<Customer>(new Uri("https://trojmiasto.pl/2")), Times.Never);
        }
    }
}
