using AutoMapper;
using E_commerce.Application.DTOs.ProductDtos;
using E_commerce.Application.Service.Contract;
using E_commerce.Core.Domain.Entities;
using E_commerce.PL.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace E_commerce.PL.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService,IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        [HttpGet("GetAllProducs")]
        [ProducesResponseType(typeof( ApiResponse < IReadOnlyList < ReadProductDto >>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<ReadProductDto>>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            if (!products.Success)
                return NotFound(new ApiResponse(404, products.Message));
            var productsDto = _mapper.Map<IReadOnlyList<ReadProductDto>>(products.Data);
            return Ok(new ApiResponse<IReadOnlyList < ReadProductDto >>(200,"Success",productsDto));
        }
        [HttpGet("GetProductById/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ReadProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ReadProductDto>>> GetById(int id)
        {
           var product = await _productService.GetProductByIdAsync(id);
        if (!product.Success)
            return NotFound(new ApiResponse(404, product.Message));
        var productDto = _mapper.Map<ReadProductDto>(product.Data);
        return Ok(new ApiResponse<ReadProductDto>(200, "Success", productDto));
        }
        [HttpDelete("DeleteProduct/{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productService.DeleteProductAsync(id);
            if (!product)
                return NotFound(new ApiResponse(404, "This Product Is Not Found"));
            return Ok(new ApiResponse(200, "Product Deleted Successfully"));
        }
        [HttpPost("CreateProduct")]
        [ProducesResponseType(typeof(ApiResponse<ReadProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ReadProductDto>>> CreateProduct(CreateProductDto dto)
        {
            string imagePath = null;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}_{dto.ImageFile.FileName}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                imagePath = $"images/{fileName}";

            }
            var product = _mapper.Map<Product>(dto);
            product.ImagePath = imagePath;
            var result = await _productService.CreateProductAsync(product);
            var ReadproductDto = _mapper.Map<ReadProductDto>(result.Data);
            if (result.Success)
                return Ok(new ApiResponse<ReadProductDto>(200,"Success", ReadproductDto));

            return BadRequest(new ApiResponse(400,"This an error while create product!"));
        }

        [HttpPut("UpdateProduct")]
        [ProducesResponseType(typeof(ApiResponse<ReadProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ReadProductDto>>> UpdateProduct(UpdateProductDto dto)
        {
            var existingProductResult = await _productService.GetProductByIdAsync(dto.Id);
            if (!existingProductResult.Success || existingProductResult ==null)
                return NotFound(new ApiResponse(404, "Product not found"));

            var product = existingProductResult.Data;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}_{dto.ImageFile.FileName}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                product.ImagePath = $"images/{fileName}";
            }

            product.Name = dto.Name;
            product.Category = dto.Category;
            product.Price = dto.Price;
            product.MinimumQuantity = dto.MinimumQuantity;
            product.DiscountRate = dto.DiscountRate;
            product.ProductCode = dto.ProductCode;

            var result = await _productService.UpdateProductAsync(product);

            var readProductDto = _mapper.Map<ReadProductDto>(result.Data);

            if (result.Success)
                return Ok(new ApiResponse<ReadProductDto>(200, "Product updated successfully", readProductDto));

            return BadRequest(new ApiResponse(400, "Error while updating product"));
        }

    }

}
