namespace Helper.Constants;

public static class SyosaiConst
{
    /// <summary>
    /// 初再診なし
    /// </summary>
    public const int None = 0;

    /// <summary>
    /// 初診
    /// </summary>
    public const int Syosin = 1;

    /// <summary>
    /// 指定なし
    /// </summary>
    public const int Unspecified = 2;

    /// <summary>
    /// 再診
    /// </summary>
    public const int Saisin = 3;

    /// <summary>
    /// 電話再診
    /// </summary>
    public const int SaisinDenwa = 4;

    /// <summary>
    /// 自費
    /// </summary>
    public const int Jihi = 5;

    /// <summary>
    /// 初診２科目
    /// </summary>
    public const int Syosin2 = 6;

    /// <summary>
    /// 再診２科目
    /// </summary>
    public const int Saisin2 = 7;

    /// <summary>
    /// 電話再診２科目
    /// </summary>
    public const int SaisinDenwa2 = 8;

    /// <summary>
    /// 初診コロナ
    /// </summary>
    public const int SyosinCorona = 91;

    /// <summary>
    /// 初診情報通信機器
    /// </summary>
    public const int SyosinJouhou = 81;

    /// <summary>
    /// 再診情報通信機器
    /// </summary>
    public const int SaisinJouhou = 83;

    /// <summary>
    /// 初診２科目情報通信機器
    /// </summary>
    public const int Syosin2Jouhou = 86;

    /// <summary>
    /// 再診２科目情報通信機器
    /// </summary>
    public const int Saisin2Jouhou = 87;

    /// <summary>
    /// Add Const defind next order for display next order tooltip in flowsheet calendar
    /// 次回オーダー
    /// </summary>
    public const int NextOrder = -1;
}
