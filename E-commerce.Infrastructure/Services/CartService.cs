using E_commerce.Application.Persistence;
using E_commerce.Application.Service.Contract;
using E_commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CartService(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public async Task AddToCart(int userId, int productId, int quantity)
        {
            var cart =await GetCartByUserId(userId);
            var product = await _UnitOfWork._ProductRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CartId = cart.Id
                };
                await _UnitOfWork._CartItemRepository.AddAsync(newItem);
            }

            await _UnitOfWork.CompleteAsync();
        }

        public async Task ClearCart(int userId)
        {
            var cart =await GetCartByUserId(userId);
            _UnitOfWork._CartItemRepository.RemoveRange(cart.Items);
           await _UnitOfWork.CompleteAsync();
        }

        public async Task<Cart> GetCartByUserId(int userId)
        {
            var cart = await _UnitOfWork._CartRepository.GetByIdEagerAsync(userId)!;
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _UnitOfWork._CartRepository.AddAsync(cart);
                await _UnitOfWork.CompleteAsync();
            }
            return cart;
        }

        public Task<int> GetCartItemsCount(int userId)
        {
            return _UnitOfWork._CartRepository.GetCount(userId);
        }

        public async Task RemoveFromCart(int userId, int productId)
        {
            var cart =await GetCartByUserId(userId);
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                _UnitOfWork._CartItemRepository.Remove(existingItem);
               await _UnitOfWork.CompleteAsync();
            }

        }
    }
}
