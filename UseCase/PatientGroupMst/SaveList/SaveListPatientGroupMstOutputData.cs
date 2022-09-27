using UseCase.Core.Sync.Core;

namespace UseCase.PatientGroupMst.SaveList;

public class SaveListPatientGroupMstOutputData : IOutputData
{
    public SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus status)
    {
        Status = status;
    }

    public SaveListPatientGroupMstStatus Status { get; private set; }
}
