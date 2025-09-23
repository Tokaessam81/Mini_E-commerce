using E_commerce.Application.Persistence;
using E_commerce.Application.Repository.Contract;
using E_commerce.Core.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Tests.UnitTests.Helpers
{
    public class MockUnitOfWork
    {
        public Mock<IUnitOfWork> UnitOfWork { get; private set; }

        public Mock<IUserRepo> UserRepo { get; private set; }
        public Mock<IProductRepo> ProductRepo { get; private set; }

        public MockUnitOfWork()
        {
            UserRepo = new Mock<IUserRepo>();
            ProductRepo = new Mock<IProductRepo>();

            UnitOfWork = new Mock<IUnitOfWork>();
            UnitOfWork.SetupGet(u => u._userRepo).Returns(UserRepo.Object);
            UnitOfWork.SetupGet(u => u._ProductRepository).Returns(ProductRepo.Object);
        }

        public void SetupGetUserByEmail(string email, User? user)
        {
            UserRepo.Setup(u => u.GetByEmailAsync(email))
                .ReturnsAsync(user);
        }

        public void SetupGetProductById(int id, Product? product)
        {
            ProductRepo.Setup(p => p.GetByIdAsync(id))
                .ReturnsAsync(product);
        }

        public void SetupGetAllProducts(List<Product> products)
        {
            ProductRepo.Setup(p => p.GetAllAsync())
                .ReturnsAsync(products);
        }
    }
}
