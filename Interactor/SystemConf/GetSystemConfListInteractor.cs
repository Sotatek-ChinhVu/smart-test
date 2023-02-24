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
            var result = _systemConfRepository.GetListByGrpCd(inputData.HpId, inputData.GrpItemList.Select(item => new SystemConfModel(item.GrpCd, item.GrpEdaNo)).ToList());
            return new GetSystemConfListOutputData(result, GetSystemConfListStatus.Successed);
        }
        finally
        {
            _systemConfRepository.ReleaseResource();
        }
    }
}
