using Entity.Tenant;
using Helper.Common;

namespace Reporting.Receipt.Models;

public class RousaiReceiptModel
{
    public PtHokenInf PtHokenInf { get; } = null;
    public PtRousaiTenki PtRousaiTenki { get; } = null;
    public PtInf PtInf { get; } = null;
    public PtKyusei PtKyusei { get; } = null;

    private int _rousaiCount;

    private int _ryoyoFirstDate;
    private int _ryoyoLastDate;
    private int? _jituNissu;
    private string _syobyoKeika;
    private int? _syokei;
    private int _syokeiGaku_I;
    private int _syokeiGaku_Ro;
    private int _outputYm;

    public RousaiReceiptModel(PtHokenInf ptHokenInf, PtRousaiTenki ptRousaiTenki, PtInf ptInf, PtKyusei ptKyusei, int rousaiCount, int outputYm)
    {
        PtHokenInf = ptHokenInf;
        PtRousaiTenki = ptRousaiTenki;
        PtInf = ptInf;
        PtKyusei = ptKyusei;
        _rousaiCount = rousaiCount;

        _outputYm = outputYm;
    }

    /// <summary>
    /// レコード識別情報
    /// </summary>
    public string RecId
    {
        get { return "RR"; }
    }
    /// <summary>
    /// 回数
    /// </summary>
    public int RousaiCount
    {
        get { return _rousaiCount; }
    }
    /// <summary>
    /// 災害区分
    ///     1	業務災害
    ///     3	通勤災害
    /// </summary>
    public int SaigaiKbn
    {
        get
        {
            int ret = 1;
            if (PtHokenInf.RousaiSaigaiKbn == 2)
            {
                ret = 3;
            }

            return ret;
        }
    }
    /// <summary>
    /// 帳票種別
    ///     3	診療費請求内訳書（入院外）
    ///     5	診療費請求内訳書（入院外傷）
    /// </summary>
    public int CyohyoSbt
    {
        get
        {
            int ret = 3;

            if (PtHokenInf.HokenKbn == 12)
            {
                ret = 5;
            }

            return ret;

        }
    }
    /// <summary>
    /// 年金証書番号
    /// </summary>
    public string NenkinNo
    {
        get
        {
            string ret = "";

            if (PtHokenInf.HokenKbn == 12)
            {
                ret = PtHokenInf.RousaiKofuNo ?? string.Empty;
            }

            return ret;
        }
    }
    /// <summary>
    /// 労働保険番号
    /// </summary>
    public string RoudouNo
    {
        get
        {
            string ret = "";

            if (PtHokenInf.HokenKbn == 11)
            {
                ret = PtHokenInf.RousaiKofuNo ?? string.Empty;
            }

            return ret;
        }
    }
    /// <summary>
    /// 傷病年月日
    /// </summary>
    public int SyobyoDate
    {
        get
        {
            return PtHokenInf.RousaiSyobyoDate;
        }
    }

    /// <summary>
    /// 新継再別
    /// </summary>
    public int Sinkei
    {
        get
        {
            int ret = 0;
            if (PtRousaiTenki != null)
            {
                ret = PtRousaiTenki.Sinkei;
            }
            return ret;
        }
    }
    /// <summary>
    /// 転帰区分
    /// </summary>
    public int Tenki
    {
        get
        {
            int ret = 0;
            if (PtRousaiTenki != null)
            {
                ret = PtRousaiTenki.Tenki;
            }
            return ret;
        }
    }
    /// <summary>
    /// 療養期間ー初日
    /// </summary>
    public int RyoyoStartDate
    {
        get
        {
            return _ryoyoFirstDate;
        }
        set
        {
            _ryoyoFirstDate = value;
        }
    }
    /// <summary>
    /// 療養期間ー末日
    /// </summary>
    public int RyoyoEndDate
    {
        get
        {
            return _ryoyoLastDate;
        }
        set
        {
            _ryoyoLastDate = value;
        }
    }
    /// <summary>
    /// 診療実日数
    /// </summary>
    public int? JituNissu
    {
        get
        {
            int? ret = _jituNissu;

            if (ret == 0)
            {
                ret = 999;
            }
            return ret;
        }
        set
        {
            _jituNissu = value;
        }
    }
    /// <summary>
    /// 労働者の氏名（カナ）
    /// </summary>
    public string PtKanaName
    {
        get
        {
            string ret = CIUtil.ToWide(CIUtil.KanaUpper((PtInf.KanaName ?? string.Empty).ToUpper()));

            if (PtKyusei != null && string.IsNullOrEmpty(PtKyusei.KanaName) == false)
            {
                ret = CIUtil.ToWide(CIUtil.KanaUpper((PtKyusei.KanaName ?? string.Empty).ToUpper()));
            }
            return ret;
        }
    }
    /// <summary>
    /// 事業の名称
    /// </summary>
    public string JigyosyoName
    {
        get
        {
            return CIUtil.ToWide(PtHokenInf.RousaiJigyosyoName ?? string.Empty);
        }
    }
    /// <summary>
    /// 事業場の所在地
    /// </summary>
    public string JigyosyoAddress
    {
        get
        {
            return CIUtil.ToWide((RousaiPrefName) + (RousaiCityName));
        }
    }
    /// <summary>
    /// 事業所都道府県
    /// </summary>
    public string RousaiPrefName
    {
        get => PtHokenInf.RousaiPrefName ?? string.Empty;
    }
    /// <summary>
    /// 事業所市町村
    /// </summary>
    public string RousaiCityName
    {
        get => PtHokenInf.RousaiCityName ?? string.Empty;
    }
    /// <summary>
    /// 傷病の経過
    /// </summary>
    public string SyobyoKeika
    {
        get
        {
            return _syobyoKeika ?? string.Empty;
        }
        set
        {
            _syobyoKeika = value;
        }
    }
    /// <summary>
    /// 傷病コード
    /// </summary>
    public string SyobyoCd
    {
        get => PtHokenInf.RousaiSyobyoCd;
    }

    /// <summary>
    /// 小計点数
    /// </summary>
    public int? Syokei
    {
        get
        {
            return _syokei;
        }
        set
        {
            _syokei = value;
        }
    }
    /// <summary>
    /// 小計点数金額換算（イ）
    /// </summary>
    public int SyokeiGaku_I
    {
        get
        {
            return _syokeiGaku_I;
        }
        set
        {
            _syokeiGaku_I = value;
        }
    }
    /// <summary>
    /// 小計金額（ロ）
    /// </summary>
    public int SyokeiGaku_RO
    {
        get
        {
            return _syokeiGaku_Ro;
        }
        set
        {
            _syokeiGaku_Ro = value;
        }
    }
    /// <summary>
    /// 合計額（イ）＋（ロ）＋（ハ）
    /// </summary>
    public int Gokei
    {
        get
        {
            return _syokeiGaku_I + _syokeiGaku_Ro;
        }
    }

    /// <summary>
    /// RRレコード
    /// </summary>
    public string RRRecord
    {
        get
        {
            // 年月日フィールド、2020/05からは西暦で記録
            int _GetDate(int date)
            {
                int result = date;
                if (_outputYm < 202005)
                {
                    result = CIUtil.SDateToWDate(date);
                }
                return result;
            }

            string ret = "";

            // レコード識別
            ret += RecId;
            //回数（同一傷病について）
            ret += "," + RousaiCount.ToString();
            //業務災害・通勤災害の区分
            ret += "," + SaigaiKbn.ToString();
            //帳票種別
            ret += "," + CyohyoSbt.ToString();
            //年金証書番号
            ret += "," + NenkinNo;
            //労働保険番号
            ret += "," + RoudouNo;
            //傷病年月日
            ret += "," + CIUtil.ToStringIgnoreZero(_GetDate(SyobyoDate));
            //新継再別
            ret += "," + Sinkei.ToString();
            //転帰事由
            ret += "," + Tenki.ToString();
            //療養期間ー初日
            ret += "," + _GetDate(RyoyoStartDate).ToString();
            //療養期間ー末日
            ret += "," + _GetDate(RyoyoEndDate).ToString();
            //診療実日数
            ret += "," + CIUtil.ToStringIgnoreNull(JituNissu);
            //労働者の氏名（カナ）
            ret += "," + CIUtil.Copy(PtKanaName, 1, 20);
            //事業の名称
            ret += "," + CIUtil.Copy(JigyosyoName, 1, 20);
            //事業場の所在地
            ret += "," + CIUtil.Copy(JigyosyoAddress, 1, 40);
            //傷病の経過
            ret += "," + CIUtil.Copy(SyobyoKeika, 1, 50);
            //小計点数
            ret += "," + CIUtil.ToStringIgnoreNull(Syokei);
            //小計点数金額換算（イ）
            ret += "," + SyokeiGaku_I.ToString();
            //小計金額（ロ）
            ret += "," + SyokeiGaku_RO.ToString();
            //食事療養合計回数
            ret += ",";
            //食事療養合計金額（ハ）
            ret += ",";
            //合計額（イ）＋（ロ）＋（ハ）
            ret += "," + Gokei.ToString();

            return ret;
        }
    }
}
