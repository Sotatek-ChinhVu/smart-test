using Domain.Models.Receipt;
using UseCase.Receipt.GetReceCmt;

namespace Interactor.Receipt;

public class GetReceCmtListInteractor : IGetReceCmtListInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetReceCmtListInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetReceCmtListOutputData Handle(GetReceCmtListInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetListReceCmt(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
            return new GetReceCmtListOutputData(result, GetReceCmtListStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
