using Domain.Common;

namespace Domain.Models.Cacche
{
    public interface IRemoveCacheRepository : IRepositoryBase
    {
        bool RemoveCache(string keyCache);
    }
}
