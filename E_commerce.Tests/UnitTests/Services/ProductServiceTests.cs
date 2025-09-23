using E_commerce.Application.Persistence;
using E_commerce.Core.Domain.Entities;
using E_commerce.Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Tests.UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ProductServices _service;

        public ProductServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new ProductServices(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsProducts_WhenProductsExist()
        {
            var products = new List<Product>
            {
                new Product { Name = "Prod1", ProductCode = "P01", Price = 10, Category = "Cat1" }
            };
            _mockUnitOfWork.Setup(u => u._ProductRepository.GetAllAsync())
                .ReturnsAsync(products);

            var result = await _service.GetAllProductsAsync();

            Assert.True(result.Success);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task CreateProductAsync_ReturnsCreatedProduct()
        {
            var product = new Product
            {
                Name = "Prod2",
                ProductCode = "P02",
                Price = 20,
                Category = "Cat2",
                MinimumQuantity = 1,
                ImagePath = "images/default.png"
            };

            _mockUnitOfWork.Setup(u => u._ProductRepository.AddAsync(It.IsAny<Product>()))
                           .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.CompleteAsync())
            .ReturnsAsync(1);

            var service = new ProductServices(_mockUnitOfWork.Object);

            var result = await service.CreateProductAsync(product);
            
            Assert.True(result.Success);
            Assert.Equal("Prod2", result.Data.Name);
            Assert.Equal("P02", result.Data.ProductCode);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsProduct_WhenExists()
        {
            var product = new Product { Id = 1, Name = "Prod1" };
            _mockUnitOfWork.Setup(u => u._ProductRepository.GetByIdAsync(1))
                           .ReturnsAsync(product);

            var result = await _service.GetProductByIdAsync(1);

            Assert.True(result.Success);
            Assert.Equal("Prod1", result.Data.Name);
        }

        [Fact]
        public async Task DeleteProductAsync_ReturnsTrue_WhenProductExists()
        {
            var product = new Product { Id = 1, Name = "Prod1" };
            _mockUnitOfWork.Setup(u => u._ProductRepository.GetByIdAsync(1))
                           .ReturnsAsync(product);
            _mockUnitOfWork.Setup(u => u._ProductRepository.Remove(product));
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _service.DeleteProductAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateProductAsync_ReturnsUpdatedProduct()
        {
            var product = new Product { Id = 1, Name = "Prod1" };
            _mockUnitOfWork.Setup(u => u._ProductRepository.Update(product));
             _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _service.UpdateProductAsync(product);

            Assert.True(result.Success);
            Assert.Equal("Prod1", result.Data.Name);
        }

    }
}
