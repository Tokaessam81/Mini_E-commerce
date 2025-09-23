using E_commerce.Application.Common;
using E_commerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Service.Contract
{
    public interface IProductService
    {
        Task<ServiceResult<IReadOnlyList<Product>>> GetAllProductsAsync();
        Task<ServiceResult<Product>> GetProductByIdAsync(int id);
        Task<bool> DeleteProductAsync(int id);
        Task<ServiceResult<Product>> CreateProductAsync(Product newProduct);
        Task<ServiceResult<Product>> UpdateProductAsync(Product updatedProduct);

    }
}
