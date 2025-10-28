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
    public class CartItemRepo:GenericRepository<CartItem>, ICartItemRepo
    {
        private readonly ECommerceDbContext _dbContext;
        public CartItemRepo(ECommerceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
