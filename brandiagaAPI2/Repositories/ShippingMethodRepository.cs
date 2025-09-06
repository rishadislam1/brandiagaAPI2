using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace brandiagaAPI2.Repositories
{
    public class ShippingMethodRepository : IShippingMethodRepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public ShippingMethodRepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }

        public async Task<ShippingMethod> GetShippingMethodByIdAsync(Guid shippingMethodId)
        {
            return await _context.ShippingMethods
                .FirstOrDefaultAsync(sm => sm.ShippingMethodId == shippingMethodId);
        }

        public async Task<IEnumerable<ShippingMethod>> GetAllShippingMethodsAsync()
        {
            return await _context.ShippingMethods
                .ToListAsync();
        }

        public async Task AddShippingMethodAsync(ShippingMethod shippingMethod)
        {
            await _context.ShippingMethods.AddAsync(shippingMethod);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateShippingMethodAsync(ShippingMethod shippingMethod)
        {
            _context.ShippingMethods.Update(shippingMethod);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShippingMethodAsync(Guid shippingMethodId)
        {
            var shippingMethod = await _context.ShippingMethods.FindAsync(shippingMethodId);
            if (shippingMethod != null)
            {
                _context.ShippingMethods.Remove(shippingMethod);
                await _context.SaveChangesAsync();
            }
        }
    }
}
