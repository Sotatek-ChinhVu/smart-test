using Domain.Models.HistoryOrder;
using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHistory;

namespace Interactor.MedicalExamination.HistoryCommon;

public interface IHistoryCommon
{
    GetMedicalExaminationHistoryOutputData GetHistoryOutput(int hpId, long ptId, int sinDate, (int totalCount, List<HistoryOrderModel> historyOrderModelList) historyList);

    void FilterData(ref List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiinItems, GetDataPrintKarte2InputData inputData);

    GetMedicalExaminationHistoryOutputData GetDataKarte2(GetDataPrintKarte2InputData inputData);

    void ReleaseResources();
}
