using Domain.Models.Cacche;
using UseCase.Cache;

namespace Interactor.Cache;

public class RemoveCacheInteractor : IRemoveCacheInputPort
{
    private readonly IRemoveCacheRepository _removeCacheRepository;

    public RemoveCacheInteractor(IRemoveCacheRepository removeCacheRepository)
    {
        _removeCacheRepository = removeCacheRepository;
    }
    public RemoveCacheOutputData Handle(RemoveCacheInputData inputData)
    {
        try
        {
            var result = _removeCacheRepository.RemoveCache(inputData.Key);

            if (result)
            {
                return new RemoveCacheOutputData(RemoveCacheStaus.Successed);
            }
            else
            {
                return new RemoveCacheOutputData(RemoveCacheStaus.Failed);
            }
        }
        finally 
        {
            _removeCacheRepository.ReleaseResource();
        }
    }
}
