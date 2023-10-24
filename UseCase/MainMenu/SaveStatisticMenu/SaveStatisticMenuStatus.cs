namespace UseCase.MainMenu.SaveStatisticMenu;

public enum SaveStatisticMenuStatus : byte
{
    ValidateSuccess = 0,
    Successed,
    Failed,
    InvalidMenuId,
    InvalidGrpId,
    InvalidReportId,
    InvalidMenuName,
    NoPermission,
}
