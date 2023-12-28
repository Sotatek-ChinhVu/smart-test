using Domain.Models.Cacche;
using UseCase.Cache.RemoveAllCache;

namespace Interactor.Cache
{
    public class RemoveAllCacheInteractor : IRemoveAllCacheInputPort
    {
        private readonly IRemoveCacheRepository _removeCacheRepository;

        public RemoveAllCacheInteractor(IRemoveCacheRepository removeCacheRepository)
        {
            _removeCacheRepository = removeCacheRepository;
        }
        public RemoveAllCacheOutputData Handle(RemoveAllCacheInputData inputData)
        {
            try
            {
                _removeCacheRepository.RemoveAllCache();

                return new RemoveAllCacheOutputData(RemoveAllCacheStaus.Successed);
            }
            finally
            {
                _removeCacheRepository.ReleaseResource();
            }
        }
    }
}
