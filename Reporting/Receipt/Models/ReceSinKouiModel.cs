using Helper.Constants;

namespace Reporting.Receipt.Models;

public class ReceSinKouiModel
{
    /// <summary>
    /// 更新情報
    ///     0: 変更なし
    ///     1: 追加
    ///     2: 削除
    /// </summary>
    private int _updateState = 0;

    /// <summary>
    /// キー番号
    /// </summary>
    private long _keyNo = 0;

    private string _recId = "";
    private double _kingaku = 0;

    private int _hpId;
    private long _ptId;
    private int _sinYm;
    private int _rpNo;
    private int _seqNo;
    private int _hokenPid;
    private int _hokenId;
    private string _syukeiSaki;
    private int _hokatuKensa;
    private double _totalTen;
    private double _ten;
    private double _zei;
    private int _count;
    private string _tenCount;
    private int _tenColCount;
    private int _isNodspRece;
    private int _isNodspPaperRece;
    private int _inoutKbn;
    private int _entenKbn;
    private string _cdKbn;
    private int _jihiSbt;
    private int _kazeiKbn;
    private string _detailData;
    private int _day1;
    private int _day2;
    private int _day3;
    private int _day4;
    private int _day5;
    private int _day6;
    private int _day7;
    private int _day8;
    private int _day9;
    private int _day10;
    private int _day11;
    private int _day12;
    private int _day13;
    private int _day14;
    private int _day15;
    private int _day16;
    private int _day17;
    private int _day18;
    private int _day19;
    private int _day20;
    private int _day21;
    private int _day22;
    private int _day23;
    private int _day24;
    private int _day25;
    private int _day26;
    private int _day27;
    private int _day28;
    private int _day29;
    private int _day30;
    private int _day31;
    private int _isDeleted;

    private int _santeiKbn;

    public ReceSinKouiModel(SinKouiModel sinKoui, SinRpInfModel sinRpInf, int count = 0)
    {
        _hpId = sinKoui.HpId;
        _ptId = sinKoui.PtId;
        _sinYm = sinKoui.SinYm;
        _rpNo = sinKoui.RpNo;
        _seqNo = sinKoui.SeqNo;
        _hokenPid = sinKoui.HokenPid;
        _hokenId = sinKoui.HokenId;
        _syukeiSaki = sinKoui.SyukeiSaki;
        _hokatuKensa = sinKoui.HokatuKensa;
        _totalTen = sinKoui.TotalTen;
        _ten = sinKoui.Ten;
        _zei = sinKoui.Zei;
        _count = sinKoui.Count;
        _tenCount = sinKoui.TenCount;
        _tenColCount = sinKoui.TenColCount;
        _isNodspRece = sinKoui.IsNodspRece;
        _isNodspPaperRece = sinKoui.IsNodspPaperRece;
        _inoutKbn = sinKoui.InoutKbn;
        _entenKbn = sinKoui.EntenKbn;
        _cdKbn = sinKoui.CdKbn;
        _jihiSbt = sinKoui.JihiSbt;
        _kazeiKbn = sinKoui.KazeiKbn;
        _detailData = sinKoui.DetailData;
        _day1 = sinKoui.Day1;
        _day2 = sinKoui.Day2;
        _day3 = sinKoui.Day3;
        _day4 = sinKoui.Day4;
        _day5 = sinKoui.Day5;
        _day6 = sinKoui.Day6;
        _day7 = sinKoui.Day7;
        _day8 = sinKoui.Day8;
        _day9 = sinKoui.Day9;
        _day10 = sinKoui.Day10;
        _day11 = sinKoui.Day11;
        _day12 = sinKoui.Day12;
        _day13 = sinKoui.Day13;
        _day14 = sinKoui.Day14;
        _day15 = sinKoui.Day15;
        _day16 = sinKoui.Day16;
        _day17 = sinKoui.Day17;
        _day18 = sinKoui.Day18;
        _day19 = sinKoui.Day19;
        _day20 = sinKoui.Day20;
        _day21 = sinKoui.Day21;
        _day22 = sinKoui.Day22;
        _day23 = sinKoui.Day23;
        _day24 = sinKoui.Day24;
        _day25 = sinKoui.Day25;
        _day26 = sinKoui.Day26;
        _day27 = sinKoui.Day27;
        _day28 = sinKoui.Day28;
        _day29 = sinKoui.Day29;
        _day30 = sinKoui.Day30;
        _day31 = sinKoui.Day31;
        _isDeleted = sinKoui.IsDeleted;
        _recId = sinKoui.RecId;

        _santeiKbn = sinRpInf?.SanteiKbn ?? 0;

        if (count > 0)
        {
            Count = count;
        }

    }

    /// <summary>
    /// 医療機関識別ID
    /// 
    /// </summary>
    public int HpId
    {
        get => _hpId;
    }

    /// <summary>
    /// 患者ID
    /// 
    /// </summary>
    public long PtId
    {
        get => _ptId;
    }

    /// <summary>
    /// 診療年月
    /// 
    /// </summary>
    public int SinYm
    {
        get => _sinYm;
    }

    /// <summary>
    /// 剤番号
    /// SIN_RP_INF.RP_NO
    /// </summary>
    public int RpNo
    {
        get { return _rpNo; }
    }

    /// <summary>
    /// 連番
    /// 
    /// </summary>
    public int SeqNo
    {
        get => _seqNo;
    }

    /// <summary>
    /// 保険組合せID
    /// 
    /// </summary>
    public int HokenPid
    {
        get => _hokenPid;
    }

    /// <summary>
    /// 保険ID
    /// </summary>
    public int HokenId
    {
        get => _hokenId;
    }

    /// <summary>
    /// 点数欄集計先
    /// TEN_MST.SYUKEI_SAKI + 枝番 ※別シート参照
    /// </summary>
    public string SyukeiSaki
    {
        get { return _syukeiSaki; }
        set
        {
            _syukeiSaki = value;
        }
    }

    /// <summary>
    /// 包括対象検査
    /// "0: 1～12以外の診療行為 
    /// 1: 血液化学検査の包括項目 
    /// 2: 内分泌学的検査の包括項目 
    /// 3: 肝炎ウイルス関連検査の包括項目 
    /// 5: 腫瘍マーカーの包括項目 
    /// 6: 出血・凝固検査の包括項目 
    /// 7: 自己抗体検査の包括項目 
    /// 8: 内分泌負荷試験の包括項目 
    /// 9: 感染症免疫学的検査のうち、ウイルス抗体価（定性・半定量・定量） 
    /// 10: 感染症免疫学的検査のうち、グロブリンクラス別ウイルス抗体価 
    /// 11:血漿蛋白免疫学的検査のうち、特異的ＩｇＥ半定量・定量及びアレルゲン刺激性遊離ヒスタミン（ＨＲＴ） 
    /// 12: 悪性腫瘍遺伝子検査の包括項目 "
    /// </summary>
    public int HokatuKensa
    {
        get { return _hokatuKensa; }
        set
        {
            _hokatuKensa = value;
        }
    }

    /// <summary>
    /// 合計点数
    /// 
    /// </summary>
    public double TotalTen
    {
        get => _totalTen;
        set
        {
            _totalTen = value;
        }
    }

    /// <summary>
    /// 点数小計
    /// 
    /// </summary>
    public double Ten
    {
        get => _ten;
        set
        {
            _ten = value;
            UpdateTenCount();
        }
    }

    /// <summary>
    /// 消費税
    /// 
    /// </summary>
    public double Zei
    {
        get => _zei;
        set
        {
            _zei = value;
        }
    }

    /// <summary>
    /// 回数小計
    /// 
    /// </summary>
    public int Count
    {
        get => _count;
        set
        {
            _count = value;

            UpdateTenCount();
        }
    }

    /// <summary>
    /// 点数回数
    /// 
    /// </summary>
    public string TenCount
    {
        get => _tenCount;
        set
        {
            _tenCount = value;
        }
    }

    /// <summary>
    /// 点数欄回数
    /// 
    /// </summary>
    public int TenColCount
    {
        get => _tenColCount;
        set
        {
            _tenColCount = value;
        }
    }

    /// <summary>
    /// レセ非表示区分
    /// 1:非表示
    /// </summary>
    public int IsNodspRece
    {
        get => _isNodspRece;
        set
        {
            _isNodspRece = value;
        }
    }

    /// <summary>
    /// 紙レセ非表示区分
    /// 1:非表示
    /// </summary>
    public int IsNodspPaperRece
    {
        get => _isNodspPaperRece;
        set
        {
            _isNodspPaperRece = value;
        }
    }

    /// <summary>
    /// 院外処方区分
    /// 1:院外処方
    /// </summary>
    public int InoutKbn
    {
        get => _inoutKbn;
        set
        {
            _inoutKbn = value;
        }
    }

    /// <summary>
    /// 円点区分
    /// 0:点数 1:金額
    /// </summary>
    public int EntenKbn
    {
        get => _entenKbn;
        set
        {
            _entenKbn = value;
        }
    }

    /// <summary>
    /// コード区分
    /// 代表項目のTEN_MST.CD_KBN
    /// </summary>
    public string CdKbn
    {
        get => _cdKbn;
        set
        {
            _cdKbn = value;
        }
    }

    /// <summary>
    /// 自費種別
    /// 代表項目のJIHI_SBT_MST.JIHI_SBT
    /// </summary>
    public int JihiSbt
    {
        get => _jihiSbt;
        set
        {
            _jihiSbt = value;
        }
    }

    public int KazeiKbn
    {
        get => _kazeiKbn;
        set { _kazeiKbn = value; }
    }

    /// <summary>
    /// 詳細インデックス
    /// 詳細をコード化したもの
    /// </summary>
    public string DetailData
    {
        get => _detailData;
        set
        {
            _detailData = value;
        }
    }

    /// <summary>
    /// 算定日情報1
    /// 
    /// </summary>
    public int Day1
    {
        get => _day1;
        set
        {
            _day1 = value;
        }
    }

    /// <summary>
    /// 算定日情報2
    /// 
    /// </summary>
    public int Day2
    {
        get => _day2;
        set
        {
            _day2 = value;
        }
    }

    /// <summary>
    /// 算定日情報3
    /// 
    /// </summary>
    public int Day3
    {
        get => _day3;
        set
        {
            _day3 = value;
        }
    }

    /// <summary>
    /// 算定日情報4
    /// 
    /// </summary>
    public int Day4
    {
        get => _day4;
        set
        {
            _day4 = value;
        }
    }

    /// <summary>
    /// 算定日情報5
    /// 
    /// </summary>
    public int Day5
    {
        get => _day5;
        set
        {
            _day5 = value;
        }
    }

    /// <summary>
    /// 算定日情報6
    /// 
    /// </summary>
    public int Day6
    {
        get => _day6;
        set
        {
            _day6 = value;
        }
    }

    /// <summary>
    /// 算定日情報7
    /// 
    /// </summary>
    public int Day7
    {
        get => _day7;
        set
        {
            _day7 = value;
        }
    }

    /// <summary>
    /// 算定日情報8
    /// 
    /// </summary>
    public int Day8
    {
        get => _day8;
        set
        {
            _day8 = value;
        }
    }

    /// <summary>
    /// 算定日情報9
    /// 
    /// </summary>
    public int Day9
    {
        get => _day9;
        set
        {
            _day9 = value;
        }
    }

    /// <summary>
    /// 算定日情報10
    /// 
    /// </summary>
    public int Day10
    {
        get => _day10;
        set
        {
            _day10 = value;
        }
    }

    /// <summary>
    /// 算定日情報11
    /// 
    /// </summary>
    public int Day11
    {
        get => _day11;
        set
        {
            _day11 = value;
        }
    }

    /// <summary>
    /// 算定日情報12
    /// 
    /// </summary>
    public int Day12
    {
        get => _day12;
        set
        {
            _day12 = value;
        }
    }

    /// <summary>
    /// 算定日情報13
    /// 
    /// </summary>
    public int Day13
    {
        get => _day13;
        set
        {
            _day13 = value;
        }
    }

    /// <summary>
    /// 算定日情報14
    /// 
    /// </summary>
    public int Day14
    {
        get => _day14;
        set
        {
            _day14 = value;
        }
    }

    /// <summary>
    /// 算定日情報15
    /// 
    /// </summary>
    public int Day15
    {
        get => _day15;
        set
        {
            _day15 = value;
        }
    }

    /// <summary>
    /// 算定日情報16
    /// 
    /// </summary>
    public int Day16
    {
        get => _day16;
        set
        {
            _day16 = value;
        }
    }

    /// <summary>
    /// 算定日情報17
    /// 
    /// </summary>
    public int Day17
    {
        get => _day17;
        set
        {
            _day17 = value;
        }
    }

    /// <summary>
    /// 算定日情報18
    /// 
    /// </summary>
    public int Day18
    {
        get => _day18;
        set
        {
            _day18 = value;
        }
    }

    /// <summary>
    /// 算定日情報19
    /// 
    /// </summary>
    public int Day19
    {
        get => _day19;
        set
        {
            _day19 = value;
        }
    }

    /// <summary>
    /// 算定日情報20
    /// 
    /// </summary>
    public int Day20
    {
        get => _day20;
        set
        {
            _day20 = value;
        }
    }

    /// <summary>
    /// 算定日情報21
    /// 
    /// </summary>
    public int Day21
    {
        get => _day21;
        set
        {
            _day21 = value;
        }
    }

    /// <summary>
    /// 算定日情報22
    /// 
    /// </summary>
    public int Day22
    {
        get => _day22;
        set
        {
            _day22 = value;
        }
    }

    /// <summary>
    /// 算定日情報23
    /// 
    /// </summary>
    public int Day23
    {
        get => _day23;
        set
        {
            _day23 = value;
        }
    }

    /// <summary>
    /// 算定日情報24
    /// 
    /// </summary>
    public int Day24
    {
        get => _day24;
        set
        {
            _day24 = value;
        }
    }

    /// <summary>
    /// 算定日情報25
    /// 
    /// </summary>
    public int Day25
    {
        get => _day25;
        set
        {
            _day25 = value;
        }
    }

    /// <summary>
    /// 算定日情報26
    /// 
    /// </summary>
    public int Day26
    {
        get => _day26;
        set
        {
            _day26 = value;
        }
    }

    /// <summary>
    /// 算定日情報27
    /// 
    /// </summary>
    public int Day27
    {
        get => _day27;
        set
        {
            _day27 = value;
        }
    }

    /// <summary>
    /// 算定日情報28
    /// 
    /// </summary>
    public int Day28
    {
        get => _day28;
        set
        {
            _day28 = value;
        }
    }

    /// <summary>
    /// 算定日情報29
    /// 
    /// </summary>
    public int Day29
    {
        get => _day29;
        set
        {
            _day29 = value;
        }
    }

    /// <summary>
    /// 算定日情報30
    /// 
    /// </summary>
    public int Day30
    {
        get => _day30;
        set
        {
            _day30 = value;
        }
    }

    /// <summary>
    /// 算定日情報31
    /// 
    /// </summary>
    public int Day31
    {
        get => _day31;
        set
        {
            _day31 = value;
        }
    }

    /// <summary>
    /// 削除区分
    /// </summary>
    public int IsDeleted
    {
        get => _isDeleted;
        set
        {
            _isDeleted = value;
        }
    }

    /// <summary>
    /// 更新情報
    ///     0: 変更なし
    ///     1: 追加
    ///     2: 削除
    /// </summary>
    public int UpdateState
    {
        get => _updateState;
        set { _updateState = value; }
    }

    /// <summary>
    /// キー番号
    /// </summary>
    public long KeyNo
    {
        get { return _keyNo; }
        set { _keyNo = value; }
    }
    /// <summary>
    /// レコード識別
    /// </summary>
    public string RecId
    {
        get => _recId;
        set
        {
            _recId = value;
        }
    }

    /// <summary>
    /// レコード識別を並び順に変換した値を返す
    /// レセプト電算ソート用
    /// </summary>
    public int RecIdNo
    {
        get
        {
            int ret = 0;
            if (RecId == "CO")
            {
                ret = 0;
            }
            else if (RecId == "SI" || RecId == "RI")
            {
                ret = 1;
            }
            else if (RecId == "IY")
            {
                ret = 2;
            }
            else if (RecId == "TO")
            {
                ret = 3;
            }
            return ret;
        }
    }

    /// <summary>
    /// 金額（労災用）
    /// </summary>
    public double Kingaku
    {
        get { return _kingaku; }
        set { _kingaku = value; }
    }

    private void UpdateTenCount()
    {
        TenCount =
            string.Format(FormatConst.TenCount, Ten, Count);
        TotalTen =
            Ten * Count;
    }
    /// <summary>
    /// 算定区分
    /// 2:自費
    /// </summary>
    public int SanteiKbn
    {
        get { return _santeiKbn; }
        set { _santeiKbn = value; }
    }

    /// <summary>
    /// Rp結合によって削除されたRpNo、SeqNo。実施日コメント等の展開時に使用
    /// </summary>
    public List<(int RpNo, int SeqNo)> MergeKeyNos = new List<(int, int)>();

    public int Day1Add { get; set; } = 0;
    public int Day2Add { get; set; } = 0;
    public int Day3Add { get; set; } = 0;
    public int Day4Add { get; set; } = 0;
    public int Day5Add { get; set; } = 0;
    public int Day6Add { get; set; } = 0;
    public int Day7Add { get; set; } = 0;
    public int Day8Add { get; set; } = 0;
    public int Day9Add { get; set; } = 0;
    public int Day10Add { get; set; } = 0;
    public int Day11Add { get; set; } = 0;
    public int Day12Add { get; set; } = 0;
    public int Day13Add { get; set; } = 0;
    public int Day14Add { get; set; } = 0;
    public int Day15Add { get; set; } = 0;
    public int Day16Add { get; set; } = 0;
    public int Day17Add { get; set; } = 0;
    public int Day18Add { get; set; } = 0;
    public int Day19Add { get; set; } = 0;
    public int Day20Add { get; set; } = 0;
    public int Day21Add { get; set; } = 0;
    public int Day22Add { get; set; } = 0;
    public int Day23Add { get; set; } = 0;
    public int Day24Add { get; set; } = 0;
    public int Day25Add { get; set; } = 0;
    public int Day26Add { get; set; } = 0;
    public int Day27Add { get; set; } = 0;
    public int Day28Add { get; set; } = 0;
    public int Day29Add { get; set; } = 0;
    public int Day30Add { get; set; } = 0;
    public int Day31Add { get; set; } = 0;
    /// <summary>
    /// 点数が0でも点数回数を表示するかどうか
    /// </summary>
    public bool DspZeroTenkai { get; set; } = false;
}
