﻿namespace Domain.Constant;

public static class RaiinState
{
    /// <summary>
    /// 予約
    /// </summary>
    public const int Reservation = 0;
    /// <summary>
    /// 受付
    /// </summary>
    public const int Receptionist = 1;
    /// <summary>
    /// 一時保存
    /// </summary>
    public const int TempSave = 3;
    /// <summary>
    /// 計算
    /// </summary>
    public const int Calculate = 5;
    /// <summary>
    /// 精算待ち
    /// </summary>
    public const int Waiting = 7;
    /// <summary>
    /// 精算済
    /// </summary>
    public const int Settled = 9;

    /// <summary>
    /// 診察中
    /// </summary>
    public const int Examining = 2;

    public static readonly Dictionary<int, string> VisitStatus = new()
    {
        { Reservation, "予約" },
        { Receptionist, "" },
        { Examining, "診察中" },
        { TempSave, "一時保存" },
        { Calculate, "計算" },
        { Waiting, "精算" },
        { Settled, "済み" },
    };

    public static readonly Dictionary<int, string> ChangeCalStatusDict = new()
    {
        { TempSave, "一時保存" },
        { Calculate, "計算" },
    };

    public static readonly Dictionary<int, string> ChangeWaitStatusDict = new()
    {
        { TempSave, "一時保存" },
        { Waiting, "精算" },
    };
}
