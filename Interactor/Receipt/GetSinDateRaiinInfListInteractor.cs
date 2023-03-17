using Domain.Models.Receipt;
using UseCase.Receipt.GetSinDateRaiinInfList;

namespace Interactor.Receipt;

public class GetSinDateRaiinInfListInteractor : IGetSinDateRaiinInfListInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetSinDateRaiinInfListInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetSinDateRaiinInfListOutputData Handle(GetSinDateRaiinInfListInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetSinDateRaiinInfList(inputData.HpId, inputData.PtId, inputData.SinYm, inputData.HokenId);
            return new GetSinDateRaiinInfListOutputData(result, GetSinDateRaiinInfListStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
