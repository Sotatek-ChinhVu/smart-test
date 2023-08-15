using Domain.Models.Receipt;
using UseCase.Receipt.GetListKaikeiInf;

namespace Interactor.Receipt;

public class GetListKaikeiInfInteractor : IGetListKaikeiInfInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetListKaikeiInfInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetListKaikeiInfOutputData Handle(GetListKaikeiInfInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetListKaikeiInf(inputData.HpId, inputData.SinYm, inputData.PtId);
            return new GetListKaikeiInfOutputData(result, GetListKaikeiInfStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
