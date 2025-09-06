using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface IApikeyRepository
    {
        Task<Apikey> GetApikeyByKeyAsync(string keyValue);
    }
}
