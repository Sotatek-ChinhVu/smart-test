using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.UpdateVisitTimesManagementNeedSave;

public class UpdateVisitTimesManagementNeedSaveOutputData : IOutputData
{
    public UpdateVisitTimesManagementNeedSaveOutputData(UpdateVisitTimesManagementNeedSaveStatus status)
    {
        Status = status;
    }

    public UpdateVisitTimesManagementNeedSaveStatus Status { get; private set; }
}
