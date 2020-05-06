using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByProvider(Guid providerId);
        Task<IEnumerable<Product>> GetProducsProviders();
        Task<Product> GetProductProvider(Guid id);
    }
}
