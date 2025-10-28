using AutoMapper;
using E_commerce.Application.Common;
using E_commerce.Application.DTOs.ProductDtos;
using E_commerce.Application.Mappers;
using E_commerce.Application.Service.Contract;
using E_commerce.Core.Domain.Entities;
using E_commerce.Infrastructure.Data;
using E_commerce.Infrastructure.Persistence;
using E_commerce.Infrastructure.Repositories;
using E_commerce.Infrastructure.Services;
using E_commerce.PL.Controllers;
using E_commerce.PL.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace E_commerce.Tests.IntegrationTests.Controllers
{
    public class ProductControllerTests
    {
        private readonly ProductController _controller;
        private readonly Mock<IProductService> _mockService;
        private readonly IMapper _mapper;
        private readonly IProductService _service;
        private readonly ECommerceDbContext _context;
        private readonly Mock<IMapper> _mockMapper;


        public ProductControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _mockMapper = new Mock<IMapper>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
     .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
     .Options;

            _context = new ECommerceDbContext(options);

            var productRepo = new ProductRepo(_context);
            var userRepo = new UserRepo(_context); 
            var cartRepo = new CartRepo(_context);
            var cartItemRepo = new CartItemRepo(_context);
            var refreshRepo = new RefreshTokenRepo(_context);

            var unitOfWork = new UnitOfWork(_context, userRepo, refreshRepo, productRepo,cartRepo,cartItemRepo);

            _service = new ProductServices(unitOfWork);
            _controller = new ProductController(_service, _mapper);
        }

        [Fact]
        public async Task GetAll_ReturnsProducts()
        {
            var product1 = new Product
            {
                Name = "Test Product 1",
                ProductCode = "P01",
                Price = 100,
                Category = "Category1",
                MinimumQuantity = 1,
                ImagePath = "images/default.png"
            };
            var product2 = new Product
            {
                Name = "Test Product 2",
                ProductCode = "P02",
                Price = 200,
                Category = "Category2",
                MinimumQuantity = 1,
                ImagePath = "images/default.png"
            };

            await _service.CreateProductAsync(product1);
            await _service.CreateProductAsync(product2);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IReadOnlyList<ReadProductDto>>>(okResult.Value);
            Assert.Equal(2, apiResponse.Result.Count);
            Assert.Equal("Test Product 1", apiResponse.Result[0].Name);
            Assert.Equal("Test Product 2", apiResponse.Result[1].Name);
        }
    
        [Fact]
        public async Task GetById_ReturnsProduct()
        {
            var product = new Product
            {
                Name = "ProductById",
                ProductCode = "P02",
                Price = 50,
                Category = "Category2",
                MinimumQuantity = 1,
                ImagePath = "images/default.png"
            };
            var created = await _service.CreateProductAsync(product);

            var result = await _controller.GetById(created.Data.Id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<ReadProductDto>>(okResult.Value);
            Assert.Equal("ProductById", apiResponse.Result.Name);
        }

        [Fact]
        public async Task CreateProduct_ReturnsOk_WhenProductCreated()
        {
            var content = "Fake image content"; 
            var fileName = "test.png";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

            IFormFile fakeFile = new FormFile(stream, 0, stream.Length, "ImageFile", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };

            var dto = new CreateProductDto
            {
                Name = "Prod1",
                ProductCode = "P01",
                Price = 50,
                Category = "Cat",
                ImageFile = fakeFile 
            };
            var product = new Product { Name = dto.Name, ProductCode = dto.ProductCode, Price = dto.Price, Category = dto.Category, ImagePath = "images/default.png" };

            _mockService.Setup(s => s.CreateProductAsync(It.IsAny<Product>()))
                        .ReturnsAsync(ServiceResult<Product>.Ok(product));

            var result = await _controller.CreateProduct(dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<ReadProductDto>>(okResult.Value);
            Assert.Equal("Prod1", apiResponse.Result!.Name);
        }
        
        [Fact]
        public async Task DeleteProduct_ReturnsOk_WhenProductDeleted()
        {
            var product = new Product
            {
                Name = "ToDelete",
                ProductCode = "DEL01",
                Price = 100,
                Category = "CategoryX",
                MinimumQuantity = 1,
                ImagePath = "images/default.png"
            };

            var created = await _service.CreateProductAsync(product);

            var result = await _controller.DeleteProduct(created.Data.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal(200, apiResponse.StatusCode);
            Assert.Equal("Product deleted successfully", apiResponse.Message, ignoreCase: true);

        }

        [Fact]
        public async Task UpdateProduct_ReturnsOk_WhenProductUpdated()
        {
            var dto = new UpdateProductDto
            {
                Id = 1,
                Name = "Updated",
                ProductCode = "P01",
                Price = 60,
                Category = "Cat"
            };

            var existingProduct = new Product
            {
                Id = 1,
                Name = "Old",
                ProductCode = "P01",
                Price = 50,
                Category = "Cat"
            };

            _mockService.Setup(s => s.GetProductByIdAsync(1))
                .ReturnsAsync(ServiceResult<Product>.Ok(existingProduct));

            _mockService.Setup(s => s.UpdateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(ServiceResult<Product>.Ok(existingProduct));

            _mockMapper.Setup(m => m.Map<ReadProductDto>(It.IsAny<Product>()))
                .Returns(new ReadProductDto
                {
                    Id = 1,
                    Name = "Updated",
                    ProductCode = "P01",
                    Price = 60,
                    Category = "Cat"
                });

            var controller = new ProductController(_mockService.Object, _mockMapper.Object);

            var result = await controller.UpdateProduct(dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<ReadProductDto>>(okResult.Value);
            Assert.Equal("Updated", apiResponse.Result!.Name);
        }

    }
}
