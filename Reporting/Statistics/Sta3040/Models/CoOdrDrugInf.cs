using Entity.Tenant;

namespace Reporting.Statistics.Sta3040.Models;

public class CoOdrDrugInf
{
    public OdrInf OdrInf { get; }

    public OdrInfDetail OdrInfDetail { get; }

    public TenMst TenMst { get; }

    public YakkaSyusaiMst YakkaSyusaiMst { get; }

    public DrugUnitConv DrugUnitConv { get; }

    public CoOdrDrugInf(OdrInf odrInf, OdrInfDetail odrInfDetail, TenMst tenMst,
        YakkaSyusaiMst yakkaSyusaiMst, DrugUnitConv drugUnitConv)
    {
        OdrInf = odrInf;
        OdrInfDetail = odrInfDetail;
        TenMst = tenMst;
        YakkaSyusaiMst = yakkaSyusaiMst;
        DrugUnitConv = drugUnitConv;
    }

    /// <summary>
    /// 診療日
    /// </summary>
    public int SinDate
    {
        get => OdrInf.SinDate;
    }

    /// <summary>
    /// 診療年月
    /// </summary>
    public int SinYm
    {
        get => OdrInf.SinDate / 100;
    }

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public string ItemCd
    {
        get => TenMst.ItemCd;
    }

    /// <summary>
    /// 請求名称
    /// </summary>
    public string ReceName
    {
        get => TenMst.ReceName ?? string.Empty;
    }

    /// <summary>
    /// 数量
    /// </summary>
    public double Suryo
    {
        get => OdrInfDetail.Suryo * OdrInf.DaysCnt * TermVal / CnvVal;
    }

    /// <summary>
    /// レセ単位名称
    /// </summary>
    public string ReceUnitName
    {
        get => TenMst.ReceUnitName ?? string.Empty;
    }

    /// <summary>
    /// 薬価
    /// </summary>
    public double Price
    {
        get
        {
            double price;
            if (OdrInf.InoutKbn == 1 && IsGetPriceInYakka && OdrInfDetail.SyohoKbn == 3 && YakkaSyusaiMst.Yakka > 0)
            {
                price = YakkaSyusaiMst.Yakka;
            }
            else
            {
                price = TenMst.Ten;
            }
            return EntenKbn == 0 ? price * 10 : price;
        }
    }

    public bool IsGetPriceInYakka { get; set; }

    /// <summary>
    /// 円点区分
    /// 0: 点数, 1:金額
    /// </summary>
    public int EntenKbn
    {
        get => new int[] { 1, 2, 4, 10, 11, 99 }.Contains(TenMst.TenId) ? 1 : 0;
    }

    /// <summary>
    /// 区分
    /// </summary>
    public string Kbn
    {
        get
        {
            string ret = "×";

            if (YakkaSyusaiMst?.ItemCd != null)
            {
                ret = YakkaSyusaiMst?.Kbn?.Trim() ?? string.Empty;
            }

            return ret;
        }
    }

    /// <summary>
    /// 単位
    /// </summary>
    public string UnitName
    {
        get => YakkaSyusaiMst?.UnitName ?? string.Empty;
    }

    /// <summary>
    /// 数量回数
    /// </summary>
    public double SuryoKaisu
    {
        get => OdrInfDetail.Suryo * OdrInf.DaysCnt;
    }

    /// <summary>
    /// 単位換算値
    /// </summary>
    public double TermVal
    {
        get => OdrInfDetail.TermVal > 0 ? OdrInfDetail.TermVal : 1;
    }

    /// <summary>
    /// 換算係数
    /// </summary>
    public double CnvVal
    {
        get => DrugUnitConv?.CnvVal ?? 1;
    }

    /// <summary>
    /// 換算有無
    /// </summary>
    public string ExistCnvVal
    {
        get => DrugUnitConv?.CnvVal != null ? "●" : "";
    }
}
