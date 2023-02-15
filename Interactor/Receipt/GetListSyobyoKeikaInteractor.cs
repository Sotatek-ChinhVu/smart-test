using Domain.Models.Receipt;
using UseCase.Receipt.GetListSyobyoKeika;

namespace Interactor.Receipt;

public class GetListSyobyoKeikaInteractor : IGetListSyobyoKeikaInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetListSyobyoKeikaInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetListSyobyoKeikaOutputData Handle(GetListSyobyoKeikaInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetListSyobyoKeika(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
            return new GetListSyobyoKeikaOutputData(result, GetListSyobyoKeikaStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}