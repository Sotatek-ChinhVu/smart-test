using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3070.Models;

public class CoSta3070PrintData
{
    public CoSta3070PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 列タイトル１行目（２行タイプ）
    /// </summary>
    public string ColTitleA1 { get; set; } = string.Empty;

    /// <summary>
    /// 列タイトル２行目（２行タイプ）
    /// </summary>
    public string ColTitleA2 { get; set; } = string.Empty;

    /// <summary>
    /// 列タイトル（１行タイプ）
    /// </summary>
    public string ColTitleB { get; set; } = string.Empty;

    /// <summary>
    /// 初診来院回数
    /// </summary>
    public string SyosinRaiinCnt { get; set; } = string.Empty;

    /// <summary>
    /// 再診来院回数
    /// </summary>
    public string SaisinRaiinCnt { get; set; } = string.Empty;

    /// <summary>
    /// その他来院回数
    /// </summary>
    public string SonotaRaiinCnt { get; set; } = string.Empty;

    /// <summary>
    /// 来院回数
    /// </summary>
    public string RaiinCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数
    /// </summary>
    public string PtCnt { get; set; } = string.Empty;

    /// <summary>
    /// 新患数
    /// </summary>
    public string SinkanCnt { get; set; } = string.Empty;

    /// <summary>
    /// 診療日数
    /// </summary>
    public string SinDateCnt { get; set; } = string.Empty;

    /// <summary>
    /// １日平均初診数
    /// </summary>
    public string SyosinAvg { get; set; } = string.Empty;

    /// <summary>
    /// １日平均患者数
    /// </summary>
    public string PtAvg { get; set; } = string.Empty;

    /// <summary>
    /// １日平均新患数
    /// </summary>
    public string SinkanAvg { get; set; } = string.Empty;

    /// <summary>
    /// 患者平均来院回数
    /// </summary>
    public string RaiinCntAvg { get; set; } = string.Empty;

    /// <summary>
    /// 構成比（来院回数）
    /// </summary>
    public string RaiinRatio { get; set; } = string.Empty;

    /// <summary>
    /// 構成比（実人数）
    /// </summary>
    public string PtRatio { get; set; } = string.Empty;

    /// <summary>
    /// 構成比（新患数）
    /// </summary>
    public string SinkanRatio { get; set; } = string.Empty;

    /// <summary>
    /// 初診来院回数内訳（男性）
    /// </summary>
    public string SyosinMaleCnt { get; set; } = string.Empty;

    /// <summary>
    /// 初診来院回数内訳（女性）
    /// </summary>
    public string SyosinFemaleCnt { get; set; } = string.Empty;

    /// <summary>
    /// 初診来院回数内訳（時間内）
    /// </summary>
    public string SyosinJinaiCnt { get; set; } = string.Empty;

    /// <summary>
    /// 初診来院回数内訳（時間外）
    /// </summary>
    public string SyosinJigaiCnt { get; set; } = string.Empty;

    /// <summary>
    /// 初診来院回数内訳（休日）
    /// </summary>
    public string SyosinKyujituCnt { get; set; } = string.Empty;

    /// <summary>
    /// 初診来院回数内訳（深夜）
    /// </summary>
    public string SyosinSinyaCnt { get; set; } = string.Empty;

    /// <summary>
    /// 初診来院回数内訳（時間外等）
    /// </summary>
    public string SyosinJigaiEtcCnt { get; set; } = string.Empty;

    /// <summary>
    /// 初診来院回数内訳（夜間早朝）
    /// </summary>
    public string SyosinYasouCnt { get; set; } = string.Empty;

    /// <summary>
    /// 再診来院回数内訳（男性）
    /// </summary>
    public string SaisinMaleCnt { get; set; } = string.Empty;

    /// <summary>
    /// 再診来院回数内訳（女性）
    /// </summary>
    public string SaisinFemaleCnt { get; set; } = string.Empty;

    /// <summary>
    /// 再診来院回数内訳（時間内）
    /// </summary>
    public string SaisinJinaiCnt { get; set; } = string.Empty;

    /// <summary>
    /// 再診来院回数内訳（時間外）
    /// </summary>
    public string SaisinJigaiCnt { get; set; } = string.Empty;

    /// <summary>
    /// 再診来院回数内訳（休日）
    /// </summary>
    public string SaisinKyujituCnt { get; set; } = string.Empty;

    /// <summary>
    /// 再診来院回数内訳（深夜）
    /// </summary>
    public string SaisinSinyaCnt { get; set; } = string.Empty;

    /// <summary>
    /// 再診来院回数内訳（時間外等）
    /// </summary>
    public string SaisinJigaiEtcCnt { get; set; } = string.Empty;

    /// <summary>
    /// 再診来院回数内訳（夜間早朝）
    /// </summary>
    public string SaisinYasouCnt { get; set; } = string.Empty;

    /// <summary>
    /// その他来院回数内訳（男性）
    /// </summary>
    public string SonotaMaleCnt { get; set; } = string.Empty;

    /// <summary>
    /// その他来院回数内訳（女性）
    /// </summary>
    public string SonotaFemaleCnt { get; set; } = string.Empty;

    /// <summary>
    /// その他来院回数内訳（時間内）
    /// </summary>
    public string SonotaJinaiCnt { get; set; } = string.Empty;

    /// <summary>
    /// その他来院回数内訳（時間外）
    /// </summary>
    public string SonotaJigaiCnt { get; set; } = string.Empty;

    /// <summary>
    /// その他来院回数内訳（休日）
    /// </summary>
    public string SonotaKyujituCnt { get; set; } = string.Empty;

    /// <summary>
    /// その他来院回数内訳（深夜）
    /// </summary>
    public string SonotaSinyaCnt { get; set; } = string.Empty;

    /// <summary>
    /// その他来院回数内訳（時間外等）
    /// </summary>
    public string SonotaJigaiEtcCnt { get; set; } = string.Empty;

    /// <summary>
    /// その他来院回数内訳（夜間早朝）
    /// </summary>
    public string SonotaYasouCnt { get; set; } = string.Empty;

    /// <summary>
    /// 来院回数内訳（男性）
    /// </summary>
    public string RaiinMaleCnt { get; set; } = string.Empty;

    /// <summary>
    /// 来院回数内訳（女性）
    /// </summary>
    public string RaiinFemaleCnt { get; set; } = string.Empty;

    /// <summary>
    /// 来院回数内訳（時間内）
    /// </summary>
    public string RaiinJinaiCnt { get; set; } = string.Empty;

    /// <summary>
    /// 来院回数内訳（時間外）
    /// </summary>
    public string RaiinJigaiCnt { get; set; } = string.Empty;

    /// <summary>
    /// 来院回数内訳（休日）
    /// </summary>
    public string RaiinKyujituCnt { get; set; } = string.Empty;

    /// <summary>
    /// 来院回数内訳（深夜）
    /// </summary>
    public string RaiinSinyaCnt { get; set; } = string.Empty;

    /// <summary>
    /// 来院回数内訳（時間外等）
    /// </summary>
    public string RaiinJigaiEtcCnt { get; set; } = string.Empty;

    /// <summary>
    /// 来院回数内訳（夜間早朝）
    /// </summary>
    public string RaiinYasouCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数内訳（男性）
    /// </summary>
    public string PtMaleCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数内訳（女性）
    /// </summary>
    public string PtFemaleCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数内訳（時間内）
    /// </summary>
    public string PtJinaiCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数内訳（時間外）
    /// </summary>
    public string PtJigaiCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数内訳（休日）
    /// </summary>
    public string PtKyujituCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数内訳（深夜）
    /// </summary>
    public string PtSinyaCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数内訳（時間外等）
    /// </summary>
    public string PtJigaiEtcCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数内訳（夜間早朝）
    /// </summary>
    public string PtYasouCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数内訳（初診）
    /// </summary>
    public string PtSyosinCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数内訳（再診）
    /// </summary>
    public string PtSaisinCnt { get; set; } = string.Empty;

    /// <summary>
    /// 実人数内訳（その他）
    /// </summary>
    public string PtSonotaCnt { get; set; } = string.Empty;

    /// <summary>
    /// 新患数内訳（男性）
    /// </summary>
    public string SinkanMaleCnt { get; set; } = string.Empty;

    /// <summary>
    /// 新患数内訳（女性）
    /// </summary>
    public string SinkanFemaleCnt { get; set; } = string.Empty;
}
