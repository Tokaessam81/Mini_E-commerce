using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.DTOs.ProductDtos
{
    public class ReadProductDto : BaseProductDto
    {
        public int Id { get; set; }

        public string ImagePath { get; set; } = null!;
    }
}
