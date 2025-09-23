using E_commerce.Application.Repository.Contract;
using E_commerce.Core.Domain.Entities;
using E_commerce.Infrastructure.Data;
using E_commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Repositories
{
    public class UserRepo :GenericRepository<User>, IUserRepo
    {
        private readonly ECommerceDbContext _dbcontext;

        public UserRepo(ECommerceDbContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public Task<User?> GetByEmailAsync(string Email)
        {
            return _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == Email);
        }
    }
}
