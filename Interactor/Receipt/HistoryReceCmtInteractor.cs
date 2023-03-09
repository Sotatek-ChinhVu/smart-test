using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.Receipt;
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
            var receCmtList = _receiptRepository.GetReceCmtList(inputData.HpId, inputData.PtId);

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
        var sinYmList = receCmtList.Select(item => item.SinYm).Distinct().ToList();
        foreach (var sinYm in sinYmList)
        {
            var receCmtOutputList = receCmtList.Where(item => item.SinYm == sinYm).ToList();
        }



        return result;
    }

    private string GetHokenName(List<HokenInfModel> hokenInfList, List<ReceCmtModel> receCmtList)
    {
        foreach (var cmt in receCmtList)
        {
            str = GetHokenNameById(cmt.HokenId, hokenInfList);
        }
    }

    private string GetHokenNameById(int hokenId, List<HokenInfModel> hokenInfList)
    {
        return hokenInfList.FirstOrDefault(p => p.HokenId == hokenId)?.HokenSentaku ?? string.Empty;
    }
}
