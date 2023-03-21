using Domain.Models.HistoryOrder;
using Interactor.MedicalExamination.HistoryCommon;
using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHistory;

namespace Interactor.MedicalExamination;

public class GetDataPrintKarte2Interactor : IGetDataPrintKarte2InputPort
{
    private readonly IHistoryCommon _historyCommon;
    private readonly IHistoryOrderRepository _historyOrderRepository;

    public GetDataPrintKarte2Interactor(IHistoryCommon historyCommon, IHistoryOrderRepository historyOrderRepository)
    {
        _historyCommon = historyCommon;
        _historyOrderRepository = historyOrderRepository;
    }
    public GetMedicalExaminationHistoryOutputData Handle(GetDataPrintKarte2InputData inputData)
    {
        try
        {
            (int, List<HistoryOrderModel>) historyList = _historyOrderRepository.GetList(
                inputData.HpId,
                inputData.UserId,
                inputData.PtId,
                inputData.SinDate,
                inputData.Offset,
                inputData.Limit,
                (int)inputData.FilterId,
                inputData.DeleteConditon);
            return _historyCommon.GetHistoryOutput(inputData.HpId, inputData.PtId, inputData.SinDate, historyList);
        }
        finally
        {
            _historyCommon.ReleaseResources();
        }
    }
}
