using Emr.DatabaseEntity;
using Entity.Tenant;

namespace CalculateService.Receipt.Models;

public class EFRaiinInfModel
{
    public RaiinInf RaiinInf { get; } = null;
    public SinKouiCount SinKouiCount { get; } = null;
    public KaMst KaMst { get; } = null;
    public KacodeReceYousiki KacodeReceYousiki { get; } = null;

    public EFRaiinInfModel(RaiinInf raiinInf, SinKouiCount sinKouiCount, KaMst kaMst, KacodeReceYousiki kacodeReceYousiki)
    {
        RaiinInf = raiinInf;
        SinKouiCount = sinKouiCount;
        KaMst = kaMst;
        KacodeReceYousiki = kacodeReceYousiki;
    }

    public long PtId
    {
        get { return SinKouiCount.PtId; }
    }

    public int HpId
    {
        get { return SinKouiCount.HpId; }
    }
    public int RpNo
    {
        get { return SinKouiCount.RpNo; }
    }
    public int SeqNo
    {
        get { return SinKouiCount.SeqNo; }
    }
    public int SinYm
    {
        get { return SinKouiCount.SinYm; }
    }
    public int SinDate
    {
        get { return SinKouiCount.SinDate; }
    }
    public long RaiinNo
    {
        get { return SinKouiCount.RaiinNo; }
    }
    public string UketukeTime
    {
        get { return RaiinInf.UketukeTime; }
    }
    public string ReceKaCd
    {
        get
        {
            if (KaMst != null)
            {
                return KaMst.ReceKaCd;
            }
            return string.Empty;
        }
    }
    public string YousikiKaCd
    {
        get
        {
            if (KaMst != null)
            {
                if (string.IsNullOrEmpty(KaMst.YousikiKaCd) == false)
                {
                    return KaMst.YousikiKaCd;
                }
                else if (KacodeReceYousiki != null)
                {
                    return KacodeReceYousiki.YousikiKaCd;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public int TantoId
    {
        get
        {
            return RaiinInf.TantoId;
        }
    }
}
