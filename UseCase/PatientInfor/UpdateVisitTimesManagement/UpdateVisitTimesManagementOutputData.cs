using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.UpdateVisitTimesManagement;

public class UpdateVisitTimesManagementOutputData : IOutputData
{
    public UpdateVisitTimesManagementOutputData(UpdateVisitTimesManagementStatus status)
    {
        Status = status;
    }

    public UpdateVisitTimesManagementStatus Status { get; private set; }
}
