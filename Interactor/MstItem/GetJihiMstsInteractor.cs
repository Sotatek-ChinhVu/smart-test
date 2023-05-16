using Domain.Models.MstItem;
using UseCase.MstItem.GetJihiSbtMstList;

namespace Interactor.Byomei;

public class GetJihiMstsInteractor : IGetJihiSbtMstListInputPort
{
    private readonly IMstItemRepository _inputItemRepository;

    public GetJihiMstsInteractor(IMstItemRepository inputItemRepository)
    {
        _inputItemRepository = inputItemRepository;
    }

    public GetJihiSbtMstListOutputData Handle(GetJihiSbtMstListInputData inputData)
    {
        try
        {
            if (inputData.HpId < 1)
            {
                return new GetJihiSbtMstListOutputData(GetJihiSbtMstListStatus.InvalidHpId, new());
            }

            var listData = _inputItemRepository.GetJihiSbtMstList(inputData.HpId);
            return new GetJihiSbtMstListOutputData(GetJihiSbtMstListStatus.Successed, listData);
        }
        finally
        {
            _inputItemRepository.ReleaseResource();
        }
    }
}