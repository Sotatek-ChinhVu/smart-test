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
            var insuranceList = _insuranceRepository.GetInsuranceListById(inputData.HpId, inputData.PtId, 0).ListInsurance;
            var receCmtList = _receiptRepository.GetReceCmtList(inputData.HpId, inputData.PtId);

            var result = ConvertToResult();
            return new HistoryReceCmtOutputData(result, HistoryReceCmtStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
        }
    }

    private List<HistoryReceCmtOutputItem> ConvertToResult(List<InsuranceModel> insuranceList, List<ReceCmtModel> receCmtList)
    {
        List<HistoryReceCmtOutputItem> result = new();
    }
}
