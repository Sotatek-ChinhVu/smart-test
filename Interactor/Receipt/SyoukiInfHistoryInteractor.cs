using Domain.Models.Insurance;
using Domain.Models.Receipt;
using Helper.Common;
using UseCase.Receipt.SyoukiInfHistory;

namespace Interactor.Receipt;

public class SyoukiInfHistoryInteractor : ISyoukiInfHistoryInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IInsuranceRepository _insuranceRepository;

    public SyoukiInfHistoryInteractor(IReceiptRepository receiptRepository, IInsuranceRepository insuranceRepository)
    {
        _receiptRepository = receiptRepository;
        _insuranceRepository = insuranceRepository;
    }

    public SyoukiInfHistoryOutputData Handle(SyoukiInfHistoryInputData inputData)
    {
        try
        {
            var insuranceData = _insuranceRepository.GetInsuranceListById(inputData.HpId, inputData.PtId, 0);
            var hokenInfList = insuranceData.ListHokenInf;
            var syoukiInfList = _receiptRepository.GetSyoukiInfList(inputData.HpId, 0, inputData.PtId, 0);
            var syoukiKbnList = _receiptRepository.GetSyoukiKbnMstList(inputData.SinYm);

            var result = ConvertToResult(hokenInfList, syoukiInfList, syoukiKbnList);
            return new SyoukiInfHistoryOutputData(result, SyoukiInfHistoryStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
        }
    }

    private List<SyoukiInfHistoryOutputItem> ConvertToResult(List<HokenInfModel> hokenInfList, List<SyoukiInfModel> syoukiInfList, List<SyoukiKbnMstModel> syoukiKbnList)
    {
        List<SyoukiInfHistoryOutputItem> result = new();
        var sinYmList = syoukiInfList.Select(item => item.SinYm).Distinct().OrderByDescending(item => item).ToList();
        foreach (var sinYm in sinYmList)
        {
            var hokenId = syoukiInfList.FirstOrDefault(item => item.SinYm == sinYm)?.HokenId ?? 0;
            var outputItem = new SyoukiInfHistoryOutputItem(
                                 sinYm,
                                 CIUtil.SMonthToShowSWMonth(sinYm, 1),
                                 hokenId,
                                 GetHokenName(hokenId, hokenInfList),
                                 syoukiInfList.Where(item => item.SinYm == sinYm).ToList(),
                                 syoukiKbnList);
            result.Add(outputItem);
        }
        return result;
    }

    private string GetHokenName(int hokenId, List<HokenInfModel> hokenInfList)
    {
        return hokenInfList.FirstOrDefault(p => p.HokenId == hokenId)?.HokenSentaku ?? string.Empty;
    }
}
