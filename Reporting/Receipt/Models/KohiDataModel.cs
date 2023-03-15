using Entity.Tenant;
using Helper.Common;

namespace Reporting.Receipt.Models;

public class KohiDataModel
{
    public PtKohi PtKohi { get; } = null;
    public HokenMst HokenMst { get; } = null;

    private int? _jituNissu;
    private int? _receTen;
    private int? _receFutan;
    private int? _receKyufu;
    private int? _futan;
    private int? _futan10en;


    public KohiDataModel(PtKohi ptKohi, HokenMst hokenMst)
    {
        PtKohi = ptKohi;
        HokenMst = hokenMst;
    }

    /// <summary>
    /// レコード識別
    /// </summary>
    public string RecId
    {
        get { return "KO"; }
    }

    /// <summary>
    /// 負担者番号
    /// </summary>
    public string FutansyaNo
    {
        get { return PtKohi.FutansyaNo ?? ""; }
    }

    /// <summary>
    /// 受給者番号
    /// </summary>
    public string JyukyusyaNo
    {
        get { return PtKohi.JyukyusyaNo ?? ""; }
    }

    /// <summary>
    /// 実日数
    /// </summary>
    public int? JituNissu
    {
        get { return _jituNissu; }
        set { _jituNissu = value; }
    }
    /// <summary>
    /// 合計点数（レセプトに印字する分）
    /// </summary>
    public int? ReceTen
    {
        get { return _receTen; }
        set { _receTen = value; }
    }
    /// <summary>
    /// 点数（レセプトに印字しない場合も含む）
    /// </summary>
    public int? Tensu { get; set; }
    /// <summary>
    /// 負担金額(RECE_INF.KOHI_RECE_FUTAN)
    /// </summary>
    public int? ReceFutan
    {
        get { return _receFutan; }
        set { _receFutan = value; }
    }
    /// <summary>
    /// 一部負担額(RECE_INF.KOHI_RECE_KYUFU)
    /// </summary>
    public int? ReceKyufu
    {
        get { return _receKyufu; }
        set { _receKyufu = value; }
    }
    /// <summary>
    /// 特殊番号
    /// </summary>
    public string TokusyuNo
    {
        get { return PtKohi.TokusyuNo ?? ""; }
    }
    /// <summary>
    /// 保険ID
    /// </summary>
    public int HokenId
    {
        get { return PtKohi.HokenId; }
    }
    /// <summary>
    /// 保険番号
    /// </summary>
    public int HokenNo
    {
        get { return PtKohi.HokenNo; }
    }
    /// <summary>
    /// 保険枝番
    /// </summary>
    public int HokenEdaNo
    {
        get { return PtKohi.HokenEdaNo; }
    }
    /// <summary>
    /// 都道府県番号
    /// </summary>
    public int PrefNo
    {
        get { return PtKohi.PrefNo; }
    }

    /// <summary>
    /// 負担率
    /// </summary>
    public int Rate
    {
        get => PtKohi.Rate;
    }
    /// <summary>
    /// KOレコード
    /// </summary>
    public string KoRecord
    {
        get
        {
            string ret = "";

            // レコード識別
            ret += RecId;
            // 負担者番号
            ret += string.Format(",{0}", FutansyaNo.PadLeft(8, ' '));
            // 受給者番号
            ret += string.Format(",{0}", JyukyusyaNo.PadLeft(7, '0'));
            // 任意給付区分
            ret += ",";
            // 診療実日数
            ret += "," + CIUtil.ToStringIgnoreNull(JituNissu);
            // 合計点数
            ret += "," + CIUtil.ToStringIgnoreNull(ReceTen);
            // 負担金額 公費
            ret += "," + CIUtil.ToStringIgnoreNull(ReceFutan);
            // 負担金額 公費給付対象外来一部負担金
            ret += "," + CIUtil.ToStringIgnoreNull(ReceKyufu);
            // 負担金額 公費給付対象入院一部負担金
            ret += ",";
            // 予備
            ret += ",";
            // 食事療養・生活療養    回数
            ret += ",";
            // 食事療養・生活療養    合計金額
            ret += ",";

            return ret;
        }
    }

    /// <summary>
    /// レセ請求区分
    ///     0:社(併用)国(併用)
    ///     1:社(併用)国(単独)
    ///     2:社(単独)国(併用)
    ///     3:社(単独)国(単独)
    ///     4:社(併用)国(単独/組合併用)
    ///     5:社(単独)国(併用/組合単独)
    /// </summary>
    public int ReceSeikyuKbn
    {
        get => HokenMst == null ? 0 : HokenMst.ReceSeikyuKbn;
    }
    /// <summary>
    /// 法別番号（保険マスタ）
    /// </summary>
    public string Houbetu
    {
        get => HokenMst == null ? "" : HokenMst.Houbetu ?? string.Empty;
    }
    /// <summary>
    /// 公費負担額
    /// </summary>
    public int? Futan
    {
        get => _futan;
        set { _futan = value; }
    }
    /// <summary>
    /// 公費負担額10円単位
    /// </summary>
    public int? Futan10en
    {
        get => _futan10en;
        set { _futan10en = value; }
    }
}
