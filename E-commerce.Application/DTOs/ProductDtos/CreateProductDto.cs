using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.DTOs.ProductDtos
{
    public class CreateProductDto :BaseProductDto
    {
        public IFormFile ImageFile { get; set; } = null!;
    }
}
