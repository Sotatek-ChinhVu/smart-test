using Domain.Models.Insurance;
using Domain.Models.Receipt;
using Helper.Common;
using UseCase.Receipt;
using UseCase.Receipt.HistoryReceCmt;

namespace Interactor.Receipt;

public class HistoryReceCmtInteractor : IHistoryReceCmtInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IInsuranceRepository _insuranceRepository;

    public HistoryReceCmtInteractor(IReceiptRepository receiptRepository, IInsuranceRepository insuranceRepository)
    {
        _receiptRepository = receiptRepository;
        _insuranceRepository = insuranceRepository;
    }

    public HistoryReceCmtOutputData Handle(HistoryReceCmtInputData inputData)
    {
        try
        {
            var insuranceData = _insuranceRepository.GetInsuranceListById(inputData.HpId, inputData.PtId, 0);
            var hokenInfList = insuranceData.ListHokenInf;
            var receCmtList = _receiptRepository.GetReceCmtList(inputData.HpId, 0, inputData.PtId, 0);

            var result = ConvertToResult(hokenInfList, receCmtList);
            return new HistoryReceCmtOutputData(result, HistoryReceCmtStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
        }
    }

    private List<HistoryReceCmtOutputItem> ConvertToResult(List<HokenInfModel> hokenInfList, List<ReceCmtModel> receCmtList)
    {
        List<HistoryReceCmtOutputItem> result = new();
        var sinYmList = receCmtList.Select(item => item.SinYm).Distinct().OrderByDescending(item => item).ToList();
        foreach (var sinYm in sinYmList)
        {
            var receCmtOutputList = receCmtList.Where(item => item.SinYm == sinYm)
                                               .Select(item => new ReceCmtItem(item))
                                               .ToList();

            var hokenId = receCmtList.FirstOrDefault(item => item.SinYm == sinYm)?.HokenId ?? 0;
            var outputItem = new HistoryReceCmtOutputItem(
                    sinYm,
                    CIUtil.SMonthToShowSWMonth(sinYm, 1),
                    GetHokenName(hokenId, hokenInfList),
                    receCmtOutputList
                );
            result.Add(outputItem);
        }
        return result;
    }

    private string GetHokenName(int hokenId, List<HokenInfModel> hokenInfList)
    {
        return hokenInfList.FirstOrDefault(p => p.HokenId == hokenId)?.HokenSentaku ?? string.Empty;
    }
}
