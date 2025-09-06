using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace brandiagaAPI2.Repositories
{
    public class ApikeyRepository : IApikeyRepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public ApikeyRepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }

        public async Task<Apikey> GetApikeyByKeyAsync(string keyValue)
        {
            return await _context.Apikeys
                .FirstOrDefaultAsync(a => a.KeyValue == keyValue);
        }
    }
}
