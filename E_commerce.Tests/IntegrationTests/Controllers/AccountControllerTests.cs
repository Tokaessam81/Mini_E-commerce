using E_commerce.Application.Common;
using E_commerce.Application.DTOs.AuthDtos;
using E_commerce.Application.Service.Contract;
using E_commerce.Infrastructure.Services;
using E_commerce.PL.Controllers;
using E_commerce.PL.Errors;
using E_commerce.Tests.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Tests.IntegrationTests.Controllers
{
    public class AccountControllerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ITokenService> _mockTokenService;

        private readonly AccountController _controller;

        public AccountControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
            _mockAuthService = new Mock<IAuthService>();
            _mockTokenService = new Mock<ITokenService>();

            _controller = new AccountController(_mockAuthService.Object, _mockTokenService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        Connection = { RemoteIpAddress = System.Net.IPAddress.Loopback }
                    }
                }
            };
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenEmailExists()
        {
            var dto = new RegisterDto { UserName = "Test", Email = "existing@test.com", Password = "123456" };

            _mockAuthService.Setup(x => x.RegisterAsync(It.IsAny<RegisterDto>()))
                            .ReturnsAsync(ServiceResult<string>.Fail("Email is already in use"));

            var result = await _controller.Register(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse>(badRequest.Value);
            Assert.Equal(400, apiResponse.StatusCode);
            Assert.Equal("Email is already in use", apiResponse.Message);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            var loginDto = new LoginDto { Email = "test@test.com", Password = "password" };
            var tokenResponse = new TokenResponseDto { AccessToken = "access", RefreshToken = "refresh" };

            _mockAuthService.Setup(x => x.LoginAsync(It.IsAny<LoginDto>(), It.IsAny<string>()))
                            .ReturnsAsync(ServiceResult<TokenResponseDto>.Ok(tokenResponse));

            var result = await _controller.Login(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<TokenResponseDto>>(okResult.Value);
            Assert.Equal("access", apiResponse.Result!.AccessToken);
            Assert.Equal("refresh", apiResponse.Result.RefreshToken);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            var loginDto = new LoginDto { Email = "wrong@test.com", Password = "wrong" };

            _mockAuthService.Setup(x => x.LoginAsync(It.IsAny<LoginDto>(), It.IsAny<string>()))
                            .ReturnsAsync(ServiceResult<TokenResponseDto>.Fail("Invalid credentials"));

            var result = await _controller.Login(loginDto);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse>(unauthorizedResult.Value);
            Assert.Equal("Invalid credentials", apiResponse.Message);
        }

        [Fact]
        public async Task RefreshToken_ReturnsOk_WhenTokenIsValid()
        {
            var token = "refresh_token";
            var tokenResponse = new TokenResponseDto { AccessToken = "new_access", RefreshToken = "new_refresh" };

            _mockTokenService.Setup(x => x.RefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>()))
                             .ReturnsAsync(tokenResponse);

            var result = await _controller.RefreshToken(token);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<TokenResponseDto>(okResult.Value);
            Assert.Equal("new_access", response.AccessToken);
            Assert.Equal("new_refresh", response.RefreshToken);
        }

        [Fact]
        public async Task RefreshToken_ReturnsUnauthorized_WhenTokenIsInvalid()
        {
            var token = "invalid_token";

            _mockTokenService.Setup(x => x.RefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>()))
                             .ReturnsAsync((TokenResponseDto?)null);

            var result = await _controller.RefreshToken(token);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}