using Domain.Models.HistoryOrder;
using UseCase.MedicalExamination.GetHistory;

namespace Interactor.MedicalExamination.HistoryCommon;

public interface IHistoryCommon
{
    GetMedicalExaminationHistoryOutputData GetHistoryOutput(int hpId, long ptId, int sinDate, (int totalCount, List<HistoryOrderModel> historyOrderModelList) historyList);

    void ReleaseResources();
}
