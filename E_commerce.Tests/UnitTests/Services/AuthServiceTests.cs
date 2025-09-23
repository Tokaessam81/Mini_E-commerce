using E_commerce.Application.DTOs.AuthDtos;
using E_commerce.Core.Domain.Entities;
using E_commerce.Infrastructure.Services;
using E_commerce.Tests.IntegrationTests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Tests.UnitTests.Services
{
    public class AuthServiceTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly AuthServices _authService;

        public AuthServiceTests(TestFixture fixture)
        {
            _fixture = fixture;
            _authService = new AuthServices(_fixture.MockTokenService.Object, _fixture.MockUnitOfWork.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFail_WhenUserNotFound()
        {
            _fixture.MockUnitOfWork.Setup(u => u._userRepo.GetByEmailAsync(It.IsAny<string>()))
                                   .ReturnsAsync((User)null);

            var loginDto = new LoginDto { Email = "test@test.com", Password = "123456" };

            var result = await _authService.LoginAsync(loginDto, "127.0.0.1");

            Assert.False(result.Success);
            Assert.Equal("Invalid email or password", result.Message);
        }

        [Fact]
        public async Task RegisterAsync_ShouldFail_WhenEmailExists()
        {
            _fixture.MockUnitOfWork.Setup(u => u._userRepo.GetByEmailAsync(It.IsAny<string>()))
                                   .ReturnsAsync(new User { Email = "existing@test.com" });

            var registerDto = new RegisterDto { UserName = "Test", Email = "existing@test.com", Password = "123456" };

            var result = await _authService.RegisterAsync(registerDto);

            Assert.False(result.Success);
            Assert.Equal("Email is already in use", result.Message);
        }
    }
}
