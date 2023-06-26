using Domain.Models.HistoryOrder;
using Domain.Models.Receipt.Recalculation;
using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHistory;

namespace Interactor.MedicalExamination.HistoryCommon;

public interface IHistoryCommon
{
    GetMedicalExaminationHistoryOutputData GetHistoryOutput(int hpId, long ptId, int sinDate, (int totalCount, List<HistoryOrderModel> historyOrderModelList) historyList, List<SinKouiListModel> sinkouiList);

    void FilterData(ref List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiinItems, GetDataPrintKarte2InputData inputData);

    GetMedicalExaminationHistoryOutputData GetDataKarte2(GetDataPrintKarte2InputData inputData);

    void ReleaseResources();
}
