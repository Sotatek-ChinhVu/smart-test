using UseCase.Core.Sync.Core;

namespace UseCase.PatientGroupMst.SaveList;

public class SaveListPatientGroupMstInputData : IInputData<SaveListPatientGroupMstOutputData>
{
    public SaveListPatientGroupMstInputData(int hpId, int userId, List<SaveListPatientGroupMstInputItem> saveListPatientGroupMstInputItems)
    {
        HpId = hpId;
        UserId = userId;
        SaveListPatientGroupMstInputs = saveListPatientGroupMstInputItems;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
    public List<SaveListPatientGroupMstInputItem> SaveListPatientGroupMstInputs { get; private set; }
}
