namespace UseCase.PatientGroupMst.SaveList;

public enum SaveListPatientGroupMstStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
}
