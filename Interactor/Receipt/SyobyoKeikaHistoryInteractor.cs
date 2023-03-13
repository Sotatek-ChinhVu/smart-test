using Domain.Models.Insurance;
using Domain.Models.Receipt;
using Helper.Common;
using UseCase.Receipt.SyobyoKeikaHistory;

namespace Interactor.Receipt;

public class SyobyoKeikaHistoryInteractor : ISyobyoKeikaHistoryInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IInsuranceRepository _insuranceRepository;

    public SyobyoKeikaHistoryInteractor(IReceiptRepository receiptRepository, IInsuranceRepository insuranceRepository)
    {
        _receiptRepository = receiptRepository;
        _insuranceRepository = insuranceRepository;
    }

    public SyobyoKeikaHistoryOutputData Handle(SyobyoKeikaHistoryInputData inputData)
    {
        try
        {
            var insuranceData = _insuranceRepository.GetInsuranceListById(inputData.HpId, inputData.PtId, 0);
            var hokenInfList = insuranceData.ListHokenInf;
            var SyobyoKeikaList = _receiptRepository.GetSyobyoKeikaList(inputData.HpId, 0, inputData.PtId, 0);

            var result = ConvertToResult(hokenInfList, SyobyoKeikaList);
            return new SyobyoKeikaHistoryOutputData(result, SyobyoKeikaHistoryStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
        }
    }

    private List<SyobyoKeikaHistoryOutputItem> ConvertToResult(List<HokenInfModel> hokenInfList, List<SyobyoKeikaModel> SyobyoKeikaList)
    {
        List<SyobyoKeikaHistoryOutputItem> result = new();
        var sinYmList = SyobyoKeikaList.Select(item => item.SinYm).Distinct().OrderByDescending(item => item).ToList();
        foreach (var sinYm in sinYmList)
        {
            var hokenId = SyobyoKeikaList.FirstOrDefault(item => item.SinYm == sinYm)?.HokenId ?? 0;
            var outputItem = new SyobyoKeikaHistoryOutputItem(
                                 sinYm,
                                 CIUtil.SMonthToShowSWMonth(sinYm, 1),
                                 hokenId,
                                 GetHokenName(hokenId, hokenInfList),
                                 SyobyoKeikaList.Where(item => item.SinYm == sinYm).ToList());
            result.Add(outputItem);
        }
        return result;
    }

    private string GetHokenName(int hokenId, List<HokenInfModel> hokenInfList)
    {
        return hokenInfList.FirstOrDefault(p => p.HokenId == hokenId)?.HokenSentaku ?? string.Empty;
    }
}
