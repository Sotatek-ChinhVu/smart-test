using Domain.Models.Receipt;
using UseCase.Receipt;
using UseCase.Receipt.ReceiptEdit;

namespace Interactor.Receipt;

public class GetReceiptEditInteractor : IGetReceiptEditInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetReceiptEditInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetReceiptEditOutputData Handle(GetReceiptEditInputData inputData)
    {
        try
        {
            ReceiptEditItem receiptEditOrigin = new();
            ReceiptEditItem receiptEditCurrent = new();
            var tokkiMstDictionary = _receiptRepository.GetTokkiMstDictionary(inputData.HpId);

            var receInfEdit = _receiptRepository.GetReceInfEdit(inputData.HpId, inputData.SeikyuYm, inputData.PtId, inputData.SinYm, inputData.HokenId);
            var receInfPreEdit = _receiptRepository.GetReceInfPreEdit(inputData.HpId, inputData.SeikyuYm, inputData.PtId, inputData.SinYm, inputData.HokenId);
            var receInf = _receiptRepository.GetReceInf(inputData.HpId, inputData.SeikyuYm, inputData.PtId, inputData.SinYm, inputData.HokenId);
            int seqNo = 0;

            if (receInf.PtId > 0)
            {
                if (receInfPreEdit.SeqNo == 0)
                {
                    receiptEditOrigin = new ReceiptEditItem(receInf);
                }
                else
                {
                    receiptEditOrigin = new ReceiptEditItem(receInfPreEdit);
                }

                if (receInfEdit.SeqNo == 0)
                {
                    receiptEditCurrent = new ReceiptEditItem(receInf);
                }
                else
                {
                    seqNo = receInfEdit.SeqNo;
                    receiptEditCurrent = new ReceiptEditItem(receInfEdit);
                }
            }
            return new GetReceiptEditOutputData(seqNo, receiptEditOrigin, receiptEditCurrent, tokkiMstDictionary, GetReceiptEditStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}