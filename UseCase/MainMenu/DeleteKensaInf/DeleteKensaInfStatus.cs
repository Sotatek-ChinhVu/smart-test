namespace UseCase.MainMenu.DeleteKensaInf;

public enum DeleteKensaInfStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidPtId = 3,
    InvalidIraiCd = 4,
}
