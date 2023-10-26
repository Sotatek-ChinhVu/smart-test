namespace UseCase.MainMenu.SaveOdrSet;

public enum SaveOdrSetStatus : byte
{
    ValidateSuccessd = 0,
    Successed = 1,
    Failed = 2,
    InvalidItemCd = 3,
    InvalidSetCd = 4,
    InvalidQuanlity = 5,
}
