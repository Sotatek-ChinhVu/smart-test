using Domain.Models.MstItem;
using UseCase.MstItem.GetRenkeiTiming;

namespace Interactor.MstItem;

public class GetRenkeiTimingInteractor : IGetRenkeiTimingInputPort
{
    private readonly IMstItemRepository _mstItemRepository;

    public GetRenkeiTimingInteractor(IMstItemRepository mstItemRepository)
    {
        _mstItemRepository = mstItemRepository;
    }

    public GetRenkeiTimingOutputData Handle(GetRenkeiTimingInputData inputData)
    {
        try
        {
            var result = _mstItemRepository.GetRenkeiTimingModel(inputData.HpId, inputData.RenkeiId);
            return new GetRenkeiTimingOutputData(GetRenkeiTimingStatus.Successed, result);
        }
        finally
        {
            _mstItemRepository.ReleaseResource();
        }
    }
}
