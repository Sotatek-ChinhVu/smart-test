using Domain.Models.Reception;
using UseCase.Reception.GetOutDrugOrderList;

namespace Interactor.Reception;

public class GetOutDrugOrderListInteractor : IGetOutDrugOrderListInputPort
{
    private readonly IReceptionRepository _raiinInfRepository;

    public GetOutDrugOrderListInteractor(IReceptionRepository raiinInfRepository)
    {
        _raiinInfRepository = raiinInfRepository;
    }

    public GetOutDrugOrderListOutputData Handle(GetOutDrugOrderListInputData inputData)
    {
        try
        {
            var result = _raiinInfRepository.GetOutDrugOrderList(inputData.HpId, inputData.FromDate, inputData.ToDate);
            return new GetOutDrugOrderListOutputData(result, GetOutDrugOrderListStatus.Successed);
        }
        finally
        {
            _raiinInfRepository.ReleaseResource();
        }
    }
}
