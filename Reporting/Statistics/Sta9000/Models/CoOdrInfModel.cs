using Entity.Tenant;
using Helper.Common;
using Helper.Extension;

namespace Reporting.Statistics.Sta9000.Models;

public class CoOdrInfModel
{
    public RaiinInf RaiinInf { get; private set; }
    public UketukeSbtMst UketukeSbtMst { get; private set; }
    public KaMst KaMst { get; private set; }
    public UserMst UserMst { get; private set; }
    public OdrInf OdrInf { get; private set; }
    public OdrInfDetail OdrInfDetail { get; private set; }
    public PtHokenPattern PtHokenPattern { get; set; }

    private string kohi1Houbetu;
    private string kohi2Houbetu;
    private string kohi3Houbetu;
    private string kohi4Houbetu;

    public CoOdrInfModel(RaiinInf raiinInf, UketukeSbtMst uketukeSbtMst, KaMst kaMst, UserMst userMst,
        OdrInf odrInf, OdrInfDetail odrInfDetail)
    {
        RaiinInf = raiinInf;
        UketukeSbtMst = uketukeSbtMst;
        KaMst = kaMst;
        UserMst = userMst;
        OdrInf = odrInf;
        OdrInfDetail = odrInfDetail;
        PtHokenPattern = new();
        kohi1Houbetu = string.Empty;
        kohi2Houbetu = string.Empty;
        kohi3Houbetu = string.Empty;
        kohi4Houbetu = string.Empty;
    }

    public CoOdrInfModel()
    {
        RaiinInf = new();
        UketukeSbtMst = new();
        KaMst = new();
        UserMst = new();
        OdrInf = new();
        OdrInfDetail = new();
        PtHokenPattern = new();
        kohi1Houbetu = string.Empty;
        kohi2Houbetu = string.Empty;
        kohi3Houbetu = string.Empty;
        kohi4Houbetu = string.Empty;
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

    public int SortNo
    {
        get => OdrInf.SortNo;
    }

    public long RpNo
    {
        get => OdrInf.RpNo;
    }

    public long RpEdaNo
    {
        get => OdrInf.RpEdaNo;
    }

    public int HokenPid
    {
        get => OdrInf.HokenPid;
    }

    public string HokenPname
    {
        get
        {
            if (PtHokenPattern.HokenSbtCd == 0)
            {
                switch (PtHokenPattern.HokenKbn)
                {
                    case 0:
                        var hokenHoubetu109 = HokenHoubetu == "109" ? "自費レセ" : string.Empty;
                        return
                            HokenHoubetu == "108" ? "自費" :
                            hokenHoubetu109;
                    case 11: return "労災（短期給付）";
                    case 12: return "労災（傷病年金）";
                    case 13: return "労災（アフターケア）";
                    case 14: return "自賠責";
                    default:
                        return string.Empty;
                }
            }
            else
            {
                string hokenName;

                string hokenSbtCd = PtHokenPattern.HokenSbtCd.AsString().PadRight(3, '0');
                int firstNum = hokenSbtCd[0].AsInteger();
                int thirNum = hokenSbtCd[2].AsInteger();

                switch (firstNum)
                {
                    case 1: hokenName = "社保"; break;
                    case 2: hokenName = "国保"; break;
                    case 3: hokenName = "後期"; break;
                    case 4: hokenName = "退職"; break;
                    case 5: hokenName = "公費"; break;
                    default:
                        return string.Empty;
                }

                string houbetu = Kohi1Houbetu;
                if (Kohi2Houbetu.AsString() != string.Empty) houbetu += "+" + Kohi2Houbetu;
                if (Kohi3Houbetu.AsString() != string.Empty) houbetu += "+" + Kohi3Houbetu;
                if (Kohi4Houbetu.AsString() != string.Empty) houbetu += "+" + Kohi4Houbetu;

                if (thirNum == 1)
                {
                    hokenName += "単独";
                }
                else
                {
                    hokenName += $"{thirNum}併";
                }

                if (houbetu.AsString() != string.Empty)
                {
                    return hokenName + $"({houbetu})";
                }
                return hokenName;
            }
        }
    }

    public int OdrKouiKbn
    {
        get => OdrInf.OdrKouiKbn;
    }

    public string RpName
    {
        get => OdrInf.RpName ?? string.Empty;
    }

    public string InoutKbn
    {
        get => OdrInf.InoutKbn == 1 ? "院外" : string.Empty;
    }

    public string SikyuKbn
    {
        get => OdrInf.SikyuKbn == 1 ? "至急" : string.Empty;
    }

    public string SyohoSbt
    {
        get
        {
            switch (OdrInf.SyohoSbt)
            {
                case 1: return "臨時";
                case 2: return "常態";
            }
            return string.Empty;
        }
    }

    public string SanteiKbn
    {
        get
        {
            switch (OdrInf.SanteiKbn)
            {
                case 1: return "算定外";
                case 2: return "自費算定";
            }
            return string.Empty;
        }
    }

    public string TosekiKbn
    {
        get
        {
            switch (OdrInf.TosekiKbn)
            {
                case 1: return "透析前";
                case 2: return "透析後";
            }
            return string.Empty;
        }
    }

    public int DaysCnt
    {
        get => OdrInf.DaysCnt;
    }

    public int RowNo
    {
        get => OdrInfDetail.RowNo;
    }

    public int SinKouiKbn
    {
        get => OdrInfDetail.SinKouiKbn;
    }

    public string ItemCd
    {
        get => OdrInfDetail.ItemCd ?? string.Empty;
    }

    public string ItemName
    {
        get => OdrInfDetail.ItemName ?? string.Empty;
    }

    public double Suryo
    {
        get => OdrInfDetail.Suryo;
    }

    public string UnitName
    {
        get => OdrInfDetail.UnitName ?? string.Empty;
    }

    public int UnitSBT
    {
        get => OdrInfDetail.UnitSBT;
    }

    public double TermVal
    {
        get => OdrInfDetail.TermVal;
    }

    public string KohatuKbn
    {
        get
        {
            switch (OdrInfDetail.KohatuKbn)
            {
                case 0: return "後発医薬品のない先発医薬品";
                case 1: return "先発医薬品がある後発医薬品である";
                case 2: return "後発医薬品がある先発医薬品である";
                case 7: return "先発医薬品のない後発医薬品である";
            }
            return string.Empty;
        }
    }

    public string SyohoKbn
    {
        get
        {
            switch (OdrInfDetail.SyohoKbn)
            {
                case 1: return "変更不可";
                case 2: return "後発品（他銘柄）への変更可";
                case 3: return "一般名処方";
            }
            return string.Empty;
        }
    }

    public string SyohoLimitKbn
    {
        get
        {
            switch (OdrInfDetail.SyohoLimitKbn)
            {
                case 1: return "剤形不可";
                case 2: return "含量規格不可";
                case 3: return "含量規格・剤形不可";
            }
            return string.Empty;
        }
    }

    public string DrugKbn
    {
        get
        {
            switch (OdrInfDetail.DrugKbn)
            {
                case 1: return "内用薬";
                case 3: return "その他";
                case 4: return "注射薬";
                case 6: return "外用薬";
                case 8: return "歯科用薬剤";
            }
            return string.Empty;
        }
    }

    public string YohoKbn
    {
        get
        {
            switch (OdrInfDetail.YohoKbn)
            {
                case 1: return "基本用法";
                case 2: return "補助用法";
            }
            return string.Empty;
        }
    }

    public string Kokuji1
    {
        get
        {
            switch (OdrInfDetail.Kokuji1)
            {
                case "1": return "基本項目";
                case "3": return "合成項目";
                case "5": return "準用項目";
                case "7": return "加算項目";
                case "9": return "通則加算項目";
            }
            return string.Empty;
        }
    }

    public string Kokuji2
    {
        get
        {
            switch (OdrInfDetail.Kokiji2)
            {
                case "1": return "基本項目";
                case "3": return "合成項目";
                case "7": return "加算項目";
            }
            return string.Empty;
        }
    }

    public int IsNodspRece
    {
        get => OdrInfDetail.IsNodspRece;
    }

    public string IpnCd
    {
        get => OdrInfDetail.IpnCd ?? string.Empty;
    }

    public string IpnName
    {
        get => OdrInfDetail.IpnName ?? string.Empty;
    }

    public int JissiKbn
    {
        get => OdrInfDetail.JissiKbn;
    }

    public string JissiDate
    {
        get =>
            OdrInfDetail.JissiDate == null ? string.Empty :
                OdrInfDetail.JissiDate?.ToString("yyyy/MM/dd HH:mm") ?? string.Empty;
    }

    public int JissiId
    {
        get => OdrInfDetail.JissiId;
    }

    public string JissiMachine
    {
        get => OdrInfDetail.JissiMachine ?? string.Empty;
    }

    public string ReqCd
    {
        get => OdrInfDetail.ReqCd ?? string.Empty;
    }

    public string Bunkatu
    {
        get => OdrInfDetail.Bunkatu ?? string.Empty;
    }

    public string CmtName
    {
        get => OdrInfDetail.CmtName ?? string.Empty;
    }

    public string CmtOpt
    {
        get => OdrInfDetail.CmtOpt ?? string.Empty;
    }

    public string FontColor
    {
        get => OdrInfDetail.FontColor ?? string.Empty;
    }

    public string HokenHoubetu { get; set; } = string.Empty;

    public string Kohi1Houbetu
    {
        get => kohi1Houbetu == "102" ? "マル長" : kohi1Houbetu;
        set => kohi1Houbetu = value;
    }

    public string Kohi2Houbetu
    {
        get => kohi2Houbetu == "102" ? "マル長" : kohi2Houbetu;
        set => kohi2Houbetu = value;
    }

    public string Kohi3Houbetu
    {
        get => kohi3Houbetu == "102" ? "マル長" : kohi3Houbetu;
        set => kohi3Houbetu = value;
    }

    public string Kohi4Houbetu
    {
        get => kohi4Houbetu == "102" ? "マル長" : kohi4Houbetu;
        set => kohi4Houbetu = value;
    }
}