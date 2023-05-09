using Entity.Tenant;
using Helper.Common;

namespace Reporting.Statistics.Sta9000.Models;

public class CoSinKouiModel
{
    public RaiinInf RaiinInf { get; private set; }
    public UketukeSbtMst UketukeSbtMst { get; private set; }
    public KaMst KaMst { get; private set; }
    public UserMst UserMst { get; private set; }

    public SinKouiCount SinKouiCount { get; private set; }
    public SinRpInf SinRpInf { get; private set; }
    public SinKoui SinKoui { get; private set; }
    public SinKouiDetail SinKouiDetail { get; private set; }
    public JihiSbtMst JihiSbtMst { get; private set; }

    public CoSinKouiModel(RaiinInf raiinInf, UketukeSbtMst uketukeSbtMst, KaMst kaMst, UserMst userMst,
        SinKouiCount sinKouiCount, SinRpInf sinRpInf, SinKoui sinKoui, SinKouiDetail sinKouiDetail, JihiSbtMst jihiSbtMst)
    {
        RaiinInf = raiinInf;
        UketukeSbtMst = uketukeSbtMst;
        KaMst = kaMst;
        UserMst = userMst;
        SinKouiCount = sinKouiCount;
        SinRpInf = sinRpInf;
        SinKoui = sinKoui;
        SinKouiDetail = sinKouiDetail;
        JihiSbtMst = jihiSbtMst;
    }

    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId
    {
        get => RaiinInf.PtId;
    }

    public string SinDate
    {
        get => CIUtil.SDateToShowSDate(RaiinInf.SinDate);
    }

    public long RaiinNo
    {
        get => RaiinInf.RaiinNo;
    }

    public long OyaRaiinNo
    {
        get => RaiinInf.OyaRaiinNo;
    }

    public string Status
    {
        get
        {
            switch (RaiinInf.Status)
            {
                case 0: return "予約";
                case 1: return "受付";
                case 3: return "一時保存";
                case 5: return "計算";
                case 7: return "精算待ち";
                case 9: return "済み";
            }
            return string.Empty;
        }
    }

    public string UketukeSbt
    {
        get => UketukeSbtMst?.KbnName ?? RaiinInf.UketukeSbt.ToString();
    }

    public string KaName
    {
        get => KaMst?.KaSname ?? RaiinInf.KaId.ToString();
    }

    public int TantoId
    {
        get => RaiinInf.TantoId;
    }

    public string TantoName
    {
        get => UserMst?.Sname ?? RaiinInf.TantoId.ToString();
    }

    public int UketukeNo
    {
        get => RaiinInf.UketukeNo;
    }

    public int SinId
    {
        get => SinRpInf.SinId;
    }

    public int RpNo
    {
        get => SinKouiCount.RpNo;
    }

    public int SeqNo
    {
        get => SinKouiCount.SeqNo;
    }

    public int RowNo
    {
        get => SinKouiDetail.RowNo;
    }

    public string SanteiKbn
    {
        get
        {
            switch (SinRpInf.SanteiKbn)
            {
                case 1: return "算定外";
                case 2: return "自費算定";
            }
            return string.Empty;
        }
    }

    public int HokenPid
    {
        get => SinKoui.HokenPid;
    }

    public int HokenId
    {
        get => SinKoui.HokenId;
    }

    public double SubTotalTen
    {
        get => SinKoui.Ten;
    }

    public double SubTotalZei
    {
        get => SinKoui.Zei;
    }

    public int SubTotalCount
    {
        get => SinKouiCount.Count;
    }

    public string ItemCd
    {
        get => SinKouiDetail.ItemCd ?? string.Empty;
    }

    public string OdrItemCd
    {
        get => SinKouiDetail.OdrItemCd ?? string.Empty;
    }

    public string ItemName
    {
        get => SinKouiDetail.ItemName ?? string.Empty;
    }

    public double Suryo
    {
        get => SinKouiDetail.Suryo;
    }

    public string UnitName
    {
        get => SinKouiDetail.UnitName ?? string.Empty;
    }

    public double Ten
    {
        get => SinKouiDetail.Ten;
    }

    public double Zei
    {
        get => SinKouiDetail.Zei;
    }

    public int IsNodspRece
    {
        get => SinKoui.IsNodspRece == 1 ? 1 : SinKouiDetail.IsNodspRece;
    }

    public int IsNodspPaperRece
    {
        get => SinKoui.IsNodspPaperRece == 1 ? 1 : SinKouiDetail.IsNodspPaperRece;
    }

    public int IsNodspRyosyu
    {
        get => SinKouiDetail.IsNodspRyosyu;
    }

    public int InoutKbn
    {
        get => SinKoui.InoutKbn;
    }

    public int EntenKbn
    {
        get => SinKoui.EntenKbn;
    }

    public string JihiSbt
    {
        get => JihiSbtMst?.Name ?? SinKoui.JihiSbt.ToString();
    }

    public string KazeiKbn
    {
        get
        {
            switch (SinKoui.KazeiKbn)
            {
                case 1: return "外税";
                case 2: return "外税(軽減税率対象)";
                case 3: return "内税";
                case 4: return "内税(軽減税率対象)";
            }
            return string.Empty;
        }
    }
}