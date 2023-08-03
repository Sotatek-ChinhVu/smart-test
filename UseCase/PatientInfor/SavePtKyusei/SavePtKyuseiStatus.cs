namespace UseCase.PatientInfor.SavePtKyusei;

public enum SavePtKyuseiStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidPtId = 3,
    InvalidSeqNo = 4,
    InvalidName = 5,
    InvalidKanaName = 6,
    InvalidSindate = 7,
    ValidateSuccess = 8,
}
