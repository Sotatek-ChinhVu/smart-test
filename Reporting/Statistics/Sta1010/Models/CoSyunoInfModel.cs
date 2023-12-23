using Entity.Tenant;
using Helper.Common;

namespace Reporting.Statistics.Sta1010.Models;

public class CoSyunoInfModel
{
    private readonly bool isNewSeikyu;
    private readonly bool includeAdjustFutan;

    public SyunoSeikyu SyunoSeikyu { get; private set; }
    public PtInf PtInf { get; private set; }
    public RaiinInf RaiinInf { get; private set; }
    public KaMst KaMst { get; private set; }
    public UserMst TantoMst { get; private set; }

    /// <summary>
    /// 入金時の調整額（NyukinInf.AdjustFutan）
    /// </summary>
    public int NyukinAdjustFutan { get; private set; }

    /// <summary>
    /// 入金額
    /// </summary>
    public int NyukinGaku { get; private set; }

    /// <summary>
    /// 入金コメント
    /// </summary>
    public string NyukinCmt { get; set; } = string.Empty;

    /// <summary>
    /// 保険種別コード
    ///     0: 下記以外
    ///     左から          
    ///         1桁目 - 1:社保 2:国保 3:後期 4:退職 5:公費          
    ///         2桁目 - 組合せ数          
    ///         3桁目 - 1:単独 2:２併 .. 5:５併          
    ///     例) 社保単独             = 111    
    ///         社保２併(54)         = 122    
    ///         社保２併(マル長+54)  = 132    
    ///         国保単独             = 211    
    ///         国保２併(54)         = 222    
    ///         国保２併(マル長+54)  = 232    
    ///         公費単独(12)         = 511    
    ///         公費２併(21+12)      = 522    
    /// </summary>
    public int HokenSbtCd { get; private set; }

    /// <summary>
    /// 保険区分
    ///     0:自費
    ///     1:社保          
    ///     2:国保          
    ///     11:労災(短期給付)          
    ///     12:労災(傷病年金)          
    ///     13:アフターケア          
    ///     14:自賠責          
    /// </summary>
    public int HokenKbn { get; private set; }

    /// <summary>
    /// レセプト種別
    ///     11x2: 本人
    ///     11x4: 未就学者          
    ///     11x6: 家族          
    ///     11x8: 高齢一般・低所          
    ///     11x0: 高齢７割          
    ///     12x2: 公費          
    ///     13x8: 後期一般・低所          
    ///     13x0: 後期７割          
    ///     14x2: 退職本人          
    ///     14x4: 退職未就学者          
    ///     14x6: 退職家族          
    /// </summary>
    public string ReceSbt { get; private set; }

    /// <summary>
    /// 最終来院日
    /// </summary>
    public int LastVisitDate { get; private set; }

    public CoSyunoInfModel(bool isNewSeikyu, bool includeAdjustFutan,
        SyunoSeikyu syunoSeikyu, int nyukinAdjustFutan, int nyukinGaku,
        PtInf ptInf, RaiinInf raiinInf, KaMst kaMst, UserMst tantoMst,
        int hokenKbn, int hokenSbtCd, string receSbt, int lastVisitDate)
    {
        this.isNewSeikyu = isNewSeikyu;
        this.includeAdjustFutan = includeAdjustFutan;
        SyunoSeikyu = syunoSeikyu;
        NyukinAdjustFutan = nyukinAdjustFutan;
        NyukinGaku = nyukinGaku;
        PtInf = ptInf;
        RaiinInf = raiinInf;
        KaMst = kaMst;
        TantoMst = tantoMst;
        HokenKbn = hokenKbn;
        HokenSbtCd = hokenSbtCd;
        ReceSbt = receSbt;
        LastVisitDate = lastVisitDate;
    }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum
    {
        get => PtInf.PtNum;
    }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string PtKanaName
    {
        get => PtInf.KanaName ?? string.Empty;
    }

    /// <summary>
    /// 氏名
    /// </summary>
    public string PtName
    {
        get => PtInf.Name ?? string.Empty;
    }

    /// <summary>
    /// 性別コード
    /// </summary>
    public int SexCd
    {
        get => PtInf.Sex;
    }

    /// <summary>
    /// 性別
    /// </summary>
    public string Sex
    {
        get
        {
            switch (PtInf.Sex)
            {
                case 1: return "男";
                case 2: return "女";
                default:
                    return "";
            }
        }
    }

    /// <summary>
    /// 生年月日
    /// </summary>
    public int BirthDay
    {
        get => PtInf.Birthday;
    }

    /// <summary>
    /// 年齢
    /// </summary>
    public int Age
    {
        get => CIUtil.SDateToAge(PtInf.Birthday, SyunoSeikyu.SinDate);
    }

    /// <summary>
    /// 郵便番号
    /// </summary>
    public string HomePost
    {
        get => PtInf.HomePost ?? string.Empty;
    }

    /// <summary>
    /// 住所
    /// </summary>
    public string HomeAddress
    {
        get => PtInf.HomeAddress1 + " " + PtInf.HomeAddress2;
    }

    /// <summary>
    /// 電話番号１
    /// </summary>
    public string Tel1
    {
        get => PtInf.Tel1 ?? string.Empty;
    }

    /// <summary>
    /// 電話番号２
    /// </summary>
    public string Tel2
    {
        get => PtInf.Tel2??string.Empty;
    }

    /// <summary>
    /// 緊急連絡先電話番号
    /// </summary>
    public string RenrakuTel
    {
        get => PtInf.RenrakuTel ?? string.Empty;
    }

    /// <summary>
    /// 診察日
    /// </summary>
    public int SinDate
    {
        get => SyunoSeikyu.SinDate;
    }

    /// <summary>
    /// 診察日 (yyyy/MM/dd)
    /// </summary>
    public string SinDateFmt
    {
        get => CIUtil.SDateToShowSDate(SyunoSeikyu.SinDate);
    }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public int KaId
    {
        get => RaiinInf.KaId;
    }

    /// <summary>
    /// 診療科略称
    /// </summary>
    public string KaSname
    {
        get => KaMst?.KaSname ?? "";
    }

    /// <summary>
    /// 担当医ID
    /// </summary>
    public int TantoId
    {
        get => RaiinInf.TantoId;
    }

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string TantoSname
    {
        get => TantoMst?.Sname ?? "";
    }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo
    {
        get => SyunoSeikyu.RaiinNo;
    }

    /// <summary>
    /// 保険種別
    /// </summary>
    public string HokenSbt
    {
        get
        {
            string hokenSbt = "";
            switch (HokenKbn)
            {
                case CommonMasters.Constants.HokenKbn.Jihi:
                    switch (ReceSbt.Substring(0, 1))
                    {
                        case "8": return "自費保険";
                        case "9": return "自レ保険";
                    }
                    break;
                case CommonMasters.Constants.HokenKbn.Syaho:
                    switch (ReceSbt.Substring(1, 1))
                    {
                        case "1": hokenSbt = "社"; break;
                        case "2": hokenSbt = "公費"; break;
                    }
                    break;
                case CommonMasters.Constants.HokenKbn.Kokho:
                    switch (ReceSbt.Substring(1, 1))
                    {
                        case "1": hokenSbt = "国"; break;
                        case "3": hokenSbt = "後期"; break;
                        case "4": hokenSbt = "退"; break;
                    }
                    break;
                case CommonMasters.Constants.HokenKbn.RousaiShort:
                case CommonMasters.Constants.HokenKbn.RousaiLong:
                case CommonMasters.Constants.HokenKbn.AfterCare:
                    return "労災";
                case CommonMasters.Constants.HokenKbn.Jibai:
                    return "自賠";
            }

            if (hokenSbt != "")
            {
                //公費・後期..に本人家族は不要なので抜ける
                if(!new string[] { "2", "3" }.Contains(ReceSbt.Substring(1, 1)))
                {
                    switch (ReceSbt.Substring(3, 1))
                    {
                        case "2": hokenSbt += "本"; break;
                        case "4": hokenSbt += "６"; break;
                        case "6": hokenSbt += "家"; break;
                        case "0":
                        case "8": hokenSbt += "高"; break;
                    }
                }

                switch (HokenSbtCd % 10)
                {
                    case 1: hokenSbt += "単独"; break;
                    case 2: hokenSbt += "２併"; break;
                    case 3: hokenSbt += "３併"; break;
                    case 4: hokenSbt += "４併"; break;
                    case 5: hokenSbt += "５併"; break;
                }
            }

            return hokenSbt;
        }
    }

    /// <summary>
    /// 初再診
    /// </summary>
    public string Syosaisin
    {
        get =>
            new int[] { 1, 6 }.Contains(RaiinInf.SyosaisinKbn) ? "初診" :
            new int[] { 3, 4, 7, 8 }.Contains(RaiinInf.SyosaisinKbn) ? "再診" :
            "-";
    }

    /// <summary>
    /// 請求額
    /// </summary>
    public int SeikyuGaku
    {
        get => isNewSeikyu ?
            SyunoSeikyu.NewSeikyuGaku + SyunoSeikyu.NewAdjustFutan :
            SyunoSeikyu.SeikyuGaku + SyunoSeikyu.AdjustFutan;
    }

    /// <summary>
    /// 請求額(旧)
    /// </summary>
    public int OldSeikyuGaku
    {
        get => SyunoSeikyu.SeikyuGaku + SyunoSeikyu.AdjustFutan;
    }

    /// <summary>
    /// 請求額(新)
    /// </summary>
    public int NewSeikyuGaku
    {
        get => SyunoSeikyu.NewSeikyuGaku + SyunoSeikyu.NewAdjustFutan;
    }

    /// <summary>
    /// 調整額
    /// </summary>
    public int AdjustFutan
    {
        get => NyukinAdjustFutan + (isNewSeikyu ? SyunoSeikyu.NewAdjustFutan : SyunoSeikyu.AdjustFutan);
    }

    /// <summary>
    /// 調整額(旧)
    /// </summary>
    public int OldAdjustFutan
    {
        get => NyukinAdjustFutan + SyunoSeikyu.AdjustFutan;
    }

    /// <summary>
    /// 調整額(新)
    /// </summary>
    public int NewAdjustFutan
    {
        get => NyukinAdjustFutan + SyunoSeikyu.NewAdjustFutan;
    }

    /// <summary>
    /// 合計請求額
    /// </summary>
    public int TotalSeikyuGaku
    {
        get => SeikyuGaku - (includeAdjustFutan ? 0 : AdjustFutan);
    }

    /// <summary>
    /// 未収額
    /// </summary>
    public int MisyuGaku
    {
        get => TotalSeikyuGaku - NyukinGaku;
    }

    /// <summary>
    /// 未収区分
    /// </summary>
    public string MisyuKbn
    {
        get =>
            SyunoSeikyu.NyukinKbn == 2 ? "免除" :
            TotalSeikyuGaku - NyukinGaku == AdjustFutan ? "調整" :
            "未収";
    }
}
