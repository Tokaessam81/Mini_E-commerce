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
    public class RefreshTokenRepo : GenericRepository<RefreshToken>, IRefreshTokenRepo
    {
        private readonly ECommerceDbContext _dbcontext;

        public RefreshTokenRepo(ECommerceDbContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<RefreshToken?> GetValidTokenWithUserAsync(string token)
        {
            var now = DateTime.UtcNow;

            return await _dbcontext.RefreshTokens
                .Include(rt => rt.User)
                .Where(rt => rt.Token == token && rt.Expires > now) 
                .FirstOrDefaultAsync();
        }
    }
}
