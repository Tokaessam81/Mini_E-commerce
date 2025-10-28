using E_commerce.Application.Repository.Contract;
using E_commerce.Core.Domain.Entities;
using E_commerce.Infrastructure.Data;
using E_commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Repositories
{
    public class CartRepo:GenericRepository<Cart>, ICartRepo
    {
        private readonly ECommerceDbContext _context;
        public CartRepo(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cart>? GetByIdEagerAsync(int userId)
        {
            var Cart =await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
            if(Cart == null)
            {
               return null!;
            }
            return Cart;
 
        }
        public async Task<int> GetCount(int userId)
        {
            var cart = await GetByIdEagerAsync(userId);
            if (cart == null)
            {
                return 0;
            }
            return cart.Items.Sum(i => i.Quantity);
        }
    }
}
