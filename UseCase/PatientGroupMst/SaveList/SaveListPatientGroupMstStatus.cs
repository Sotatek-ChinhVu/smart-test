namespace UseCase.PatientGroupMst.SaveList;

public enum SaveListPatientGroupMstStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
    DuplicateGroupId = 5,
    DuplicateGroupName = 6,
    DuplicateGroupDetailCode = 7,
    DuplicateGroupDetailName = 8,
    InvalidGroupId = 9,
    InvalidGroupName = 10,
    InvalidDetailGroupCode = 11,
    InvalidGroupDetailName = 12,
    DuplicateGroupDetailSeqNo = 13,
}
