using CalculateService.Utils;
using Helper.Common;

namespace CalculateService.Receipt.Models;

public class FDataModel
{
    public FDataModel() { }

    /// <summary>
    /// 施設コード(F-1)
    /// </summary>
    public string SisetuCd { get; set; } = string.Empty;
    /// <summary>
    /// データ識別番号(F-2)
    /// </summary>
    public long DataNo { get; set; } = 0;
    public string DataNoDsp
    {
        get
        {
            return DataNo.ToString().PadLeft(10, '0');
        }
    }
    /// <summary>
    /// 生年月日(西暦)(F-3)
    /// </summary>
    public int Birthday { get; set; } = 0;
    /// <summary>
    /// 外来受診年月日(西暦)(F-4)
    /// </summary>
    public int SinDay { get; set; } = 0;
    /// <summary>
    /// データ区分(F-5)
    /// </summary>
    public string DataKbn { get; set; } = string.Empty;
    /// <summary>
    /// 順序番号(F-6)
    /// </summary>
    public int SeqNo { get; set; } = 0;
    public string SeqNoDsp
    {
        get
        {
            return SeqNo.ToString().PadLeft(4, '0');
        }
    }
    ///<summary>
    ///行為明細番号(F-7)
    ///</summary>
    public int RowNo { get; set; } = 0;
    public string RowNoDsp
    {
        get
        {
            return RowNo.ToString().PadLeft(3, '0');
        }
    }
    /// <summary>
    /// 病院点数マスタコード(F-8)
    /// </summary>
    public string MasterItemCd { get; set; } = string.Empty;
    /// <summary>
    /// レセプト電算処理システム用コード(F-9)
    /// </summary>
    public string ReceItemCd { get; set; } = string.Empty;
    ///<summary>
    /// 解釈番号（基本） (F-10)
    ///</summary>
    public string KaisyakuNo { get; set; } = string.Empty;
    /// <summary>
    /// 診療行為名称(F-11)
    /// </summary>
    public string ItemName { get; set; } = string.Empty;
    public string ItemNameDsp
    {
        get
        {
            string ret = ItemName;
            if (string.IsNullOrEmpty(ret) == false)
            {
                try
                {
                    ret = ret.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                }
                catch
                {
                    ret = "";
                }
            }
            return ret;
        }
    }
    ///<summary>
    /// 使用量(F-12)
    ///</summary>
    public double Suryo { get; set; } = 0;
    public string SuryoDsp
    {
        get
        {
            return Suryo.ToString("0000000.000").PadLeft(11, '0');
        }
    }
    /// <summary>
    /// 基準単位(F-13)
    /// </summary>
    public int Unit { get; set; } = 0;
    public string UnitDsp
    {
        get
        {
            string ret = string.Empty;

            ret = Unit.ToString().PadLeft(3, '0');

            return ret;
        }
    }
    /// <summary>
    /// 行為明細点数(F-14)
    /// </summary>
    public double MeisaiTen { get; set; } = 0;
    public string MeisaiTenDsp
    {
        get
        {
            return MeisaiTen.ToString().PadLeft(8, '0');
        }
    }
    /// <summary>
    /// 行為明細薬剤料(F-15)
    /// </summary>
    public double MeisaiYakuzai { get; set; } = 0;
    public string MeisaiYakuzaiDsp
    {
        get
        {
            return CalcUtils.DoubleToAlignmentString(MeisaiYakuzai, 8, 3);
        }
    }
    /// <summary>
    /// 行為明細材料料(F-16)
    /// </summary>
    public double MeisaiZairyo { get; set; } = 0;
    public string MeisaiZairyoDsp
    {
        get
        {
            return CalcUtils.DoubleToAlignmentString(MeisaiZairyo, 8, 3);
        }
    }
    ///<summary>
    ///  円・点区分(F-17)
    ///</summary>
    public int EnTenKbn { get; set; } = 0;
    /// <summary>
    /// 出来高実績点数 (F-18)
    /// </summary>
    public double TotalTen { get; set; } = 0;
    public string TotalTenDsp
    {
        get
        {
            return TotalTen.ToString().PadLeft(8, '0');
        }
    }
    /// <summary>
    /// 行為明細区分情報(F-19)
    /// </summary>
    public string MeisaiKbn
    {
        get
        {
            return $"{OutDrug}{IpnDrug}{Sex}{TenkiKbn}{SyubyoKbn}{HoukatuKbn}{Refill}00000";
        }
    }
    /// <summary>
    /// ①院外処方区分
    /// 院外処方は「1」、院内処方は「0」
    /// </summary>
    public int OutDrug { get; set; } = 0;
    /// <summary>
    /// ②一般名処方区分
    /// 院内処方の薬剤は「0」、院外処方で一般名処方を行った場合は「1」、その他は「0」
    /// </summary>
    public int IpnDrug { get; set; } = 0;
    /// <summary>
    /// ③性別
    /// 男性は「1」を、女性は「2」
    /// </summary>
    public int Sex { get; set; } = 0;
    /// <summary>
    /// ④転帰区分
    /// 傷病に関するレコードに対しては、当該傷病名に係る以下の表の転帰区分を入力
    /// 傷病に関するレコードではない場合は「0」
    　      /// 1 治癒、死亡、中止以外 、2 治癒、3 死亡 、4 中止（転医） 
    /// </summary>
    public int TenkiKbn { get; set; } = 0;
    /// <summary>
    /// ⑤主傷病
    /// 傷病に関するレコードで、当該傷病が主傷病である場合は「1」、それ以外の場合は「0」
    /// </summary>
    public int SyubyoKbn { get; set; } = 0;
    /// <summary>
    /// ⑥医学管理料等包括項目区分
    /// 特定の医学管理料等に包括される診療項目（薬剤、材料を含む。）は「1」、それ以外には「0」
    /// </summary>
    public int HoukatuKbn { get; set; } = 0;
    /// <summary>
    /// ⑦リフィル処方箋区分
    /// </summary>
    public double Refill { get; set; } = 0;
    /// <summary>
    /// 書式区分
    /// </summary>
    public int FmtKbn { get; set; } = 0;
    /// <summary>
    /// Fファイルの行データ
    /// </summary>
    public string FData
    {
        get
        {
            string ret = string.Empty;

            // 施設コード(F-1)
            ret += SisetuCd + "\t";
            // データ識別番号(F-2)
            ret += DataNoDsp + "\t";
            // 生年月日(西暦)(F-3)
            ret += Birthday.ToString() + "\t";
            // 外来受診年月日(西暦)(F-4)
            ret += SinDay.ToString() + "\t";
            // データ区分(F-5)
            ret += DataKbn + "\t";
            // 順序番号(F-6)
            ret += SeqNoDsp + "\t";
            //行為明細番号(F-7)
            ret += RowNoDsp + "\t";
            // 病院点数マスタコード(F-8)
            ret += MasterItemCd + "\t";
            // レセプト電算処理システム用コード(F-9)
            ret += ReceItemCd + "\t";
            // 解釈番号（基本） (F-10)
            ret += KaisyakuNo + "\t";
            // 診療行為名称(F-11)
            string refText = "";
            string badText = "";
            string commentData = ItemNameDsp;

            if (CIUtil.IsUntilJISKanjiLevel2(commentData, ref refText, ref badText) == false)
            {
                //Log.WriteLogMsg("FDataModel", this, "FData",
                //    string.Format("CommentData is include bad charcter PtId:{0} Comment:{1} badCharcters:{2}",
                //        DataNoDsp, commentData, badText));

                commentData = refText;
            }

            ret += commentData + "\t";
            // 使用量(F-12)
            ret += SuryoDsp + "\t";
            // 基準単位(F-13)
            ret += UnitDsp + "\t";
            // 行為明細点数(F-14)
            ret += MeisaiTenDsp + "\t";
            // 行為明細薬剤料(F-15)
            ret += MeisaiYakuzaiDsp + "\t";
            // 行為明細材料料(F-16)
            ret += MeisaiZairyoDsp + "\t";
            //  円・点区分(F-17)
            ret += EnTenKbn.ToString() + "\t";
            // 出来高実績点数 (F-18)
            ret += TotalTenDsp + "\t";
            // 行為明細区分情報(F-19)
            ret += MeisaiKbn;

            return ret;
        }
    }
}
