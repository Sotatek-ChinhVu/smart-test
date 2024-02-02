using CalculateService.Utils;
using Helper.Common;

namespace CalculateService.Receipt.Models;

internal class EFDataModel
{
    public EFDataModel() { }

    /// <summary>
    /// 施設コード(EF-1)
    /// </summary>
    public string SisetuCd { get; set; } = string.Empty;
    /// <summary>
    /// データ識別番号(EF-2)
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
    /// 生年月日(西暦)(EF-3)
    /// </summary>
    public int Birthday { get; set; } = 0;
    /// <summary>
    /// 外来受診年月日(西暦)(EF-4)
    /// </summary>
    public int SinDay { get; set; } = 0;
    /// <summary>
    /// データ区分(EF-5)
    /// </summary>
    public string DataKbn { get; set; } = string.Empty;
    /// <summary>
    /// 順序番号(EF-6)
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
    ///行為明細番号(EF-7)
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
    /// 病院点数マスタコード(EF-8)
    /// </summary>
    public string MasterItemCd { get; set; } = string.Empty;
    /// <summary>
    /// レセプト電算処理システム用コード(EF-9)
    /// </summary>
    public string ReceItemCd { get; set; } = string.Empty;
    ///<summary>
    /// 解釈番号（基本） (EF-10)
    ///</summary>
    public string KaisyakuNo { get; set; } = string.Empty;
    /// <summary>
    /// 診療行為名称(E-11)
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
    /// 使用量(EF-12)
    ///</summary>
    public double Suryo { get; set; } = 0;
    public string SuryoDsp
    {
        get
        {
            return CalcUtils.DoubleToAlignmentString(Suryo, 7, 3);
        }
    }
    /// <summary>
    /// 基準単位(EF-13)
    /// </summary>
    public int Unit { get; set; } = 0;
    public string UnitDsp
    {
        get { return Unit.ToString().PadLeft(3, '0'); }
    }
    ///<summary>
    /// 明細点数・金額(EF-14)
    ///</summary>
    public double MeisaiTen { get; set; } = 0;
    public string MeisaiTenDsp
    {
        get
        {
            if (DataKbn == "SY")
            {
                return "00000000";
            }
            else
            {
                return CalcUtils.DoubleToAlignmentString(MeisaiTen, 8, 3);
            }
        }
    }
    ///<summary>
    ///  円・点区分(EF-15)
    ///</summary>
    public int EnTenKbn { get; set; } = 0;
    /// <summary>
    /// 出来高実績点数 (EF-16)
    /// </summary>
    public double TotalTen { get; set; } = 0;
    public string TotalTenDsp
    {
        get
        {
            return CalcUtils.DoubleToAlignmentString(TotalTen, 8, 0);
        }
    }
    /// <summary>
    /// 行為明細区分情報(EF-17)
    /// </summary>
    public string MeisaiKbn
    {
        get
        {
            return $"{OutDrug}{IpnDrug}{Sex}{TenkiKbn}{SyubyoKbn}{HoukatuKbn}{Refill}00000";
        }
    }
    /// <summary>
    /// 行為点数(EF-18)
    /// </summary>
    public double KouiTen { get; set; } = 0;
    public string KouiTenDsp
    {
        get
        {
            return CalcUtils.DoubleToAlignmentString(KouiTen, 8, 0);
        }
    }
    /// <summary>
    /// 行為薬剤料(EF-19)
    /// </summary>
    public double KouiYakuzai { get; set; } = 0;
    public string KouiYakuzaiDsp
    {
        get
        {
            return CalcUtils.DoubleToAlignmentString(KouiYakuzai, 8, 0);
        }
    }
    /// <summary>
    /// 行為材料料(EF-20)
    /// </summary>
    public double KouiZairyo { get; set; } = 0;
    public string KouiZairyoDsp
    {
        get
        {
            return CalcUtils.DoubleToAlignmentString(KouiZairyo, 8, 0);
        }
    }
    /// <summary>
    /// 行為回数(EF-21)
    /// </summary>
    public int KouiCount { get; set; } = 0;
    public string KouiCountDsp
    {
        get
        {
            return KouiCount.ToString().PadLeft(3, '0');
        }
    }
    /// <summary>
    /// 保険者番号(EF-22)
    /// </summary>
    public string HokensyaNo { get; set; } = string.Empty;
    /// <summary>
    /// レセプト種別コード(EF-23)
    /// </summary>
    public string ReceSbt { get; set; } = string.Empty;
    /// <summary>
    /// 実施年月日・診療開始日(EF-24)
    /// </summary>
    public int JissiDay { get; set; } = 0;
    /// <summary>
    /// レセプト科区分(EF-25)
    /// </summary>
    public string ReceKaCd { get; set; } = string.Empty;
    /// <summary>
    /// 診療科区分(EF-26)
    /// </summary>
    public string SinKaCd { get; set; } = string.Empty;
    public string SinKaCdDsp
    {
        get { return (string.IsNullOrEmpty(SinKaCd) ? "000" : SinKaCd.PadLeft(3, '0')); }
    }
    /// <summary>
    /// 医師コード(EF-27)
    /// </summary>
    public int DrCd { get; set; } = 0;
    /// <summary>
    /// 病棟コード(EF-28)
    /// </summary>
    public long ByotoCd { get { return 9999999999; } }
    /// <summary>
    /// 病棟区分(EF-29)
    /// </summary>
    public int ByotoKbn { get { return 9; } }
    /// <summary>
    /// 入外区分(EF-30)
    /// </summary>
    public int InoutKbn { get { return 1; } }
    /// <summary>
    /// 施設タイプ(EF-31)
    /// </summary>
    public string SisetuType { get { return ""; } }
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
    /// EFファイルの行データ
    /// </summary>
    public string EFData
    {
        get
        {
            string ret = string.Empty;

            // 施設コード(EF-1)
            ret += SisetuCd + "\t";
            // データ識別番号(EF-2)
            ret += DataNoDsp + "\t";
            // 生年月日(西暦)(EF-3)
            ret += Birthday.ToString() + "\t";
            // 外来受診年月日(西暦)(EF-4)
            ret += SinDay.ToString() + "\t";
            // データ区分(EF-5)
            ret += DataKbn + "\t";
            // 順序番号(EF-6)
            ret += SeqNoDsp + "\t";
            //行為明細番号(EF-7)
            ret += RowNoDsp + "\t";
            // 病院点数マスタコード(EF-8)
            ret += MasterItemCd + "\t";
            // レセプト電算処理システム用コード(EF-9)
            ret += ReceItemCd + "\t";
            // 解釈番号（基本） (EF-10)
            ret += KaisyakuNo + "\t";
            // 診療行為名称(E-11)
            string refText = "";
            string badText = "";
            string commentData = ItemNameDsp;

            if (CIUtil.IsUntilJISKanjiLevel2(commentData, ref refText, ref badText) == false)
            {
                //Log.WriteLogMsg("EFDataModel", this, "EFData",
                //    string.Format("CommentData is include bad charcter PtId:{0} Comment:{1} badCharcters:{2}",
                //        DataNoDsp, commentData, badText));

                commentData = refText;
            }

            ret += commentData + "\t";
            // 使用量(EF-12)
            ret += SuryoDsp + "\t";
            // 基準単位(EF-13)
            ret += UnitDsp + "\t";
            // 明細点数・金額(EF-14)
            ret += MeisaiTenDsp + "\t";
            //  円・点区分(EF-15)
            ret += EnTenKbn + "\t";
            // 出来高実績点数 (EF-16)
            ret += TotalTenDsp + "\t";
            // 行為明細区分情報(EF-17)
            if (RowNo == 0)
            {
                ret += "\t";
            }
            else
            {
                ret += MeisaiKbn + "\t";
            }
            // 行為点数(EF-18)
            ret += KouiTenDsp + "\t";
            // 行為薬剤料(EF-19)
            ret += KouiYakuzaiDsp + "\t";
            // 行為材料料(EF-20)
            ret += KouiZairyoDsp + "\t";
            // 行為回数(EF-21)
            ret += KouiCountDsp + "\t";
            // 保険者番号(EF-22)
            ret += HokensyaNo + "\t";
            // レセプト種別コード(EF-23)
            ret += ReceSbt + "\t";
            // 実施年月日・診療開始日(EF-24)
            ret += JissiDay + "\t";
            // レセプト科区分(EF-25)
            ret += ReceKaCd.ToString() + "\t";
            // 診療科区分(EF-26)
            ret += SinKaCdDsp + "\t";
            // 医師コード(EF-27)
            ret += DrCd.ToString() + "\t";
            // 病棟コード(EF-28)
            ret += ByotoCd + "\t";
            // 病棟区分(EF-29)
            ret += ByotoKbn + "\t";
            // 入外区分(EF-30)
            if (RowNo == 0)
            {
                ret += InoutKbn + "\t";
            }
            else
            {
                ret += "\t";
            }
            // 施設タイプ(EF-31)
            ret += SisetuType;

            return ret;
        }
    }
}
