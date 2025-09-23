using E_commerce.Application.DTOs.AuthDtos;
using E_commerce.Application.Service.Contract;
using E_commerce.Infrastructure.Services;
using E_commerce.PL.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_commerce.PL.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AccountController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<TokenResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<TokenResponseDto>>> Login(LoginDto dto)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            var token = await _authService.LoginAsync(dto, ipAddress);
            if (!token.Success)
                return Unauthorized(new ApiResponse(401, token.Message));

            return Ok(new ApiResponse<TokenResponseDto>(200,"Success",token.Data));
        }
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.Success)
                return BadRequest(new ApiResponse(400, result.Message));
            return Ok(new ApiResponse(200,result.Data));
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] string token)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            var response = await _tokenService.RefreshTokenAsync(token, ipAddress);
            if (response == null)
                return Unauthorized("Invalid or expired token");

            return Ok(response);
        }

    }
}
