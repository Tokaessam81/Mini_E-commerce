using E_commerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Service.Contract
{
    public interface ICartService
    {
        Task<Cart> GetCartByUserId(int userId);
        Task AddToCart(int userId, int productId, int quantity);
        Task RemoveFromCart(int userId, int productId);
        Task ClearCart(int userId);
        Task<int> GetCartItemsCount(int userId);
    }
}
