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
    public string ColTitleA1 { get; set; }

    /// <summary>
    /// 列タイトル２行目（２行タイプ）
    /// </summary>
    public string ColTitleA2 { get; set; }

    /// <summary>
    /// 列タイトル（１行タイプ）
    /// </summary>
    public string ColTitleB { get; set; }

    /// <summary>
    /// 初診来院回数
    /// </summary>
    public string SyosinRaiinCnt { get; set; }

    /// <summary>
    /// 再診来院回数
    /// </summary>
    public string SaisinRaiinCnt { get; set; }

    /// <summary>
    /// その他来院回数
    /// </summary>
    public string SonotaRaiinCnt { get; set; }

    /// <summary>
    /// 来院回数
    /// </summary>
    public string RaiinCnt { get; set; }

    /// <summary>
    /// 実人数
    /// </summary>
    public string PtCnt { get; set; }

    /// <summary>
    /// 新患数
    /// </summary>
    public string SinkanCnt { get; set; }

    /// <summary>
    /// 診療日数
    /// </summary>
    public string SinDateCnt { get; set; }

    /// <summary>
    /// １日平均初診数
    /// </summary>
    public string SyosinAvg { get; set; }

    /// <summary>
    /// １日平均患者数
    /// </summary>
    public string PtAvg { get; set; }

    /// <summary>
    /// １日平均新患数
    /// </summary>
    public string SinkanAvg { get; set; }

    /// <summary>
    /// 患者平均来院回数
    /// </summary>
    public string RaiinCntAvg { get; set; }

    /// <summary>
    /// 構成比（来院回数）
    /// </summary>
    public string RaiinRatio { get; set; }

    /// <summary>
    /// 構成比（実人数）
    /// </summary>
    public string PtRatio { get; set; }

    /// <summary>
    /// 構成比（新患数）
    /// </summary>
    public string SinkanRatio { get; set; }

    /// <summary>
    /// 初診来院回数内訳（男性）
    /// </summary>
    public string SyosinMaleCnt { get; set; }

    /// <summary>
    /// 初診来院回数内訳（女性）
    /// </summary>
    public string SyosinFemaleCnt { get; set; }

    /// <summary>
    /// 初診来院回数内訳（時間内）
    /// </summary>
    public string SyosinJinaiCnt { get; set; }

    /// <summary>
    /// 初診来院回数内訳（時間外）
    /// </summary>
    public string SyosinJigaiCnt { get; set; }

    /// <summary>
    /// 初診来院回数内訳（休日）
    /// </summary>
    public string SyosinKyujituCnt { get; set; }

    /// <summary>
    /// 初診来院回数内訳（深夜）
    /// </summary>
    public string SyosinSinyaCnt { get; set; }

    /// <summary>
    /// 初診来院回数内訳（時間外等）
    /// </summary>
    public string SyosinJigaiEtcCnt { get; set; }

    /// <summary>
    /// 初診来院回数内訳（夜間早朝）
    /// </summary>
    public string SyosinYasouCnt { get; set; }

    /// <summary>
    /// 再診来院回数内訳（男性）
    /// </summary>
    public string SaisinMaleCnt { get; set; }

    /// <summary>
    /// 再診来院回数内訳（女性）
    /// </summary>
    public string SaisinFemaleCnt { get; set; }

    /// <summary>
    /// 再診来院回数内訳（時間内）
    /// </summary>
    public string SaisinJinaiCnt { get; set; }

    /// <summary>
    /// 再診来院回数内訳（時間外）
    /// </summary>
    public string SaisinJigaiCnt { get; set; }

    /// <summary>
    /// 再診来院回数内訳（休日）
    /// </summary>
    public string SaisinKyujituCnt { get; set; }

    /// <summary>
    /// 再診来院回数内訳（深夜）
    /// </summary>
    public string SaisinSinyaCnt { get; set; }

    /// <summary>
    /// 再診来院回数内訳（時間外等）
    /// </summary>
    public string SaisinJigaiEtcCnt { get; set; }

    /// <summary>
    /// 再診来院回数内訳（夜間早朝）
    /// </summary>
    public string SaisinYasouCnt { get; set; }

    /// <summary>
    /// その他来院回数内訳（男性）
    /// </summary>
    public string SonotaMaleCnt { get; set; }

    /// <summary>
    /// その他来院回数内訳（女性）
    /// </summary>
    public string SonotaFemaleCnt { get; set; }

    /// <summary>
    /// その他来院回数内訳（時間内）
    /// </summary>
    public string SonotaJinaiCnt { get; set; }

    /// <summary>
    /// その他来院回数内訳（時間外）
    /// </summary>
    public string SonotaJigaiCnt { get; set; }

    /// <summary>
    /// その他来院回数内訳（休日）
    /// </summary>
    public string SonotaKyujituCnt { get; set; }

    /// <summary>
    /// その他来院回数内訳（深夜）
    /// </summary>
    public string SonotaSinyaCnt { get; set; }

    /// <summary>
    /// その他来院回数内訳（時間外等）
    /// </summary>
    public string SonotaJigaiEtcCnt { get; set; }

    /// <summary>
    /// その他来院回数内訳（夜間早朝）
    /// </summary>
    public string SonotaYasouCnt { get; set; }

    /// <summary>
    /// 来院回数内訳（男性）
    /// </summary>
    public string RaiinMaleCnt { get; set; }

    /// <summary>
    /// 来院回数内訳（女性）
    /// </summary>
    public string RaiinFemaleCnt { get; set; }

    /// <summary>
    /// 来院回数内訳（時間内）
    /// </summary>
    public string RaiinJinaiCnt { get; set; }

    /// <summary>
    /// 来院回数内訳（時間外）
    /// </summary>
    public string RaiinJigaiCnt { get; set; }

    /// <summary>
    /// 来院回数内訳（休日）
    /// </summary>
    public string RaiinKyujituCnt { get; set; }

    /// <summary>
    /// 来院回数内訳（深夜）
    /// </summary>
    public string RaiinSinyaCnt { get; set; }

    /// <summary>
    /// 来院回数内訳（時間外等）
    /// </summary>
    public string RaiinJigaiEtcCnt { get; set; }

    /// <summary>
    /// 来院回数内訳（夜間早朝）
    /// </summary>
    public string RaiinYasouCnt { get; set; }

    /// <summary>
    /// 実人数内訳（男性）
    /// </summary>
    public string PtMaleCnt { get; set; }

    /// <summary>
    /// 実人数内訳（女性）
    /// </summary>
    public string PtFemaleCnt { get; set; }

    /// <summary>
    /// 実人数内訳（時間内）
    /// </summary>
    public string PtJinaiCnt { get; set; }

    /// <summary>
    /// 実人数内訳（時間外）
    /// </summary>
    public string PtJigaiCnt { get; set; }

    /// <summary>
    /// 実人数内訳（休日）
    /// </summary>
    public string PtKyujituCnt { get; set; }

    /// <summary>
    /// 実人数内訳（深夜）
    /// </summary>
    public string PtSinyaCnt { get; set; }

    /// <summary>
    /// 実人数内訳（時間外等）
    /// </summary>
    public string PtJigaiEtcCnt { get; set; }

    /// <summary>
    /// 実人数内訳（夜間早朝）
    /// </summary>
    public string PtYasouCnt { get; set; }

    /// <summary>
    /// 実人数内訳（初診）
    /// </summary>
    public string PtSyosinCnt { get; set; }

    /// <summary>
    /// 実人数内訳（再診）
    /// </summary>
    public string PtSaisinCnt { get; set; }

    /// <summary>
    /// 実人数内訳（その他）
    /// </summary>
    public string PtSonotaCnt { get; set; }

    /// <summary>
    /// 新患数内訳（男性）
    /// </summary>
    public string SinkanMaleCnt { get; set; }

    /// <summary>
    /// 新患数内訳（女性）
    /// </summary>
    public string SinkanFemaleCnt { get; set; }
}
