using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.DTOs.ProductDtos
{
    public class BaseProductDto 
    {
        public string Category { get; set; } = null!;
        public string ProductCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int MinimumQuantity { get; set; }
        public double? DiscountRate { get; set; }
    }
}
