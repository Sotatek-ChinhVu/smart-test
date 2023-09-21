namespace UseCase.MainMenu.SaveStaCsvMst;

public enum SaveStaCsvMstStatus : byte
{
    ValidateSuccess = 0,
    Successed,
    Failed,
    InvalidMenuId,
    InvalidGrpId,
    InvalidReportId,
    InvalidMenuName,
}
