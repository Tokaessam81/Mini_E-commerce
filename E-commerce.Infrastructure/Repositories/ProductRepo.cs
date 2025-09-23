using E_commerce.Application.Repository.Contract;
using E_commerce.Core.Domain.Entities;
using E_commerce.Infrastructure.Data;
using E_commerce.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Repositories
{
    public class ProductRepo:GenericRepository<Product>, IProductRepo
    {
        private readonly ECommerceDbContext _context;
        public ProductRepo(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
