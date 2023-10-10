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
            var renkeiConfList = _mstItemRepository.GetRenkeiConfModels(inputData.HpId, inputData.RenkeiSbt);
            List<RenkeiMstModel> renkeiMstModelList = inputData.NotLoadMst ? new() : _mstItemRepository.GetRenkeiMstModels(inputData.HpId);
            List<RenkeiTemplateMstModel> renkeiTemplateMstModelList = inputData.NotLoadMst ? new() : _mstItemRepository.GetRenkeiTemplateMstModels(inputData.HpId);
            return new GetRenkeiConfOutputData(renkeiConfList, renkeiMstModelList, renkeiTemplateMstModelList, GetRenkeiConfStatus.Successed);
        }
        finally
        {
            _mstItemRepository.ReleaseResource();
        }
    }
}
