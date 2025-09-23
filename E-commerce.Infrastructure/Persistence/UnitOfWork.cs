using E_commerce.Application.Persistence;
using E_commerce.Application.Repository.Contract;
using E_commerce.Core.Domain.Entities;
using E_commerce.Infrastructure.Data;
using E_commerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceDbContext _dbcontext;
        public IUserRepo _userRepo { get; set; }
        public IRefreshTokenRepo _refreshTokenRepo { get; set; }
        public IProductRepo _ProductRepository { get ; set ; }

        public UnitOfWork(ECommerceDbContext context,IUserRepo userRepo, IRefreshTokenRepo refreshTokenRepo, IProductRepo productRepository)
        {
            _dbcontext = context;
            _userRepo = userRepo;
            _refreshTokenRepo = refreshTokenRepo;
            _ProductRepository = productRepository;
        }
        public async Task<int> CompleteAsync() => await _dbcontext.SaveChangesAsync();

        public async ValueTask DisposeAsync() => await _dbcontext.DisposeAsync();
    }
}
