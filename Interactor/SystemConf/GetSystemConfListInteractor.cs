using Domain.Models.SystemConf;
using UseCase.SystemConf.GetSystemConfList;

namespace Interactor.SystemConf;

public class GetSystemConfListInteractor : IGetSystemConfListInputPort
{
    private readonly ISystemConfRepository _systemConfRepository;

    public GetSystemConfListInteractor(ISystemConfRepository systemConfRepository)
    {
        _systemConfRepository = systemConfRepository;
    }

    public GetSystemConfListOutputData Handle(GetSystemConfListInputData inputData)
    {
        try
        {
            var result = _systemConfRepository.GetAllSystemConfig(inputData.HpId);
            return new GetSystemConfListOutputData(result, GetSystemConfListStatus.Successed);
        }
        finally
        {
            _systemConfRepository.ReleaseResource();
        }
    }
}
