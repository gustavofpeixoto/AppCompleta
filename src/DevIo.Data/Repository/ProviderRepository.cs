using DevIO.Data.Context;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class ProviderRepository : Repository<Provider>, IProviderRepository
    {
        public ProviderRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<Provider> GetProviderAddress(Guid id)
        {
            return await _dbSet.AsNoTracking()
                .Include(a => a.Address)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Provider> GetProviderProductsAddress(Guid id)
        {
            return await _dbSet.AsNoTracking()
                .Include(pdc => pdc.Products)
                .Include(a => a.Address)
                .FirstOrDefaultAsync(pvd => pvd.Id == id);
        }
    }
}
