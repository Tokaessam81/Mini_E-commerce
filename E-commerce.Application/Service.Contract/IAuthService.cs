using E_commerce.Application.Common;
using E_commerce.Application.DTOs.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Service.Contract
{
    public interface IAuthService
    {
        Task<ServiceResult<string>> RegisterAsync(RegisterDto dto);
        Task<ServiceResult<TokenResponseDto>> LoginAsync(LoginDto dto, string ipAddress);
    }
}
