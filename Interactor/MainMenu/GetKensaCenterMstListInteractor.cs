using Domain.Models.MstItem;
using UseCase.MainMenu.GetKensaCenterMstList;

namespace Interactor.MainMenu;

public class GetKensaCenterMstListInteractor : IGetKensaCenterMstListInputPort
{
    private readonly IMstItemRepository _mstItemRepository;

    public GetKensaCenterMstListInteractor(IMstItemRepository mstItemRepository)
    {
        _mstItemRepository = mstItemRepository;
    }

    public GetKensaCenterMstListOutputData Handle(GetKensaCenterMstListInputData inputData)
    {
        try
        {
            var result = _mstItemRepository.GetListKensaCenterMst(inputData.HpId);
            return new GetKensaCenterMstListOutputData(result, GetKensaCenterMstListStatus.Successed);
        }
        finally
        {
            _mstItemRepository.ReleaseResource();
        }
    }
}
