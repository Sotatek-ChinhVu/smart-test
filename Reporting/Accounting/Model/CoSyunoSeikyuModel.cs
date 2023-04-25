using Entity.Tenant;

namespace Reporting.Accounting.Model;

public class CoSyunoSeikyuModel
{
    public SyunoSeikyu SyunoSeikyu { get; }
    public List<CoSyunoNyukinModel> SyunoNyukins { get; }

    public CoSyunoSeikyuModel(SyunoSeikyu syunoSeikyu, List<CoSyunoNyukinModel> syunoNyukins)
    {
        SyunoSeikyu = syunoSeikyu;
        SyunoNyukins = syunoNyukins;

        // 対象入金日を取得しておく　※後で、外部から調整される
        TargetNyukinDate = MaxNyukinDate;
    }

    public CoSyunoSeikyuModel()
    {
        SyunoSeikyu = new();
        SyunoNyukins = new();

        // 対象入金日を取得しておく　※後で、外部から調整される
        TargetNyukinDate = MaxNyukinDate;
    }

    /// <summary>
    /// 医療機関識別ID
    /// 
    /// </summary>
    public int HpId
    {
        get { return SyunoSeikyu.HpId; }
    }

    /// <summary>
    /// 患者ID
    /// 患者を識別するためのシステム固有の番号
    /// </summary>
    public long PtId
    {
        get { return SyunoSeikyu.PtId; }
    }

    /// <summary>
    /// 診療日
    /// 
    /// </summary>
    public int SinDate
    {
        get { return SyunoSeikyu.SinDate; }
    }

    /// <summary>
    /// 来院番号
    /// 
    /// </summary>
    public long RaiinNo
    {
        get { return SyunoSeikyu.RaiinNo; }
    }

    /// <summary>
    /// 入金区分
    /// 0:未精算 1:一部精算 2:免除 3:精算済
    /// </summary>
    public int NyukinKbn
    {
        get { return SyunoSeikyu.NyukinKbn; }
    }

    /// <summary>
    /// 請求点数
    /// 診療点数（KAIKEI_INF.TENSU）
    /// </summary>
    public int SeikyuTensu
    {
        get { return SyunoSeikyu.SeikyuTensu; }
    }

    /// <summary>
    /// 請求額
    /// 請求額 （KAIKEI_INF.TOTAL_PT_FUTAN）
    /// </summary>
    public int SeikyuGaku
    {
        get { return SyunoSeikyu.SeikyuGaku; }
    }

    /// <summary>
    /// 請求詳細
    /// 診療明細（SIN_KOUI.DETAIL_DATA）
    /// </summary>
    public string SeikyuDetail
    {
        get { return SyunoSeikyu.SeikyuDetail ?? string.Empty; }
    }

    /// <summary>
    /// 最後以外の入金額
    /// </summary>
    public int ExceptLastNyukin
    {
        get
        {
            int ret = 0;

            if (SyunoNyukins != null && SyunoNyukins.Any())
            {
                List<CoSyunoNyukinModel> syunoNyukins = SyunoNyukins.OrderBy(p => p.SeqNo).ToList();
                for (int i = 0; i < syunoNyukins.Count; i++)
                {
                    if (i < syunoNyukins.Count - 1)
                    {
                        // 最後の1つ手前まで合計
                        ret += syunoNyukins[i].NyukinGaku;
                    }
                    else if (syunoNyukins[i].NyukinDate != TargetNyukinDate)
                    {
                        ret += syunoNyukins[i].NyukinGaku;
                    }
                }
            }
            return ret;
        }
    }
    /// <summary>
    /// 最後の入金額
    /// </summary>
    public int LastNyukin
    {
        get
        {
            int ret = 0;
            if (SyunoNyukins != null && SyunoNyukins.Any() && SyunoNyukins.OrderByDescending(p => p.SeqNo).First().NyukinDate == TargetNyukinDate)
            {
                ret = SyunoNyukins.OrderByDescending(p => p.SeqNo).First().NyukinGaku;
            }
            return ret;
        }
    }
    /// <summary>
    /// 合計入金額
    /// </summary>
    public int TotalNyukin
    {
        get
        {
            int ret = 0;
            if (SyunoNyukins != null && SyunoNyukins.Any())
            {
                ret = SyunoNyukins.Sum(p => p.NyukinGaku);
            }
            return ret;
        }
    }
    /// <summary>
    /// 合計入金調整額
    /// </summary>
    public int TotalNyukinAjust
    {
        get
        {
            int ret = 0;
            if (SyunoNyukins != null && SyunoNyukins.Any())
            {
                ret = SyunoNyukins.Sum(p => p.AdjustFutan);
            }

            return ret;
        }
    }
    /// <summary>
    /// 支払方法
    /// </summary>
    public List<string> PayMethod
    {
        get
        {
            List<string> ret = new List<string>();
            if (SyunoNyukins != null && SyunoNyukins.Any())
            {
                var paymethods = SyunoNyukins.Select(p => p.PayMehod).GroupBy(p => p).ToList();
                paymethods?.ForEach(p =>
                {
                    ret.Add(p.Key);
                });


            }
            return ret;
        }
    }

    /// <summary>
    /// 未収額
    /// </summary>
    public int Misyu
    {
        get
        {
            int ret = SyunoSeikyu.SeikyuGaku;

            if (SyunoNyukins != null && SyunoNyukins.Any())
            {
                if (SyunoNyukins.Count > 1)
                {
                    ret -= ExceptLastNyukin;
                }
                else
                {
                    ret -= TotalNyukin;
                }
            }


            return ret;
        }
    }
    /// <summary>
    /// 患者未収
    /// </summary>
    public int PtMisyu
    {
        get
        {
            int ret = SyunoSeikyu.SeikyuGaku;

            if (SyunoNyukins != null && SyunoNyukins.Any())
            {
                ret -= TotalNyukin;
            }

            return ret;
        }
    }
    /// <summary>
    /// 返金額
    /// 未収金がマイナス値の場合、その絶対値を返す
    /// </summary>
    public int Henkin
    {
        get
        {
            int ret = Misyu;

            if (ret >= 0) return 0;

            return Math.Abs(Misyu);
        }
    }
    /// <summary>
    /// 現在のオーダー内容での請求点数
    /// </summary>
    public int NewSeikyuTensu
    {
        get => SyunoSeikyu.NewSeikyuTensu;
    }
    /// <summary>
    /// 現在のオーダー内容での請求額
    /// </summary>
    public int NewSeikyuGaku
    {
        get => SyunoSeikyu.NewSeikyuGaku;
    }
    /// <summary>
    /// 対象入金日（今回入金額として扱う日）
    /// </summary>
    public int TargetNyukinDate { get; set; } = 0;
    /// <summary>
    /// 最大入金日
    /// </summary>
    public int MaxNyukinDate
    {
        get { return SyunoNyukins != null && SyunoNyukins.Any() ? SyunoNyukins.Max(p => p.NyukinDate) : 0; }
    }
}
