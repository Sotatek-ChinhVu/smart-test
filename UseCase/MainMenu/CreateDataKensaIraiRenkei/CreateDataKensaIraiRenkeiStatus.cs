namespace UseCase.MainMenu.CreateDataKensaIraiRenkei;

public enum CreateDataKensaIraiRenkeiStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidPtId = 3,
    InvalidRaiinNo = 4,
    InvalidCenterCd = 5,
}
