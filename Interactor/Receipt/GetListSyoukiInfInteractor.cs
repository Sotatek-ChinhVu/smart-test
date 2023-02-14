using Domain.Models.Receipt;
using UseCase.Receipt.GetListSyoukiInf;

namespace Interactor.Receipt;

public class GetListSyoukiInfInteractor : IGetListSyoukiInfInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetListSyoukiInfInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }
    public GetListSyoukiInfOutputData Handle(GetListSyoukiInfInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetListSyoukiInf(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
            var listSyoukiKbnMst = _receiptRepository.GetListSyoukiKbnMst(inputData.SinYm);
            return new GetListSyoukiInfOutputData(result, listSyoukiKbnMst, GetListSyoukiInfStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
