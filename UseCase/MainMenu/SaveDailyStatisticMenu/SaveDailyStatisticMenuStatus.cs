namespace UseCase.MainMenu.SaveDailyStatisticMenu;

public enum SaveDailyStatisticMenuStatus : byte
{
    ValidateSuccess = 0,
    Successed,
    Failed,
    InvalidMenuId,
    InvalidGrpId,
    InvalidReportId,
    InvalidMenuName,
}
