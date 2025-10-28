using AutoMapper;
using E_commerce.Application.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.PL.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartService.GetCartByUserId(userId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int userId, int productId, int quantity = 1)
        {
            await _cartService.AddToCart(userId, productId, quantity);
            return Ok(new { message = "Product added to cart successfully" });
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            await _cartService.RemoveFromCart(userId, productId);
            return Ok(new { message = "Product removed from cart successfully" });
        }

        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _cartService.ClearCart(userId);
            return Ok(new { message = "Cart cleared successfully" });
        }
        [HttpGet("GetCartItemsCount/{userId}")]
        public async Task<IActionResult> GetCartItemsCount(int userId)
        {
            var itemCount = await _cartService.GetCartItemsCount(userId);
            return Ok(itemCount);
        }

    }


}
