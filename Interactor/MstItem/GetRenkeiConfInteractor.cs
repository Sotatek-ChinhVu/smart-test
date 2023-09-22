using Domain.Models.MstItem;
using UseCase.MstItem.GetRenkeiConf;

namespace Interactor.MstItem;

public class GetRenkeiConfInteractor : IGetRenkeiConfInputPort
{
    private readonly IMstItemRepository _mstItemRepository;

    public GetRenkeiConfInteractor(IMstItemRepository mstItemRepository)
    {
        _mstItemRepository = mstItemRepository;
    }

    public GetRenkeiConfOutputData Handle(GetRenkeiConfInputData inputData)
    {
        try
        {
            var result = _mstItemRepository.GetRenkeiConfModels(inputData.HpId, inputData.RenkeiSbt);
            return new GetRenkeiConfOutputData(result, GetRenkeiConfStatus.Successed);
        }
        finally
        {
            _mstItemRepository.ReleaseResource();
        }
    }
}
