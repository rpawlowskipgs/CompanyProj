using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Configuration;
using Basket.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Basket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IBasketRepository, BasketRepository>();
            services.AddSingleton<ApiHelper>();
            services.AddTransient<ICustomerDetailsRepository, CustomerDetailsRepository>();
            services.AddTransient<IProductDetailsRepository, ProductDetailsRepository>();
            services.AddTransient<IBasketService, BasketService>();
            services.AddMvc();
            services.AddOptions();
            services.Configure<ProductConfiguration>(Configuration.GetSection("ProductsSettings"));
            services.Configure<CustomerConfiguration>(Configuration.GetSection("CustomerSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
