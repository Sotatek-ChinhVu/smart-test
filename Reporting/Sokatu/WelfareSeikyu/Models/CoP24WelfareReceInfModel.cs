using Entity.Tenant;
using Helper.Common;

namespace Reporting.Sokatu.WelfareSeikyu.Models;

public class CoP24WelfareReceInfModel
{
    public ReceInf ReceInf { get; private set; }
    public PtInf PtInf { get; private set; }
    public PtKohi PtKohi1 { get; private set; }
    public PtKohi PtKohi2 { get; private set; }
    public PtKohi PtKohi3 { get; private set; }
    public PtKohi PtKohi4 { get; private set; }

    private List<int> kohiHokenNos;

    public CoP24WelfareReceInfModel(ReceInf receInf, PtInf ptInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4, List<int> kohiHokenNos)
    {
        ReceInf = receInf;
        PtInf = ptInf;
        PtKohi1 = ptKohi1;
        PtKohi2 = ptKohi2;
        PtKohi3 = ptKohi3;
        PtKohi4 = ptKohi4;
        this.kohiHokenNos = kohiHokenNos;
    }

    /// <summary>
    /// 診療年月
    /// </summary>
    public int SinYm
    {
        get => ReceInf.SinYm;
    }

    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId
    {
        get => ReceInf.PtId;
    }

    /// <summary>
    /// 公費種別
    ///     1:障がい者
    ///     2:一人親家庭等
    ///     3:子ども
    ///     4:その他1
    ///     5:その他2
    /// </summary>
    /// 
    public int KohiSbt
    {
        get
        {
            switch (welfareHokenNo)
            {
                case 101: return 1;
                case 102: return 2;
                case 103: return 3;
                case 105: return 5;
                case 106: return 4;
                case 107: return 4;
                case 203: return 4;
                case 206: return 5;
            }
            return 9;
        }
    }

    private int welfareHokenNo
    {
        get =>
            kohiHokenNos.Contains(PtKohi1.HokenNo) ? PtKohi1.HokenNo :
            kohiHokenNos.Contains(PtKohi2.HokenNo) ? PtKohi2.HokenNo :
            kohiHokenNos.Contains(PtKohi3.HokenNo) ? PtKohi3.HokenNo :
            kohiHokenNos.Contains(PtKohi4.HokenNo) ? PtKohi4.HokenNo :
            0;
    }

    public string WelfareJyukyusyaNo
    {
        get =>
            kohiHokenNos.Contains(PtKohi1.HokenNo) ? CIUtil.Copy(PtKohi1.TokusyuNo, 5, 8) :
            kohiHokenNos.Contains(PtKohi2.HokenNo) ? CIUtil.Copy(PtKohi2.TokusyuNo, 5, 8) :
            kohiHokenNos.Contains(PtKohi3.HokenNo) ? CIUtil.Copy(PtKohi3.TokusyuNo, 5, 8) :
            kohiHokenNos.Contains(PtKohi4.HokenNo) ? CIUtil.Copy(PtKohi4.TokusyuNo, 5, 8) :
            "";
    }

    /// <summary>
    /// 市町コード
    /// </summary>
    public string CityCode
    {
        get =>
            kohiHokenNos.Contains(PtKohi1.HokenNo) ? CIUtil.Copy(PtKohi1.TokusyuNo, 1, 3) :
            kohiHokenNos.Contains(PtKohi2.HokenNo) ? CIUtil.Copy(PtKohi2.TokusyuNo, 1, 3) :
            kohiHokenNos.Contains(PtKohi3.HokenNo) ? CIUtil.Copy(PtKohi3.TokusyuNo, 1, 3) :
            kohiHokenNos.Contains(PtKohi4.HokenNo) ? CIUtil.Copy(PtKohi4.TokusyuNo, 1, 3) :
            "";
    }

    /// <summary>
    /// 市町村名
    /// </summary>
    public string CityName
    {
        get
        {
            switch (CityCode)
            {
                case "001": return "津市";
                case "002": return "四日市市";
                case "003": return "伊勢市";
                case "004": return "松阪市";
                case "005": return "桑名市";
                case "007": return "鈴鹿市";
                case "008": return "名張市";
                case "009": return "尾鷲市";
                case "010": return "亀山市";
                case "011": return "鳥羽市";
                case "012": return "熊野市";
                case "014": return "いなべ市";
                case "015": return "志摩市";
                case "016": return "伊賀市";
                case "053": return "木曽岬町";
                case "056": return "東員町";
                case "059": return "菰野町";
                case "061": return "朝日町";
                case "062": return "川越町";
                case "076": return "多気町";
                case "077": return "明和町";
                case "078": return "大台町";
                case "081": return "玉城町";
                case "090": return "度会町";
                case "103": return "御浜町";
                case "104": return "紀宝町";
                case "107": return "大紀町";
                case "108": return "南伊勢町";
                case "109": return "紀北町";
            }
            return "";
        }
    }

    /// <summary>
    /// 保険負担率
    /// </summary>
    public int HokenRate
    {
        get => ReceInf.HokenRate;
    }

    /// <summary>
    /// 点数
    /// </summary>
    public int Tensu
    {
        get => ReceInf.Tensu;
    }

    /// <summary>
    /// 保険レセ負担額
    /// </summary>
    public int? HokenReceFutan
    {
        get => ReceInf.HokenReceFutan;
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
            CIUtil.Copy(ReceInf.Tokki, 1, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki, 3, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki, 5, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki, 7, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki, 9, 2) == tokkiCd;
    }

    /// <summary>
    /// 公費レセ点数
    /// </summary>
    public int KohiReceTensu(int kohiIndex)
    {
        switch (kohiIndex)
        {
            case 1: return ReceInf.Kohi1ReceTensu ?? 0;
            case 2: return ReceInf.Kohi2ReceTensu ?? 0;
            case 3: return ReceInf.Kohi3ReceTensu ?? 0;
            case 4: return ReceInf.Kohi4ReceTensu ?? 0;
        }
        return 0;
    }

    public int KohiReceFutan(int kohiIndex)
    {
        switch (kohiIndex)
        {
            case 1: return ReceInf.Kohi1ReceFutan ?? 0;
            case 2: return ReceInf.Kohi2ReceFutan ?? 0;
            case 3: return ReceInf.Kohi3ReceFutan ?? 0;
            case 4: return ReceInf.Kohi4ReceFutan ?? 0;
        }
        return 0;
    }

    public int Kohi1Limit
    {
        get => ReceInf.Kohi1Limit;
    }

    /// <summary>
    /// 公費レセ記載
    /// </summary>
    public int KohiReceKisai(int kohiIndex)
    {
        switch (kohiIndex)
        {
            case 1: return ReceInf.Kohi1ReceKisai;
            case 2: return ReceInf.Kohi2ReceKisai;
            case 3: return ReceInf.Kohi3ReceKisai;
            case 4: return ReceInf.Kohi4ReceKisai;
        }
        return 0;
    }

    /// <summary>
    /// 公費法別
    /// </summary>
    public string KohiHoubetu(int kohiIndex)
    {
        switch (kohiIndex)
        {
            case 1: return ReceInf.Kohi1Houbetu;
            case 2: return ReceInf.Kohi2Houbetu;
            case 3: return ReceInf.Kohi3Houbetu;
            case 4: return ReceInf.Kohi4Houbetu;
        }
        return "";
    }

    /// <summary>
    /// 患者氏名
    /// </summary>
    public string PtName
    {
        get => PtInf.Name;
    }

    /// <summary>
    /// 患者カナ氏名
    /// </summary>
    public string PtKanaName
    {
        get => PtInf.KanaName;
    }

    /// <summary>
    /// 性別
    /// </summary>
    public int Sex
    {
        get => PtInf.Sex;
    }

    /// <summary>
    /// 生年月日
    /// </summary>
    public int BirthDay
    {
        get => PtInf.Birthday;
    }

    /// <summary>
    /// 院外処方有無
    /// </summary>
    public bool IsOutDrug { get; set; }
}
