﻿using Ecommorce.Core.DTO;
using Ecommorce.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce.Core.interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<bool> AddAsync(AddProductDTO productDTO);
        Task<bool> UpdateAsync(UpdateProductDTO productDTO);
        Task  DeleteAsync(Product product);
    }
}
