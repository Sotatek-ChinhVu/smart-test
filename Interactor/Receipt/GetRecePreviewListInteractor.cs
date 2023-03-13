using Domain.Models.Receipt;
using UseCase.Receipt.GetRecePreviewList;

namespace Interactor.Receipt;

public class GetRecePreviewListInteractor : IGetRecePreviewListInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetRecePreviewListInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetRecePreviewListOutputData Handle(GetRecePreviewListInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetReceInf(inputData.HpId, inputData.TypeReceiptPreview, inputData.PtId);
            return new GetRecePreviewListOutputData(result, GetRecePreviewListStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}