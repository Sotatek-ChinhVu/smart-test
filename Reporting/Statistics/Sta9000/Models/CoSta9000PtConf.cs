namespace Reporting.Statistics.Sta9000.Models;

public class CoSta9000PtConf
{
    /// <summary>
    /// テスト患者の有無
    /// </summary>
    public bool IsTester { get; set; }

    /// <summary>
    /// 患者番号Start
    /// </summary>
    public long StartPtNum { get; set; }

    /// <summary>
    /// 患者番号End
    /// </summary>
    public long EndPtNum { get; set; }

    /// <summary>
    /// 患者番号(複数指定)
    /// </summary>
    public List<long> PtNums { get; set; } = new();

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string KanaName { get; set; } = string.Empty;

    /// <summary>
    /// 氏名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 生年月日Start
    /// </summary>
    public int StartBirthday { get; set; }

    /// <summary>
    /// 生年月日End
    /// </summary>
    public int EndBirthday { get; set; }

    /// <summary>
    /// 年齢Start
    /// </summary>
    public int? StartAge { get; set; }

    /// <summary>
    /// 年齢End
    /// </summary>
    public int? EndAge { get; set; }

    /// <summary>
    /// 年齢基準日
    /// </summary>
    public int AgeBaseDate { get; set; }

    /// <summary>
    /// 性別
    /// </summary>
    public int Sex { get; set; }

    /// <summary>
    /// 郵便番号
    /// </summary>
    public string HomePost { get; set; } = string.Empty;

    /// <summary>
    /// 住所
    /// </summary>
    public string HomeAddress { get; set; } = string.Empty;

    /// <summary>
    /// 電話番号
    /// </summary>
    public string Tel { get; set; } = string.Empty;

    /// <summary>
    /// 登録日Start
    /// </summary>
    public int StartRegDate { get; set; }

    /// <summary>
    /// 登録日End
    /// </summary>
    public int EndRegDate { get; set; }

    /// <summary>
    /// 患者分類
    /// </summary>
    public struct PtGrp
    {
        public int GrpId { get; set; }
        public string GrpCode { get; set; }

        public PtGrp(int grpId, string grpCode)
        {
            GrpId = grpId;
            GrpCode = grpCode;
        }
    };

    /// <summary>
    /// 患者分類
    /// </summary>
    public List<PtGrp> PtGrps { get; set; } = new();
}
