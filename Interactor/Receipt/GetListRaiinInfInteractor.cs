using Domain.Models.Receipt;
using UseCase.Receipt.GetListRaiinInf;

namespace Interactor.Receipt;

public class GetListRaiinInfInteractor : IGetListRaiinInfInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetListRaiinInfInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetListRaiinInfOutputData Handle(GetListRaiinInfInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetListRaiinInf(inputData.HpId, inputData.PtId, inputData.SinYm, inputData.DayInMonth, inputData.RpNo, inputData.SeqNo);
            return new GetListRaiinInfOutputData(result, GetListRaiinInfStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
