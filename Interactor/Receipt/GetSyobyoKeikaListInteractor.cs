using Domain.Models.Receipt;
using UseCase.Receipt.GetListSyobyoKeika;

namespace Interactor.Receipt;

public class GetSyobyoKeikaListInteractor : IGetSyobyoKeikaListInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetSyobyoKeikaListInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetSyobyoKeikaListOutputData Handle(GetSyobyoKeikaListInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetListSyobyoKeika(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
            return new GetSyobyoKeikaListOutputData(result, GetSyobyoKeikaListStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}