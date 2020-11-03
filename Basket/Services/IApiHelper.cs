using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Services
{
    public interface IApiHelper
    {
        public Task<T> Get<T>(Uri uri);
    }
}
