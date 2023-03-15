using Entity.Tenant;
using Helper.Constants;

namespace Reporting.Receipt.Models;

public class SinKouiModel
{
    public SinKoui SinKoui { get; } = null;

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

    public SinKouiModel(SinKoui sinKoui)
    {
        SinKoui = sinKoui;
    }

    /// <summary>
    /// 医療機関識別ID
    /// 
    /// </summary>
    public int HpId
    {
        get { return SinKoui.HpId; }
        set
        {
            if (SinKoui.HpId == value) return;
            SinKoui.HpId = value;
        }
    }

    /// <summary>
    /// 患者ID
    /// 
    /// </summary>
    public long PtId
    {
        get { return SinKoui.PtId; }
        set
        {
            if (SinKoui.PtId == value) return;
            SinKoui.PtId = value;
        }
    }

    /// <summary>
    /// 診療年月
    /// 
    /// </summary>
    public int SinYm
    {
        get { return SinKoui.SinYm; }
        set
        {
            if (SinKoui.SinYm == value) return;
            SinKoui.SinYm = value;
        }
    }

    /// <summary>
    /// 剤番号
    /// SIN_RP_INF.RP_NO
    /// </summary>
    public int RpNo
    {
        get { return SinKoui.RpNo; }
        set
        {
            if (SinKoui.RpNo == value) return;
            SinKoui.RpNo = value;
        }
    }

    /// <summary>
    /// 連番
    /// 
    /// </summary>
    public int SeqNo
    {
        get { return SinKoui.SeqNo; }
        set
        {
            if (SinKoui.SeqNo == value) return;
            SinKoui.SeqNo = value;
        }
    }

    /// <summary>
    /// 保険組合せID
    /// 
    /// </summary>
    public int HokenPid
    {
        get { return SinKoui.HokenPid; }
        set
        {
            if (SinKoui.HokenPid == value) return;
            SinKoui.HokenPid = value;
        }
    }

    /// <summary>
    /// 保険ID
    /// </summary>
    public int HokenId
    {
        get { return SinKoui.HokenId; }
        set
        {
            if (SinKoui.HokenId == value) return;
            SinKoui.HokenId = value;
        }
    }

    /// <summary>
    /// 点数欄集計先
    /// TEN_MST.SYUKEI_SAKI + 枝番 ※別シート参照
    /// </summary>
    public string SyukeiSaki
    {
        get { return SinKoui.SyukeiSaki; }
        set
        {
            if (SinKoui.SyukeiSaki == value) return;
            SinKoui.SyukeiSaki = value;
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
        get { return SinKoui.HokatuKensa; }
        set
        {
            if (SinKoui.HokatuKensa == value) return;
            SinKoui.HokatuKensa = value;
        }
    }

    /// <summary>
    /// 合計点数
    /// 
    /// </summary>
    public double TotalTen
    {
        get { return SinKoui.TotalTen; }
        set
        {
            if (SinKoui.TotalTen == value) return;
            SinKoui.TotalTen = value;
        }
    }

    /// <summary>
    /// 点数小計
    /// 
    /// </summary>
    public double Ten
    {
        get { return SinKoui.Ten; }
        set
        {
            if (SinKoui.Ten == value) return;
            SinKoui.Ten = value;
            UpdateTenCount();
        }
    }

    /// <summary>
    /// 消費税
    /// 
    /// </summary>
    public double Zei
    {
        get { return SinKoui.Zei; }
        set
        {
            if (SinKoui.Zei == value) return;
            SinKoui.Zei = value;
        }
    }

    /// <summary>
    /// 回数小計
    /// 
    /// </summary>
    public int Count
    {
        get { return SinKoui.Count; }
        set
        {
            if (SinKoui.Count == value) return;
            SinKoui.Count = value;

            UpdateTenCount();
        }
    }

    /// <summary>
    /// 点数回数
    /// 
    /// </summary>
    public string TenCount
    {
        get { return SinKoui.TenCount; }
        set
        {
            if (SinKoui.TenCount == value) return;
            SinKoui.TenCount = value;
        }
    }

    /// <summary>
    /// 点数欄回数
    /// 
    /// </summary>
    public int TenColCount
    {
        get { return SinKoui.TenColCount; }
        set
        {
            if (SinKoui.TenColCount == value) return;
            SinKoui.TenColCount = value;
        }
    }

    /// <summary>
    /// レセ非表示区分
    /// 1:非表示
    /// </summary>
    public int IsNodspRece
    {
        get { return SinKoui.IsNodspRece; }
        set
        {
            if (SinKoui.IsNodspRece == value) return;
            SinKoui.IsNodspRece = value;
        }
    }

    /// <summary>
    /// 紙レセ非表示区分
    /// 1:非表示
    /// </summary>
    public int IsNodspPaperRece
    {
        get { return SinKoui.IsNodspPaperRece; }
        set
        {
            if (SinKoui.IsNodspPaperRece == value) return;
            SinKoui.IsNodspPaperRece = value;
        }
    }

    /// <summary>
    /// 院外処方区分
    /// 1:院外処方
    /// </summary>
    public int InoutKbn
    {
        get { return SinKoui.InoutKbn; }
        set
        {
            if (SinKoui.InoutKbn == value) return;
            SinKoui.InoutKbn = value;
        }
    }

    /// <summary>
    /// 円点区分
    /// 0:点数 1:金額
    /// </summary>
    public int EntenKbn
    {
        get { return SinKoui.EntenKbn; }
        set
        {
            if (SinKoui.EntenKbn == value) return;
            SinKoui.EntenKbn = value;
        }
    }

    /// <summary>
    /// コード区分
    /// 代表項目のTEN_MST.CD_KBN
    /// </summary>
    public string CdKbn
    {
        get { return SinKoui.CdKbn; }
        set
        {
            if (SinKoui.CdKbn == value) return;
            SinKoui.CdKbn = value;
        }
    }

    /// <summary>
    /// 自費種別
    /// 代表項目のJIHI_SBT_MST.JIHI_SBT
    /// </summary>
    public int JihiSbt
    {
        get { return SinKoui.JihiSbt; }
        set
        {
            if (SinKoui.JihiSbt == value) return;
            SinKoui.JihiSbt = value;
        }
    }

    /// <summary>
    /// 課税区分
    /// TEN_MST.KAZEI_KBN
    /// </summary>
    public int KazeiKbn
    {
        get { return SinKoui.KazeiKbn; }
        set
        {
            if (SinKoui.KazeiKbn == value) return;
            SinKoui.KazeiKbn = value;
        }
    }
    /// <summary>
    /// 詳細インデックス
    /// 詳細をコード化したもの
    /// </summary>
    public string DetailData
    {
        get { return SinKoui.DetailData; }
        set
        {
            if (SinKoui.DetailData == value) return;
            SinKoui.DetailData = value;
        }
    }

    /// <summary>
    /// 算定日情報1
    /// 
    /// </summary>
    public int Day1
    {
        get { return SinKoui.Day1; }
        set
        {
            if (SinKoui.Day1 == value) return;
            SinKoui.Day1 = value;
        }
    }

    /// <summary>
    /// 算定日情報2
    /// 
    /// </summary>
    public int Day2
    {
        get { return SinKoui.Day2; }
        set
        {
            if (SinKoui.Day2 == value) return;
            SinKoui.Day2 = value;
        }
    }

    /// <summary>
    /// 算定日情報3
    /// 
    /// </summary>
    public int Day3
    {
        get { return SinKoui.Day3; }
        set
        {
            if (SinKoui.Day3 == value) return;
            SinKoui.Day3 = value;
        }
    }

    /// <summary>
    /// 算定日情報4
    /// 
    /// </summary>
    public int Day4
    {
        get { return SinKoui.Day4; }
        set
        {
            if (SinKoui.Day4 == value) return;
            SinKoui.Day4 = value;
        }
    }

    /// <summary>
    /// 算定日情報5
    /// 
    /// </summary>
    public int Day5
    {
        get { return SinKoui.Day5; }
        set
        {
            if (SinKoui.Day5 == value) return;
            SinKoui.Day5 = value;
        }
    }

    /// <summary>
    /// 算定日情報6
    /// 
    /// </summary>
    public int Day6
    {
        get { return SinKoui.Day6; }
        set
        {
            if (SinKoui.Day6 == value) return;
            SinKoui.Day6 = value;
        }
    }

    /// <summary>
    /// 算定日情報7
    /// 
    /// </summary>
    public int Day7
    {
        get { return SinKoui.Day7; }
        set
        {
            if (SinKoui.Day7 == value) return;
            SinKoui.Day7 = value;
        }
    }

    /// <summary>
    /// 算定日情報8
    /// 
    /// </summary>
    public int Day8
    {
        get { return SinKoui.Day8; }
        set
        {
            if (SinKoui.Day8 == value) return;
            SinKoui.Day8 = value;
        }
    }

    /// <summary>
    /// 算定日情報9
    /// 
    /// </summary>
    public int Day9
    {
        get { return SinKoui.Day9; }
        set
        {
            if (SinKoui.Day9 == value) return;
            SinKoui.Day9 = value;
        }
    }

    /// <summary>
    /// 算定日情報10
    /// 
    /// </summary>
    public int Day10
    {
        get { return SinKoui.Day10; }
        set
        {
            if (SinKoui.Day10 == value) return;
            SinKoui.Day10 = value;
        }
    }

    /// <summary>
    /// 算定日情報11
    /// 
    /// </summary>
    public int Day11
    {
        get { return SinKoui.Day11; }
        set
        {
            if (SinKoui.Day11 == value) return;
            SinKoui.Day11 = value;
        }
    }

    /// <summary>
    /// 算定日情報12
    /// 
    /// </summary>
    public int Day12
    {
        get { return SinKoui.Day12; }
        set
        {
            if (SinKoui.Day12 == value) return;
            SinKoui.Day12 = value;
        }
    }

    /// <summary>
    /// 算定日情報13
    /// 
    /// </summary>
    public int Day13
    {
        get { return SinKoui.Day13; }
        set
        {
            if (SinKoui.Day13 == value) return;
            SinKoui.Day13 = value;
        }
    }

    /// <summary>
    /// 算定日情報14
    /// 
    /// </summary>
    public int Day14
    {
        get { return SinKoui.Day14; }
        set
        {
            if (SinKoui.Day14 == value) return;
            SinKoui.Day14 = value;
        }
    }

    /// <summary>
    /// 算定日情報15
    /// 
    /// </summary>
    public int Day15
    {
        get { return SinKoui.Day15; }
        set
        {
            if (SinKoui.Day15 == value) return;
            SinKoui.Day15 = value;
        }
    }

    /// <summary>
    /// 算定日情報16
    /// 
    /// </summary>
    public int Day16
    {
        get { return SinKoui.Day16; }
        set
        {
            if (SinKoui.Day16 == value) return;
            SinKoui.Day16 = value;
        }
    }

    /// <summary>
    /// 算定日情報17
    /// 
    /// </summary>
    public int Day17
    {
        get { return SinKoui.Day17; }
        set
        {
            if (SinKoui.Day17 == value) return;
            SinKoui.Day17 = value;
        }
    }

    /// <summary>
    /// 算定日情報18
    /// 
    /// </summary>
    public int Day18
    {
        get { return SinKoui.Day18; }
        set
        {
            if (SinKoui.Day18 == value) return;
            SinKoui.Day18 = value;
        }
    }

    /// <summary>
    /// 算定日情報19
    /// 
    /// </summary>
    public int Day19
    {
        get { return SinKoui.Day19; }
        set
        {
            if (SinKoui.Day19 == value) return;
            SinKoui.Day19 = value;
        }
    }

    /// <summary>
    /// 算定日情報20
    /// 
    /// </summary>
    public int Day20
    {
        get { return SinKoui.Day20; }
        set
        {
            if (SinKoui.Day20 == value) return;
            SinKoui.Day20 = value;
        }
    }

    /// <summary>
    /// 算定日情報21
    /// 
    /// </summary>
    public int Day21
    {
        get { return SinKoui.Day21; }
        set
        {
            if (SinKoui.Day21 == value) return;
            SinKoui.Day21 = value;
        }
    }

    /// <summary>
    /// 算定日情報22
    /// 
    /// </summary>
    public int Day22
    {
        get { return SinKoui.Day22; }
        set
        {
            if (SinKoui.Day22 == value) return;
            SinKoui.Day22 = value;
        }
    }

    /// <summary>
    /// 算定日情報23
    /// 
    /// </summary>
    public int Day23
    {
        get { return SinKoui.Day23; }
        set
        {
            if (SinKoui.Day23 == value) return;
            SinKoui.Day23 = value;
        }
    }

    /// <summary>
    /// 算定日情報24
    /// 
    /// </summary>
    public int Day24
    {
        get { return SinKoui.Day24; }
        set
        {
            if (SinKoui.Day24 == value) return;
            SinKoui.Day24 = value;
        }
    }

    /// <summary>
    /// 算定日情報25
    /// 
    /// </summary>
    public int Day25
    {
        get { return SinKoui.Day25; }
        set
        {
            if (SinKoui.Day25 == value) return;
            SinKoui.Day25 = value;
        }
    }

    /// <summary>
    /// 算定日情報26
    /// 
    /// </summary>
    public int Day26
    {
        get { return SinKoui.Day26; }
        set
        {
            if (SinKoui.Day26 == value) return;
            SinKoui.Day26 = value;
        }
    }

    /// <summary>
    /// 算定日情報27
    /// 
    /// </summary>
    public int Day27
    {
        get { return SinKoui.Day27; }
        set
        {
            if (SinKoui.Day27 == value) return;
            SinKoui.Day27 = value;
        }
    }

    /// <summary>
    /// 算定日情報28
    /// 
    /// </summary>
    public int Day28
    {
        get { return SinKoui.Day28; }
        set
        {
            if (SinKoui.Day28 == value) return;
            SinKoui.Day28 = value;
        }
    }

    /// <summary>
    /// 算定日情報29
    /// 
    /// </summary>
    public int Day29
    {
        get { return SinKoui.Day29; }
        set
        {
            if (SinKoui.Day29 == value) return;
            SinKoui.Day29 = value;
        }
    }

    /// <summary>
    /// 算定日情報30
    /// 
    /// </summary>
    public int Day30
    {
        get { return SinKoui.Day30; }
        set
        {
            if (SinKoui.Day30 == value) return;
            SinKoui.Day30 = value;
        }
    }

    /// <summary>
    /// 算定日情報31
    /// 
    /// </summary>
    public int Day31
    {
        get { return SinKoui.Day31; }
        set
        {
            if (SinKoui.Day31 == value) return;
            SinKoui.Day31 = value;
        }
    }

    /// <summary>
    /// 削除区分
    /// </summary>
    public int IsDeleted
    {
        get { return SinKoui.IsDeleted; }
        set
        {
            if (SinKoui.IsDeleted == value) return;
            SinKoui.IsDeleted = value;
        }
    }

    /// <summary>
    /// 作成日時
    /// 
    /// </summary>
    public DateTime CreateDate
    {
        get { return SinKoui.CreateDate; }
        set
        {
            if (SinKoui.CreateDate == value) return;
            SinKoui.CreateDate = value;
        }
    }

    /// <summary>
    /// 作成者ID
    /// 
    /// </summary>
    public int CreateId
    {
        get { return SinKoui.CreateId; }
        set
        {
            if (SinKoui.CreateId == value) return;
            SinKoui.CreateId = value;
        }
    }

    /// <summary>
    /// 作成端末
    /// 
    /// </summary>
    public string CreateMachine
    {
        get { return SinKoui.CreateMachine; }
        set
        {
            if (SinKoui.CreateMachine == value) return;
            SinKoui.CreateMachine = value;
        }
    }

    /// <summary>
    /// 更新日時
    /// 
    /// </summary>
    public DateTime UpdateDate
    {
        get { return SinKoui.UpdateDate; }
        set
        {
            if (SinKoui.UpdateDate == value) return;
            SinKoui.UpdateDate = value;
        }
    }

    /// <summary>
    /// 更新者ID
    /// 
    /// </summary>
    public int UpdateId
    {
        get { return SinKoui.UpdateId; }
        set
        {
            if (SinKoui.UpdateId == value) return;
            SinKoui.UpdateId = value;
        }
    }

    /// <summary>
    /// 更新端末
    /// 
    /// </summary>
    public string UpdateMachine
    {
        get { return SinKoui.UpdateMachine; }
        set
        {
            if (SinKoui.UpdateMachine == value) return;
            SinKoui.UpdateMachine = value;
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
        get { return _updateState; }
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
        get { return SinKoui.RecId ?? string.Empty; }
        set
        {
            if (SinKoui.RecId == value) return;
            SinKoui.RecId = value;
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
}
