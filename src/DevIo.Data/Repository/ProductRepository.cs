using DevIO.Data.Context;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext appDbContext) : base(appDbContext) { }
        public async Task<IEnumerable<Product>> GetProducsProviders()
        {
            return await _dbSet.AsNoTracking().Include(pvd => pvd.Provider)
                .OrderBy(pdc => pdc.Name)
                .ToListAsync();
        }

        public async Task<Product> GetProductProvider(Guid id)
        {
            return await _dbSet.AsNoTracking().Include(pvd => pvd.Provider)
                .FirstOrDefaultAsync(pdc => pdc.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByProvider(Guid providerId)
        {
            return await Find(p => p.ProviderId == providerId);
        }
    }
}
