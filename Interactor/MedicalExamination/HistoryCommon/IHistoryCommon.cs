using Domain.Models.HistoryOrder;
using UseCase.MedicalExamination.GetHistory;

namespace Interactor.MedicalExamination.HistoryCommon;

public interface IHistoryCommon
{
    GetMedicalExaminationHistoryOutputData GetHistoryOutput(int hpId, long ptId, int sinDate, (int, List<HistoryOrderModel>) historyList);

    void ReleaseResources();
}
