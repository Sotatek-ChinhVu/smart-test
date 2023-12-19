using Entity.Tenant;
using Helper.Common;
using Helper.Extension;

namespace Reporting.Sokatu.WelfareSeikyu.Models;
public class CoP14WelfareReceInfModel
{
    public ReceInf ReceInf { get; private set; } 
    public PtKohi PtKohi1 { get; private set; } 
    public PtKohi PtKohi2 { get; private set; } 
    public PtKohi PtKohi3 { get; private set; } 
    public PtKohi PtKohi4 { get; private set; } 

    public CoP14WelfareReceInfModel(ReceInf receInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4)
    {
        ReceInf = receInf;
        PtKohi1 = ptKohi1;
        PtKohi2 = ptKohi2;
        PtKohi3 = ptKohi3;
        PtKohi4 = ptKohi4;
    }

    /// <summary>
    /// 公費負担者番号
    /// </summary>
    /// <param name="kohiHoubetus">法別番号</param>
    /// <returns></returns>
    public string FutansyaNo(List<string> kohiHoubetus)
    {
        string? futansyaNo =
            kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? PtKohi1.FutansyaNo :
            kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? PtKohi2.FutansyaNo :
            kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? PtKohi3.FutansyaNo :
            kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? PtKohi4.FutansyaNo :
            "";

        //横浜市は代表番号でまとめる
        switch (futansyaNo?.Substring(0, 5))
        {
            case "80144": return "80144009";
            case "81144": return "81144008";
        }
        return futansyaNo ?? string.Empty;
    }

    public int KohiReceTensu(List<string> kohiHoubetus)
    {
        return
            kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? ReceInf.Kohi1ReceTensu ?? 0 :
            kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? ReceInf.Kohi2ReceTensu ?? 0 :
            kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? ReceInf.Kohi3ReceTensu ?? 0 :
            kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? ReceInf.Kohi4ReceTensu ?? 0 :
            0;
    }

    public int KohiReceFutan(List<string> kohiHoubetus)
    {
        return
            kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? ReceInf.Kohi1ReceFutan ?? 0 :
            kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? ReceInf.Kohi2ReceFutan ?? 0 :
            kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? ReceInf.Kohi3ReceFutan ?? 0 :
            kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? ReceInf.Kohi4ReceFutan ?? 0 :
            0;
    }

    public string KohiHokenNo(List<string> kohiHoubetus)
    {
        return
            kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? PtKohi1.HokenNo.AsString() + PtKohi1.HokenEdaNo.AsString() :
            kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? PtKohi2.HokenNo.AsString() + PtKohi2.HokenEdaNo.AsString() :
            kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? PtKohi3.HokenNo.AsString() + PtKohi3.HokenEdaNo.AsString() :
            kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? PtKohi4.HokenNo.AsString() + PtKohi4.HokenEdaNo.AsString() :
            "";
    }

    public int KohiReceKyufu(List<string> kohiHoubetus)
    {
        int receKyufu =
            kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? ReceInf.Kohi1ReceKyufu ?? 0 :
            kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? ReceInf.Kohi2ReceKyufu ?? 0 :
            kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? ReceInf.Kohi3ReceKyufu ?? 0 :
            kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? ReceInf.Kohi4ReceKyufu ?? 0 :
            0;

        return receKyufu > 0 ? receKyufu : ReceInf.HokenReceFutan ?? 0;
    }

    /// <summary>
    /// マル長
    /// </summary>
    public bool IsChoki
    {
        get => TokkiContains("02") || TokkiContains("16");

    }

    public bool TokkiContains(string tokkiCd)
    {
        return
            CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 1, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 3, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 5, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 7, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 9, 2) == tokkiCd;
    }

    #region レセプト種別
    /// <summary>
    /// 11x2: 本人
    /// </summary>
    public bool IsNrMine
    {
        get => ReceInf.ReceSbt?.Substring(1, 1) == "1" && ReceInf.ReceSbt.Substring(3, 1) == "2";
    }

    /// <summary>
    /// 11x4: 未就学者
    /// </summary>
    public bool IsNrPreSchool
    {
        get => ReceInf.ReceSbt?.Substring(1, 1) == "1" && ReceInf.ReceSbt.Substring(3, 1) == "4";
    }

    /// <summary>
    /// 11x6: 家族
    /// </summary>
    public bool IsNrFamily
    {
        get => ReceInf.ReceSbt?.Substring(1, 1) == "1" && ReceInf.ReceSbt.Substring(3, 1) == "6";
    }

    /// <summary>
    /// 11x8: 高齢一般・低所
    /// </summary>
    public bool IsNrElderIppan
    {
        get => ReceInf.ReceSbt?.Substring(1, 1) == "1" && ReceInf.ReceSbt.Substring(3, 1) == "8";
    }

    /// <summary>
    /// 11x0: 高齢上位
    /// </summary>
    public bool IsNrElderUpper
    {
        get => ReceInf.ReceSbt?.Substring(1, 1) == "1" && ReceInf.ReceSbt.Substring(3, 1) == "0";
    }
    #endregion
}
