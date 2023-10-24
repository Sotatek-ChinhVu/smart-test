using Domain.Models.MstItem;
using UseCase.MstItem.GetTenMstList;

namespace Interactor.MstItem;

public class GetTenMstListInteractor : IGetTenMstListInputPort
{
    private readonly IMstItemRepository _mstItemRepository;

    public GetTenMstListInteractor(IMstItemRepository mstItemRepository)
    {
        _mstItemRepository = mstItemRepository;
    }

    public GetTenMstListOutputData Handle(GetTenMstListInputData inputData)
    {
        try
        {
            var result = _mstItemRepository.GetTenMstList(inputData.HpId, inputData.ItemCdList);
            result = result.Where(item => item.IsDeleted == 0
                                          && item.StartDate <= inputData.SinDate
                                          && item.EndDate >= inputData.SinDate)
                           .ToList();
            return new GetTenMstListOutputData(result, GetTenMstListStatus.Successed);
        }
        finally
        {
            _mstItemRepository.ReleaseResource();
        }
    }
}
