using Domain.Models.Receipt;
using UseCase.Receipt.GetReceStatus;

namespace Interactor.Receipt;

public class GetReceStatusInteractor : IGetReceStatusInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetReceStatusInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetReceStatusOutputData Handle(GetReceStatusInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetReceStatus(inputData.HpId, inputData.PtId, inputData.SeikyuYm, inputData.SinYm, inputData.HokenId);
            return new GetReceStatusOutputData(GetReceStatusStatus.Successed, result);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
