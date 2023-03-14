using Domain.Models.Receipt;
using UseCase.Receipt.DoReceCmt;

namespace Interactor.Receipt;

public class DoReceCmtInteractor : IDoReceCmtInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public DoReceCmtInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public DoReceCmtOutputData Handle(DoReceCmtInputData inputData)
    {
        try
        {
            List<ReceCmtModel> result = new();
            var allHistoryReceCmtList = _receiptRepository.GetReceCmtList(inputData.HpId, 0, inputData.PtId, 0);

            var allLatestReceCmtList = _receiptRepository.GetLastMonthReceCmt(inputData.HpId, inputData.SinYm * 100 + 1, inputData.PtId);
            var lastReceCmt = allLatestReceCmtList.GroupBy(item => new { item.SinYm, item.HokenId })
                                                           .Select(item => item.FirstOrDefault())
                                                           .OrderByDescending(item => item?.HokenId)
                                                           .ThenByDescending(item => item?.SinYm)
                                                           .FirstOrDefault();

            var lastCmtByHoken = allHistoryReceCmtList.Where(item => item.HokenId == inputData.HokenId
                                                                     && item.SinYm < inputData.SinYm)
                                                      .DefaultIfEmpty()
                                                      ?.MaxBy(item => item?.SinYm);

            if (lastCmtByHoken != null)
            {
                result = allHistoryReceCmtList.Where(item => item.HokenId == lastCmtByHoken.HokenId && item.SinYm == lastCmtByHoken.SinYm).ToList();
            }
            else if (lastReceCmt != null)
            {
                result = allLatestReceCmtList.Where(item => item.HokenId == lastReceCmt.HokenId && item.SinYm == lastReceCmt.SinYm).ToList();
            }
            return new DoReceCmtOutputData(result, DoReceCmtStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
