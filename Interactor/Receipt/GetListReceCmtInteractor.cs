using Domain.Models.Receipt;
using UseCase.Receipt.GetReceCmt;

namespace Interactor.Receipt;

public class GetListReceCmtInteractor : IGetListReceCmtInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetListReceCmtInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetListReceCmtOutputData Handle(GetListReceCmtInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetListReceCmt(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
            return new GetListReceCmtOutputData(result, GetListReceCmtStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
