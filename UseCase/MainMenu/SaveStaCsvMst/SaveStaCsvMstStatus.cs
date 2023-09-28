namespace UseCase.MainMenu.SaveStaCsvMst;

public enum SaveStaCsvMstStatus : byte
{
    Successed,
    Failed,
    InvalidHpId,
    InvalidUserId,
    InvalidConFName,
    InvalidColumnName,
}
