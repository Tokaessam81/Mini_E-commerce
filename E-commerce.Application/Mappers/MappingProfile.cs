using AutoMapper;
using E_commerce.Application.DTOs.ProductDtos;
using E_commerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Mappers
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Product,ReadProductDto>().ReverseMap();
            CreateMap<CreateProductDto, Product>().ReverseMap();
        }
    }
}
