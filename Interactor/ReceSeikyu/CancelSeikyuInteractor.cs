using Domain.Models.Receipt;
using Domain.Models.ReceSeikyu;
using Interactor.CalculateService;
using UseCase.MedicalExamination.Calculate;
using UseCase.ReceSeikyu.CancelSeikyu;

namespace Interactor.ReceSeikyu;

public class CancelSeikyuInteractor : ICancelSeikyuInputPort
{
    private readonly IReceSeikyuRepository _receSeikyuRepository;
    private readonly IReceiptRepository _receiptRepository;
    private readonly ICalculateService _calculateRepository;

    public CancelSeikyuInteractor(IReceSeikyuRepository receSeikyuRepository, ICalculateService calculateRepository, IReceiptRepository receiptRepository)
    {
        _receSeikyuRepository = receSeikyuRepository;
        _calculateRepository = calculateRepository;
        _receiptRepository = receiptRepository;
    }

    public CancelSeikyuOutputData Handle(CancelSeikyuInputData inputData)
    {
        try
        {
            var receCheckItem = _receiptRepository.GetReceInf(inputData.HpId, inputData.SeikyuYm, inputData.PtId, inputData.SinYm, inputData.HokenId);
            if (receCheckItem.PtId == 0)
            {
                return new CancelSeikyuOutputData(CancelSeikyuStatus.InvalidInputItem);
            }
            var seikyuKbn = inputData.SeikyuKbn == 0 ? 1 : inputData.SeikyuKbn;
            int seqNo = _receSeikyuRepository.InsertNewReceSeikyu(new ReceSeikyuModel(inputData.PtId, seikyuKbn, 999999, inputData.SinYm, inputData.HokenId), inputData.UserId, inputData.HpId);
            if (seqNo == 0)
            {
                return new CancelSeikyuOutputData(CancelSeikyuStatus.Failed);
            }
            var receSeikyuDuplicate = _receSeikyuRepository.GetReceSeikyuDuplicate(inputData.HpId, inputData.PtId, inputData.SinYm, inputData.HokenId);
            if (receSeikyuDuplicate.PtId > 0)
            {
                receSeikyuDuplicate.UpdateReceSeikyuModel(1);
                _receSeikyuRepository.UpdateReceSeikyu(new List<ReceSeikyuModel>() { receSeikyuDuplicate }, inputData.UserId, inputData.HpId);
            }
            _calculateRepository.ReceFutanCalculateMain(new ReceCalculateRequest(new List<long>() { inputData.PtId }, inputData.SeikyuYm, string.Empty), CancellationToken.None);
            return new CancelSeikyuOutputData(CancelSeikyuStatus.Successed);
        }
        finally
        {
            _receSeikyuRepository.ReleaseResource();
            _calculateRepository.ReleaseSource();
            _receiptRepository.ReleaseResource();
        }
    }
}
