﻿using System;
using System.Threading.Tasks;
using Basket.Configuration;
using Microsoft.Extensions.Options;
using Models.Customers.Models;

namespace Basket.Services
{
    public class CustomerDetailsRepository : ICustomerDetailsRepository
    {
        private readonly IApiHelper _apiHelper;
        private readonly IOptions<CustomerConfiguration> _settingsUrl;

        public CustomerDetailsRepository(IApiHelper apiHelper, IOptions<CustomerConfiguration> settingsUrl)
        {
            _apiHelper = apiHelper;
            _settingsUrl = settingsUrl;
        }

        public async Task<Customer> GetCustomer(int customerId)
        {
            return await _apiHelper.Get<Customer>(new Uri(string.Format(_settingsUrl.Value.ServiceUrlConfiguration, customerId)));
        }
    }
}
