namespace Reporting.Statistics.Sta9000.Models;

public class CoSta9000HokenConf
{
    /// <summary>
    /// 保険者番号Start
    /// </summary>
    public string StartHokensyaNo { get; set; } = string.Empty;

    /// <summary>
    /// 保険者番号End
    /// </summary>
    public string EndHokensyaNo { get; set; } = string.Empty;

    /// <summary>
    /// 被保険者証記号
    /// </summary>
    public string Kigo { get; set; } = string.Empty;

    /// <summary>
    /// 被保険者証番号
    /// </summary>
    public string Bango { get; set; } = string.Empty;

    public string EdaNo { get; set; } = string.Empty;

    /// <summary>
    /// 本人・家族
    ///     1:本人　2:家族
    /// </summary>
    public int HonkeKbn { get; set; }

    /// <summary>
    /// 公費負担者番号Start
    /// </summary>
    public string StartFutansyaNo { get; set; } = string.Empty;

    /// <summary>
    /// 公費負担者番号End
    /// </summary>
    public string EndFutansyaNo { get; set; } = string.Empty;

    /// <summary>
    /// 公費特殊番号Start
    /// </summary>
    public string StartTokusyuNo { get; set; } = string.Empty;

    /// <summary>
    /// 公費特殊番号End
    /// </summary>
    public string EndTokusyuNo { get; set; } = string.Empty;

    /// <summary>
    /// 有効期限Start
    /// </summary>
    public int StartDate { get; set; }

    /// <summary>
    /// 有効期限End
    /// </summary>
    public int EndDate { get; set; }

    /// <summary>
    /// 有効期限切れ
    /// </summary>
    public int YukoKbn { get; set; }

    /// <summary>
    /// 保険種別
    ///     1:社保 2:公費 3:国保 4:退職 5:後期 6:労災 7:自賠 8:自費 9:自レ
    /// </summary>
    public List<int> HokenSbts { get; set; } = new();

    /// <summary>
    /// 法別番号 主保険
    /// </summary>
    public string Houbetu0 { get; set; } = string.Empty;

    /// <summary>
    /// 法別番号 公費１
    /// </summary>
    public string Houbetu1 { get; set; } = string.Empty;

    /// <summary>
    /// 法別番号 公費２
    /// </summary>
    public string Houbetu2 { get; set; } = string.Empty;

    /// <summary>
    /// 法別番号 公費３
    /// </summary>
    public string Houbetu3 { get; set; } = string.Empty;

    /// <summary>
    /// 法別番号 公費４
    /// </summary>
    public string Houbetu4 { get; set; } = string.Empty;

    /// <summary>
    /// 高額区分
    /// </summary>
    public List<int> KogakuKbns { get; set; } = new();


    /// <summary>
    /// 公費保険番号
    /// </summary>
    public struct KohiHokenMst
    {
        public int HokenNo { get; set; }
        public int HokenEdaNo { get; set; }

        public KohiHokenMst(int hokenNo, int hokenEdaNo)
        {
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
        }
    };

    /// <summary>
    /// 公費保険番号Start
    /// </summary>
    public KohiHokenMst StartKohiHokenNo { get; set; }

    /// <summary>
    /// 公費保険番号End
    /// </summary>
    public KohiHokenMst EndKohiHokenNo { get; set; }
}
