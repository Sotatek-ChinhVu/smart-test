namespace Helper.Enum;

public enum ReceCheckStatusEnum
{
    /// <summary>
    /// 未確認
    /// </summary>
    UnConfirmed = 0,

    /// <summary>
    /// システム保留
    /// </summary>
    SystemPending = 1,

    /// <summary>
    /// 保留1
    /// </summary>
    Keep1 = 2,

    /// <summary>
    /// 保留2
    /// </summary>
    Keep2 = 3,

    /// <summary>
    /// 保留3
    /// </summary>
    Keep3 = 4,

    /// <summary>
    /// 仮確認
    /// </summary>
    TempComfirmed = 8,

    /// <summary>
    /// 確認済
    /// </summary>
    Confirmed = 9,
}
