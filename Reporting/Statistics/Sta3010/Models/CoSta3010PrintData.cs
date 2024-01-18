using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3010.Models;

public class CoSta3010PrintData
{
    public CoSta3010PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
        InOutKbn = -1;
        SikyuKbn = -1;
        SyohoSbt = -1;
        TosekiKbn = -1;
        SyohoKbn = -1;
        SyohoLimitKbn = -1;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// セット区分コード
    /// </summary>
    public int SetKbn { get; set; }

    /// <summary>
    /// セット区分枝番
    /// </summary>
    public string SetKbnEdaNo { get; set; }

    /// <summary>
    /// セット区分名称
    /// </summary>
    public string SetKbnName { get; set; }

    /// <summary>
    /// 階層１
    /// </summary>
    public string Level1 { get; set; }

    /// <summary>
    /// 階層２
    /// </summary>
    public string Level2 { get; set; }

    /// <summary>
    /// 階層３
    /// </summary>
    public string Level3 { get; set; }

    /// <summary>
    /// セットコード
    /// </summary>
    public int SetCd { get; set; }

    /// <summary>
    /// セット名称
    /// </summary>
    public string SetName { get; set; }

    /// <summary>
    /// 体重別セット
    /// </summary>
    public int WeightKbn { get; set; }

    /// <summary>
    /// 体重別セット（*）
    /// </summary>
    public string WeightKbnFmt
    {
        get => WeightKbn == 1 ? "*" : "";
    }

    /// <summary>
    /// RP
    /// </summary>
    public string Rp { get; set; }

    /// <summary>
    /// RP番号
    /// </summary>
    public string RpNo { get; set; }

    /// <summary>
    /// RP番号枝番
    /// </summary>
    public string RpEdaNo { get; set; }

    /// <summary>
    /// 行為区分
    /// </summary>
    public int OdrKouiKbn { get; set; }

    /// <summary>
    /// 行為区分名称
    /// </summary>
    public string OdrKouiKbnName
    {
        get
        {
            switch (OdrKouiKbn)
            {
                case 10: return "初再診";
                case 11: return "初診料";
                case 12: return "再診料";
                case 13: return "医学管理";
                case 14: return "在宅";
                case 20: return "投薬";
                case 21: return "内服";
                case 22: return "頓服";
                case 23: return "外用";
                case 24: return "調剤料";
                case 25: return "処方料";
                case 26: return "麻毒加算";
                case 27: return "調剤技術基本料";
                case 28: return "自己注射";
                case 30: return "注射薬剤";
                case 31: return "皮下筋肉注射";
                case 32: return "静脈注射";
                case 33: return "点滴注射";
                case 34: return "その他注射";
                case 40: return "処置";
                case 50: return "手術";
                case 52: return "輸血";
                case 54: return "麻酔";
                case 60: return "検査";
                case 61: return "検体検査";
                case 62: return "生体検査";
                case 64: return "病理診断";
                case 70: return "画像診断";
                case 77: return "フィルム";
                case 80: return "その他";
                case 81: return "リハビリ";
                case 82: return "精神";
                case 83: return "処方箋料";
                case 84: return "放射線";
                case 90: return "入院料";
                case 92: return "特定入院料";
                case 93: return "老人一部負担金";
                case 96: return "保険外";
                case 97: return "食事療養・生活療養・標準負担";
                case 99: return "コメント";
                case 100: return "コメント（処方箋）";
                case 101: return "コメント（処方箋備考）";
            }
            return "";
        }
    }

    /// <summary>
    /// 院内院外
    /// </summary>
    public int InOutKbn { get; set; }

    /// <summary>
    /// 院内院外名称
    /// </summary>
    public string InOutKbnName
    {
        get
        {
            switch (InOutKbn)
            {
                case 0: return "院内";
                case 1: return "院外";
            }
            return "";
        }
    }

    /// <summary>
    /// 至急区分
    /// </summary>
    public int SikyuKbn { get; set; }

    /// <summary>
    /// 至急区分名称
    /// </summary>
    public string SikyuKbnName
    {
        get
        {
            switch (SikyuKbn)
            {
                case 0: return "通常";
                case 1: return "至急";
            }
            return "";
        }
    }

    /// <summary>
    /// 処方種別
    /// </summary>
    public int SyohoSbt { get; set; }

    /// <summary>
    /// 処方種別名称
    /// </summary>
    public string SyohoSbtName
    {
        get
        {
            switch (SyohoSbt)
            {
                case 0: return "日数判断";
                case 1: return "臨時";
                case 2: return "常態";
            }
            return "";
        }
    }

    /// <summary>
    /// 算定区分
    /// </summary>
    public int SanteiKbn { get; set; }

    /// <summary>
    /// 算定区分名称
    /// </summary>
    public string SanteiKbnName
    {
        get
        {
            switch (SanteiKbn)
            {
                case 1: return "算定外";
                case 2: return "自費算定";
            }
            return "";
        }
    }

    /// <summary>
    /// 透析区分
    /// </summary>
    public int TosekiKbn { get; set; }

    /// <summary>
    /// 透析区分名称
    /// </summary>
    public string TosekiKbnName
    {
        get
        {
            switch (TosekiKbn)
            {
                case 0: return "透析以外";
                case 1: return "透析前";
                case 2: return "透析後";
            }
            return "";
        }
    }

    /// <summary>
    /// 行番号
    /// </summary>
    public string RowNo { get; set; }

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public string ItemCd { get; set; }

    /// <summary>
    /// 診療行為名称
    /// </summary>
    public string ItemName { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public string Suryo { get; set; }

    /// <summary>
    /// 単位
    /// </summary>
    public string UnitName { get; set; }

    /// <summary>
    /// 薬剤区分
    /// </summary>
    public int DrugKbn { get; set; }

    /// <summary>
    /// 処方せん記載区分
    /// </summary>
    public int SyohoKbn { get; set; }

    /// <summary>
    /// 処方せん記載区分名称
    /// </summary>
    public string SyohoKbnName
    {
        get
        {
            if (DrugKbn <= 0) return "";

            switch (SyohoKbn)
            {
                case 0: return "指示なし（後発品のない先発品）";
                case 1: return "変更不可";
                case 2: return "後発品（他銘柄）への変更可";
                case 3: return "一般名処方";
            }
            return "";
        }
    }

    /// <summary>
    /// 処方せん記載制限区分
    /// </summary>
    public int SyohoLimitKbn { get; set; }

    /// <summary>
    /// 処方せん記載制限区分名称
    /// </summary>
    public string SyohoLimitKbnName
    {
        get
        {
            if (DrugKbn <= 0) return "";

            switch (SyohoLimitKbn)
            {
                case 0: return "制限なし";
                case 1: return "剤形不可";
                case 2: return "含量規格不可";
                case 3: return "含量規格・剤形不可";
            }
            return "";
        }
    }

    /// <summary>
    /// 検査項目コード
    /// </summary>
    public string KensaItemCd { get; set; }

    /// <summary>
    /// 外注検査項目コード
    /// </summary>
    public string CenterItemCd { get; set; }

    /// <summary>
    /// 有効期限
    /// </summary>
    public int EndDate { get; set; }

    /// <summary>
    /// 有効期限 (YYYY(GEE)/MM/dd)
    /// </summary>
    public string EndDateFmt
    {
        get => CIUtil.SDateToShowSWDate(EndDate, 0, 1);
    }

    /// <summary>
    /// 期限切れ
    /// </summary>
    public string Expired { get; set; }

    /// <summary>
    /// 連番
    /// </summary>
    public int RenNo { get; set; }


}
