using E_commerce.Application.DTOs.AuthDtos;
using E_commerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Service.Contract
{
    public interface ITokenService
    {
        Task<TokenResponseDto> GenerateTokensAsync(User user, string ipAddress);
        Task<TokenResponseDto?> RefreshTokenAsync(string refreshToken, string ipAddress);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
