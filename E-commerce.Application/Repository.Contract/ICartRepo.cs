using E_commerce.Application.Persistence;
using E_commerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Repository.Contract
{
    public interface ICartRepo:IGenericRepository<Cart>
    {
        Task<Cart>? GetByIdEagerAsync(int userId);
        Task<int> GetCount(int userId);
    }
}
