using E_commerce.Application.Persistence;
using E_commerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Repository.Contract
{
    public interface IProductRepo :IGenericRepository<Product>
    {
    }
}
