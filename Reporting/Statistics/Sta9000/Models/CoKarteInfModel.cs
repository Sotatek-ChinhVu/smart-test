using Entity.Tenant;
using Helper.Common;
using Helper.Extension;

namespace Reporting.Statistics.Sta9000.Models;

public class CoKarteInfModel
{
    public RaiinInf RaiinInf { get; private set; }
    public UketukeSbtMst UketukeSbtMst { get; private set; }
    public KaMst KaMst { get; private set; }
    public UserMst UserMst { get; private set; }
    public KarteInf KarteInf { get; private set; }

    public CoKarteInfModel(RaiinInf raiinInf, UketukeSbtMst uketukeSbtMst, KaMst kaMst, UserMst userMst, KarteInf karteInf)
    {
        RaiinInf = raiinInf;
        UketukeSbtMst = uketukeSbtMst;
        KaMst = kaMst;
        UserMst = userMst;
        KarteInf = karteInf;
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

    public int KarteKbn
    {
        get => KarteInf.KarteKbn;
    }

    public string Text
    {
        get => (KarteInf?.Text?.AsString() ?? string.Empty).Replace(Environment.NewLine, "⏎");
    }
}