using Domain.Models.Receipt;
using UseCase.Receipt.GetListSyoukiInf;

namespace Interactor.Receipt;

public class GetSyoukiInfListInteractor : IGetSyoukiInfListInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetSyoukiInfListInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }
    public GetSyoukiInfListOutputData Handle(GetSyoukiInfListInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetListSyoukiInf(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
            var listSyoukiKbnMst = _receiptRepository.GetListSyoukiKbnMst(inputData.SinYm);
            return new GetSyoukiInfListOutputData(result, listSyoukiKbnMst, GetSyoukiInfListStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
