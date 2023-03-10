using Domain.Models.Insurance;
using Domain.Models.Receipt;
using Helper.Common;
using UseCase.Receipt.HistorySyoukiInf;

namespace Interactor.Receipt;

public class HistorySyoukiInfInteractor : IHistorySyoukiInfInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IInsuranceRepository _insuranceRepository;

    public HistorySyoukiInfInteractor(IReceiptRepository receiptRepository, IInsuranceRepository insuranceRepository)
    {
        _receiptRepository = receiptRepository;
        _insuranceRepository = insuranceRepository;
    }

    public HistorySyoukiInfOutputData Handle(HistorySyoukiInfInputData inputData)
    {
        try
        {
            var insuranceData = _insuranceRepository.GetInsuranceListById(inputData.HpId, inputData.PtId, 0);
            var hokenInfList = insuranceData.ListHokenInf;
            var syoukiInfList = _receiptRepository.GetSyoukiInfList(inputData.HpId, 0, inputData.PtId, 0);
            var syoukiKbnList = _receiptRepository.GetSyoukiKbnMstList(inputData.SinYm);

            var result = ConvertToResult(hokenInfList, syoukiInfList, syoukiKbnList);
            return new HistorySyoukiInfOutputData(result, HistorySyoukiInfStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
        }
    }

    private List<HistorySyoukiInfOutputItem> ConvertToResult(List<HokenInfModel> hokenInfList, List<SyoukiInfModel> syoukiInfList, List<SyoukiKbnMstModel> syoukiKbnList)
    {
        List<HistorySyoukiInfOutputItem> result = new();
        var sinYmList = syoukiInfList.Select(item => item.SinYm).Distinct().OrderByDescending(item => item).ToList();
        foreach (var sinYm in sinYmList)
        {
            var hokenId = syoukiInfList.FirstOrDefault(item => item.SinYm == sinYm)?.HokenId ?? 0;
            var outputItem = new HistorySyoukiInfOutputItem(
                                 sinYm,
                                 CIUtil.SMonthToShowSWMonth(sinYm, 1),
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
