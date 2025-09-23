using E_commerce.Application.Common;
using E_commerce.Application.DTOs.ProductDtos;
using E_commerce.Application.Persistence;
using E_commerce.Application.Service.Contract;
using E_commerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Services
{
    public class ProductServices : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<IReadOnlyList<Product>>> GetAllProductsAsync()
        {
            var products = await _unitOfWork._ProductRepository.GetAllAsync();
            if (products == null || !products.Any())
                return ServiceResult<IReadOnlyList<Product>>.Fail("No Products Found");
            return ServiceResult<IReadOnlyList<Product>>.Ok(products);
        }
        public async Task<ServiceResult<Product>> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork._ProductRepository.GetByIdAsync(id);
            if (product == null)
                return ServiceResult<Product>.Fail("Product Not Found");
            return ServiceResult<Product>.Ok(product);
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork._ProductRepository.GetByIdAsync(id);
            if (product == null)
                return false;
            _unitOfWork._ProductRepository.Remove(product);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<ServiceResult<Product>> CreateProductAsync(Product product)
        {
            await _unitOfWork._ProductRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return ServiceResult<Product>.Ok(product);
        }
        public async Task<ServiceResult<Product>> UpdateProductAsync(Product product)
        {
            _unitOfWork._ProductRepository.Update(product);
            await _unitOfWork.CompleteAsync();
            return ServiceResult<Product>.Ok(product);
        }

    }
}
