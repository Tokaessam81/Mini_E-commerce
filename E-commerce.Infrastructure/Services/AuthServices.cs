using E_commerce.Application.Common;
using E_commerce.Application.DTOs.AuthDtos;
using E_commerce.Application.Persistence;
using E_commerce.Application.Service.Contract;
using E_commerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Services
{
    public class AuthServices : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthServices(ITokenService tokenService,IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<TokenResponseDto>> LoginAsync(LoginDto dto,string ipAddress)
        {
            var user = await _unitOfWork._userRepo.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return ServiceResult<TokenResponseDto>.Fail("Invalid email or password");

            user.LastLoginTime = DateTime.UtcNow;
            _unitOfWork._userRepo.Update(user);
            await _unitOfWork.CompleteAsync();


            return ServiceResult<TokenResponseDto>.Ok(
                await _tokenService.GenerateTokensAsync(user, ipAddress)
            );
        }


        public async Task<ServiceResult<string>> RegisterAsync(RegisterDto dto)
        {
            var user = await _unitOfWork._userRepo.GetByEmailAsync(dto.Email);
            if (user != null)
            {
                return ServiceResult<string>.Fail("Email is already in use");
            }
            var newUser = new Core.Domain.Entities.User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                LastLoginTime = null
            };
            await _unitOfWork._userRepo.AddAsync(newUser);
            await _unitOfWork.CompleteAsync();
            return ServiceResult<string>.Ok("User registered successfully");
        }
    }
}
