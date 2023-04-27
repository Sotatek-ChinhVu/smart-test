namespace Reporting.Statistics.Sta1001.Models;

public class CoSyunoInfModel
{
    public bool IsNyukin { get; set; }

    /// <summary>
    /// 前回入金額
    /// </summary>
    public int PreNyukinGaku { get; set; }

    /// <summary>
    /// 前回調整額
    /// </summary>
    public int PreAdjustFutan { get; set; }

    /// <summary>
    /// 患者負担額
    /// </summary>
    public int PtFutan { get; set; }

    /// <summary>
    /// 保険外金額
    /// </summary>
    public int JihiFutan { get; set; }

    /// <summary>
    /// 消費税
    /// </summary>
    public int JihiTax { get; set; }

    /// <summary>
    /// 計算による調整額（KaikeInf.AdjustFutan）の正負反転
    /// </summary>
    public int KaikeiAdjustFutan { get; set; }

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
    public int HokenSbtCd { get; set; }

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
    public int HokenKbn { get; set; }

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
    public string ReceSbt
    {
        get => _receSbt ?? "0000";
        set => _receSbt = value;
    }
    private string? _receSbt;

    /// <summary>
    /// 初回来院日
    /// </summary>
    public int FirstRaiinDate { get; set; }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo { get; set; }

    /// <summary>
    /// 親来院番号
    /// </summary>
    public long OyaRaiinNo { get; set; }

    /// <summary>
    /// 診察日
    /// </summary>
    public int SinDate { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum { get; set; }

    /// <summary>
    /// 氏名
    /// </summary>
    public string PtName { get; set; } = string.Empty;

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string PtKanaName { get; set; } = string.Empty;

    /// <summary>
    /// 請求点数
    /// </summary>
    public int Tensu { get; set; }

    /// <summary>
    /// 請求点数(新)
    /// </summary>
    public int NewTensu { get; set; }

    /// <summary>
    /// 請求額
    /// </summary>
    public int SeikyuGaku
    {
        get => NyukinKbn == 2 ? 0 :
            BaseSeikyuGaku - NyukinAdjustFutan - PreAdjustFutan;
    }

    /// <summary>
    /// SyunoSeikyu.SeikyuGaku
    /// </summary>
    public int BaseSeikyuGaku { get; set; }

    /// <summary>
    /// 請求額(新)
    /// </summary>
    public int NewSeikyuGaku
    {
        get => NyukinKbn == 2 ? 0 :
            BaseNewSeikyuGaku - NyukinAdjustFutan - PreAdjustFutan;
    }

    /// <summary>
    /// SyunoSeikyu.NewSeikyuGaku
    /// </summary>
    public int BaseNewSeikyuGaku { get; set; }

    /// <summary>
    /// 入金区分
    /// </summary>
    public int NyukinKbn { get; set; }

    /// <summary>
    /// 入金区分名称
    /// </summary>
    public string NyukinKbnName
    {
        get
        {
            switch (NyukinKbn)
            {
                case 0: return "未";
                case 1: return "一部";
                case 2: return "免除";
                default:
                    return "";
            }
        }
    }

    /// <summary>
    /// 調整額
    /// </summary>
    public int AdjustFutan
    {
        get => NyukinAdjustFutan + KaikeiAdjustFutan;
    }

    /// <summary>
    /// 入金時調整額
    /// </summary>
    public int NyukinAdjustFutan
    {
        get => IsNyukin ? _nyukinAdjustFutan : 0;
        set => _nyukinAdjustFutan = value;
    }
    private int _nyukinAdjustFutan;

    /// <summary>
    /// 支払方法コード
    /// </summary>
    public int PayCd
    {
        get => IsNyukin ? _payCd : 0;
        set => _payCd = value;
    }
    private int _payCd;

    /// <summary>
    /// 支払方法略称
    ///     
    /// </summary>
    public string PaySName
    {
        get => IsNyukin ? _paySName ?? string.Empty : string.Empty;
        set => _paySName = value;
    }
    private string? _paySName;

    /// <summary>
    /// 免除額
    /// </summary>
    public int MenjyoGaku
    {
        get => NyukinKbn == 2 ? BaseSeikyuGaku : 0;
    }

    /// <summary>
    /// 入金順番
    ///     同一来院に対して分割入金した場合の入金の順番
    /// </summary>
    public int NyukinSortNo
    {
        get => IsNyukin ? _nyukinSortNo : 0;
        set => _nyukinSortNo = value;
    }
    private int _nyukinSortNo;

    /// <summary>
    /// SyunoNyukin.SeqNo
    /// </summary>
    public long NyukinSeqNo { get; set; }

    /// <summary>
    /// 入金額
    /// </summary>
    public int NyukinGaku
    {
        get => !IsNyukin || NyukinKbn == 0 ? 0 : _nyukinGaku;
        set => _nyukinGaku = value;
    }
    private int _nyukinGaku;

    /// <summary>
    /// 入金日
    /// </summary>
    public int NyukinDate
    {
        get { return !IsNyukin || NyukinKbn == 0 ? SinDate : _nyukinDate; }
        set
        {
            if (!IsNyukin) return;
            _nyukinDate = value;
        }
    }
    private int _nyukinDate;

    /// <summary>
    /// 入金年月
    /// </summary>
    public int NyukinYm
    {
        get => NyukinDate / 100;
    }

    /// <summary>
    /// 入金者ID
    /// </summary>
    public int NyukinUserId
    {
        get => IsNyukin ? _nyukinUserId : 0;
        set => _nyukinUserId = value;
    }
    private int _nyukinUserId;

    /// <summary>
    /// 入金者略称
    /// </summary>
    public string NyukinUserSname
    {
        get => IsNyukin ? _nyukinUserSname ?? string.Empty : string.Empty;
        set => _nyukinUserSname = value;
    }
    private string? _nyukinUserSname;

    /// <summary>
    /// 入金時間
    /// </summary>
    public int NyukinTime
    {
        get => !IsNyukin || NyukinKbn == 0 ? 0 : _nyukinTime;
        set => _nyukinTime = value;
    }
    private int _nyukinTime;

    /// <summary>
    /// 未収額
    /// </summary>
    public int MisyuGaku
    {
        get => new int[] { 0, 2 }.Contains(NyukinKbn) ? 0 :
            BaseSeikyuGaku - NyukinAdjustFutan - NyukinGaku - PreNyukinGaku - PreAdjustFutan;
    }

    /// <summary>
    /// 受付種別
    /// </summary>
    public int UketukeSbt { get; set; }

    /// <summary>
    /// 受付種別名称
    /// </summary>
    public string UketukeSbtName { get; set; } = string.Empty;

    /// <summary>
    /// 診療科ID
    /// </summary>
    public int KaId { get; set; }

    /// <summary>
    /// 診療科略称
    /// </summary>
    public string KaSname { get; set; } = string.Empty;

    /// <summary>
    /// 担当医ID
    /// </summary>
    public int TantoId { get; set; }

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string TantoSname { get; set; } = string.Empty;

    /// <summary>
    /// 来院時間
    /// </summary>
    public string UketukeTime { get; set; } = string.Empty;

    /// <summary>
    /// 精算時間
    /// </summary>
    public string KaikeiTime { get; set; } = string.Empty;

    /// <summary>
    /// 受付者ID
    /// </summary>
    public int UketukeId { get; set; }

    /// <summary>
    /// 受付者名
    /// </summary>
    public string UketukeSname { get; set; } = string.Empty;

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
                case Helper.Constants.HokenKbn.Jihi:
                    switch (ReceSbt.Substring(0, 1))
                    {
                        case "8": return "自費保険";
                        case "9": return "自レ保険";
                    }
                    break;
                case Helper.Constants.HokenKbn.Syaho:
                    switch (ReceSbt.Substring(1, 1))
                    {
                        case "1": hokenSbt = "社"; break;
                        case "2": hokenSbt = "公費"; break;
                    }
                    break;
                case Helper.Constants.HokenKbn.Kokho:
                    switch (ReceSbt.Substring(1, 1))
                    {
                        case "1": hokenSbt = "国"; break;
                        case "3": hokenSbt = "後期"; break;
                        case "4": hokenSbt = "退"; break;
                    }
                    break;
                case Helper.Constants.HokenKbn.RousaiShort:
                case Helper.Constants.HokenKbn.RousaiLong:
                case Helper.Constants.HokenKbn.AfterCare:
                    return "労災";
                case Helper.Constants.HokenKbn.Jibai:
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

    #region レセプト種別
    /// <summary>
    /// 11x2: 本人
    /// </summary>
    public bool IsNrMine
    {
        get => ReceSbt.Substring(1, 1) == "1" && ReceSbt.Substring(3, 1) == "2";
    }

    /// <summary>
    /// 11x4: 未就学者
    /// </summary>
    public bool IsNrPreSchool
    {
        get => ReceSbt.Substring(1, 1) == "1" && ReceSbt.Substring(3, 1) == "4";
    }

    /// <summary>
    /// 11x6: 家族
    /// </summary>
    public bool IsNrFamily
    {
        get => ReceSbt.Substring(1, 1) == "1" && ReceSbt.Substring(3, 1) == "6";
    }

    /// <summary>
    /// 11x8: 高齢一般・低所
    /// 11x0: 高齢上位
    /// </summary>
    public bool IsNrElder
    {
        get => ReceSbt.Substring(1, 1) == "1" && new string[] { "8", "0" }.Contains(ReceSbt.Substring(3, 1));
    }

    /// <summary>
    /// 11xx: 一般すべて
    /// </summary>
    public bool IsNrAll
    {
        get => ReceSbt.Substring(1, 1) == "1";
    }

    /// <summary>
    /// 12x2: 公費
    /// </summary>
    public bool IsKohiOnly
    {
        get => ReceSbt.Substring(1, 1) == "2" && ReceSbt.Substring(3, 1) == "2";
    }

    /// <summary>
    /// 13x8: 後期一般・低所
    /// 13x0: 後期上位
    /// </summary>
    public bool IsKouki
    {
        get => ReceSbt.Substring(1, 1) == "3";
    }

    /// <summary>
    /// 14x2: 退職本人
    /// </summary>
    public bool IsRetMine
    {
        get => ReceSbt.Substring(1, 1) == "4" && ReceSbt.Substring(3, 1) == "2";
    }

    /// <summary>
    /// 14x4: 退職未就学者
    /// </summary>
    public bool IsRetPreSchool
    {
        get => ReceSbt.Substring(1, 1) == "4" && ReceSbt.Substring(3, 1) == "4";
    }

    /// <summary>
    /// 14x6: 退職家族
    /// </summary>
    public bool IsRetFamily
    {
        get => ReceSbt.Substring(1, 1) == "4" && ReceSbt.Substring(3, 1) == "6";
    }

    /// <summary>
    /// 14x8: 退職高齢一般・低所
    /// 14x0: 退職高齢上位
    /// </summary>
    public bool IsRetElder
    {
        get => ReceSbt.Substring(1, 1) == "4" && new string[] { "8", "0" }.Contains(ReceSbt.Substring(3, 1));
    }

    /// <summary>
    /// 14xx: 退職すべて
    /// </summary>
    public bool IsRetAll
    {
        get => ReceSbt.Substring(1, 1) == "4";
    }

    /// <summary>
    /// true: 併用
    /// </summary>
    public bool IsHeiyo
    {
        get => HokenSbtCd % 10 >= 2;
    }

    /// <summary>
    /// 自費保険
    /// </summary>
    public bool IsJihiNoRece
    {
        get => ReceSbt.Substring(0, 1) == "8";
    }

    /// <summary>
    /// 自費レセ
    /// </summary>
    public bool IsJihiRece
    {
        get => ReceSbt.Substring(0, 1) == "9";
    }

    /// <summary>
    /// 労災
    /// </summary>
    public bool IsRousai
    {
        get =>
            new int[] {
                    Helper.Constants.HokenKbn.RousaiShort,
                    Helper.Constants.HokenKbn.RousaiLong,
                    Helper.Constants.HokenKbn.AfterCare
            }.Contains(HokenKbn);
    }
    #endregion

    /// <summary>
    /// 初再診区分
    /// </summary>
    public int SyosaisinKbn { get; set; }

    /// <summary>
    /// 初再診
    /// </summary>
    public string Syosaisin
    {
        get =>
            new int[] { 1, 6 }.Contains(SyosaisinKbn) ? "初診" :
            new int[] { 3, 4, 7, 8 }.Contains(SyosaisinKbn) ? "再診" :
            "-";
    }

    /// <summary>
    /// 初回来院
    /// </summary>
    public bool IsFirstRaiin
    {
        get => SinDate == FirstRaiinDate && SinDate == NyukinDate;
    }

    /// <summary>
    /// 当日入金
    /// </summary>
    public bool IsSinDate
    {
        //未精算の来院は当日の0円入金扱いのためtrue
        get => !IsNyukin || NyukinKbn == 0 || SinDate == NyukinDate;
    }

    /// <summary>
    /// 当月入金
    /// </summary>
    public bool IsSinMonth
    {
        //未精算の来院は当日の0円入金扱いのためtrue
        get => !IsNyukin || NyukinKbn == 0 || (SinDate / 100) == (NyukinDate / 100);
    }

    /// <summary>
    /// 来院コメント
    /// </summary>
    public string RaiinCmt { get; set; } = string.Empty;

    /// <summary>
    /// 入金コメント
    /// </summary>
    public string NyukinCmt
    {
        get => IsNyukin ? _nyukinCmt ?? string.Empty : string.Empty;
        set => _nyukinCmt = value;
    }
    private string? _nyukinCmt;
}