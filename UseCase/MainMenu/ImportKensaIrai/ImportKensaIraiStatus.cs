namespace UseCase.MainMenu.ImportKensaIrai;

public enum ImportKensaIraiStatus : byte
{
    ValidateSuccessed = 0,
    Successed = 1,
    Failed = 2,
    InvalidCenterCd = 3,
    InvalidIraiCd = 4,
    InvalidKensaItemCd = 5,
    InvalidResultType = 6,
    InvalidAbnormalKbn = 7,
    InvalidInputFile = 8,
    InvalidSizeFile = 9,
}
