namespace Domain.Models.Cacche
{
    public interface IRemoveCacheRepository
    {
        bool RemoveCache(string keyCache);

        void RemoveAllCache();
    }
}
