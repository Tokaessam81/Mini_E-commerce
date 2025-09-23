using E_commerce.Application.Repository.Contract;
using E_commerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Persistence
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<int> CompleteAsync();
        IUserRepo _userRepo { get; set; }
        IRefreshTokenRepo _refreshTokenRepo { get; set; }
        IProductRepo _ProductRepository { get; set; }
    }
}
