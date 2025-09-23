using E_commerce.Application.DTOs.AuthDtos;
using E_commerce.Application.Persistence;
using E_commerce.Application.Service.Contract;
using E_commerce.Core.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Services
{
    public class TokenServices : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public TokenServices(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _config = config;
            _unitOfWork = unitOfWork;
        }
        //Generate Access and Refresh Token
        public async Task<TokenResponseDto> GenerateTokensAsync(User user, string ipAddress)
        {
            var accessToken = GenerateAccessToken(user);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                UserId = user.Id
            };

            await _unitOfWork._refreshTokenRepo.AddAsync(refreshToken);
            await _unitOfWork.CompleteAsync();

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                Expiration = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:DurationInMinutes"]!))
            };
        }
        //new Refresh Token
        public async Task<TokenResponseDto?> RefreshTokenAsync(string refreshToken, string ipAddress)
        {
            var storedToken = await _unitOfWork._refreshTokenRepo.GetValidTokenWithUserAsync(refreshToken);
            if (storedToken == null || storedToken.IsExpired)
                return null;

            var user = storedToken.User;

             _unitOfWork._refreshTokenRepo.Remove(storedToken);

            return await GenerateTokensAsync(user, ipAddress);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _unitOfWork._refreshTokenRepo.GetValidTokenWithUserAsync(refreshToken);
            if (storedToken != null)
            {
                _unitOfWork._refreshTokenRepo.Remove(storedToken);
                await _unitOfWork.CompleteAsync();
            }
        }
        //Generate Access Token
        private string GenerateAccessToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:DurationInMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
