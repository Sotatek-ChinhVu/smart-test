using Domain.Models.Receipt;
using UseCase.Receipt.Recalculation;

namespace Interactor.Receipt;

public class RecalculationInteractor : IRecalculationInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public RecalculationInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public RecalculationOutputData Handle(RecalculationInputData inputData)
    {
        try
        {
            string errorMessage = string.Empty;
            var list = _receiptRepository.GetReceRecalculationList(inputData.HpId, inputData.SinYm, inputData.PtIdList);
            return new RecalculationOutputData(RecalculationStatus.Successed, errorMessage);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
