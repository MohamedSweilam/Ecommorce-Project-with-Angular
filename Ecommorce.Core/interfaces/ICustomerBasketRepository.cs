﻿using Ecommorce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce.Core.interfaces
{
    public interface ICustomerBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string id);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
