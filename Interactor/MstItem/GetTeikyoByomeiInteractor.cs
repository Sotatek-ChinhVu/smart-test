using Domain.Models.MstItem;
using UseCase.MstItem.GetTeikyoByomei;

namespace Interactor.MstItem;

public class GetTeikyoByomeiInteractor : IGetTeikyoByomeiInputPort
{
    private readonly IMstItemRepository _mstItemRepository;

    public GetTeikyoByomeiInteractor(IMstItemRepository mstItemRepository)
    {
        _mstItemRepository = mstItemRepository;
    }

    public GetTeikyoByomeiOutputData Handle(GetTeikyoByomeiInputData inputData)
    {
        var teikyoByomeis = _mstItemRepository.GetTeikyoByomeiModel(inputData.HpId, inputData.ItemCd);
        return new GetTeikyoByomeiOutputData(teikyoByomeis, GetTeikyoByomeiStatus.Successful);
    }
}
