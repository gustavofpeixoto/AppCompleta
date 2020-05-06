using DevIO.Data.Context;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<Address> GetAddressByProvider(Guid providerId)
        {
            return await _dbSet.AsNoTracking().FirstAsync(a => a.ProviderId == providerId);
        }
    }
}
