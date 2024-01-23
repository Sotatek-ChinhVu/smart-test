using Entity.Tenant;
using Helper.Extension;

namespace Reporting.Statistics.Sta3030.Models;

public class CoPtByomeiModel
{
    const int syushokuCdCnt = 21;
    const int settogoFrom = 1;
    const int settogoTo = 7999;
    const int setubigoFrom = 8000;
    const int setubigoTo = 8999;

    public PtInf PtInf { get; }

    public PtByomei PtByomei { get; }

    public PtLastVisitDate PtLastVisitDate { get; }

    public PtHokenInf PtHokenInf { get; }

    public CoPtByomeiModel(PtInf ptInf, PtByomei ptByomei, PtLastVisitDate ptLastVisitDate,
        PtHokenInf ptHokenInf)
    {
        PtInf = ptInf;
        PtByomei = ptByomei;
        PtLastVisitDate = ptLastVisitDate;
        PtHokenInf = ptHokenInf;
    }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum
    {
        get => PtInf.PtNum.AsLong();
    }

    /// <summary>
    /// 氏名
    /// </summary>
    public string PtName
    {
        get => PtInf.Name ?? string.Empty;
    }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string PtKanaName
    {
        get => PtInf.KanaName ?? string.Empty;
    }

    /// <summary>
    /// 生年月日
    /// </summary>
    public int Birthday
    {
        get => PtInf.Birthday;
    }

    /// <summary>
    /// 性別コード
    /// </summary>
    public int SexCd
    {
        get => PtInf.Sex;
    }

    /// <summary>
    /// 最終来院日
    /// </summary>
    public int LastVisitDate
    {
        get => PtLastVisitDate?.LastVisitDate ?? 0;
    }

    /// <summary>
    /// 病名
    /// </summary>
    public string Byomei
    {
        get => PtByomei.Byomei ?? string.Empty;
    }

    /// <summary>
    /// 病名コード
    /// </summary>
    public string ByomeiCd
    {
        get => PtByomei.ByomeiCd ?? string.Empty;
    }

    /// <summary>
    /// 主病区分
    /// </summary>
    public string SyubyoKbn
    {
        get => PtByomei.SyubyoKbn == 1 ? "（主）" : "";
    }

    /// <summary>
    /// 開始日
    /// </summary>
    public int StartDate
    {
        get => PtByomei.StartDate;
    }

    /// <summary>
    /// 転帰区分
    /// </summary>
    public int TenkiKbn
    {
        get => PtByomei.TenkiKbn;
    }

    /// <summary>
    /// 転帰日
    /// </summary>
    public int TenkiDate
    {
        get => PtByomei.TenkiDate;
    }

    /// <summary>
    /// 当月病名
    /// </summary>
    public int TogetuByomei
    {
        get => PtByomei.TogetuByomei;
    }

    /// <summary>
    /// 疾患区分
    /// </summary>
    public int SikkanKbnCd
    {
        get => PtByomei.SikkanKbn;
    }

    /// <summary>
    /// 難病外来
    /// </summary>
    public int NanbyoCd
    {
        get => PtByomei.NanByoCd;
    }

    /// <summary>
    /// 補足コメント
    /// </summary>
    public string HosokuCmt
    {
        get => PtByomei.HosokuCmt ?? string.Empty;
    }

    /// <summary>
    /// 保険
    /// </summary>
    public string Hoken
    {
        get
        {
            string ret = "";
            switch (PtHokenInf?.HokenKbn)
            {
                case 0:
                    ret = "自費"; break;
                case 1:
                    if (PtHokenInf?.HokenNo == 100)
                    {
                        ret = "公費";
                    }
                    else
                    {
                        ret = "社保";
                    }
                    break;
                case 2:
                    if (PtHokenInf?.HokensyaNo?.Length == 8 && PtHokenInf.HokensyaNo.StartsWith("39"))
                    {
                        ret = "後期";
                    }
                    else if (PtHokenInf?.HokensyaNo?.Length == 8 && PtHokenInf.HokensyaNo.StartsWith("67"))
                    {
                        ret = "退職";
                    }
                    else
                    {
                        ret = "国保";
                    }
                    break;
                case 11:
                    ret = "労災(短期)"; break;
                case 12:
                    ret = "労災(長期)"; break;
                case 13:
                    ret = "労災(ア)"; break;
                case 14:
                    ret = "自賠"; break;
                default:
                    break;
            }
            return ret;
        }
    }

    /// <summary>
    /// 保険ＩＤ
    /// </summary>
    public int HokenPid
    {
        get => PtByomei.HokenPid;
    }

    /// <summary>
    /// 接頭語コード
    /// </summary>
    public string SettogoCd
    {
        get
        {
            string ret = "";
            int syushokuCd;

            for (int i = 1; i <= syushokuCdCnt; i++)
            {
                syushokuCd = PtByomei.GetMemberValue(string.Format("SyusyokuCd{0}", i)).AsInteger();
                if (settogoFrom <= syushokuCd && syushokuCd <= settogoTo)
                {
                    ret += string.Format("{0}/", syushokuCd);
                }
            }

            ret = ret != "" ? ret.Substring(0, ret.Length - 1) : ret;
            return ret;
        }
    }

    /// <summary>
    /// 接尾語コード
    /// </summary>
    public string SetubigoCd
    {
        get
        {
            string ret = "";
            int syushokuCd;

            for (int i = 1; i <= syushokuCdCnt; i++)
            {
                syushokuCd = PtByomei.GetMemberValue(string.Format("SyusyokuCd{0}", i)).AsInteger();
                if (setubigoFrom <= syushokuCd && syushokuCd <= setubigoTo)
                {
                    ret += string.Format("{0}/", syushokuCd);
                }
            }

            ret = ret != "" ? ret.Substring(0, ret.Length - 1) : ret;
            return ret;
        }
    }
}