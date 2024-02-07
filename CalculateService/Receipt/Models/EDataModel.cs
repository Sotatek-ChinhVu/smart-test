using CalculateService.Utils;
using Helper.Common;

namespace CalculateService.Receipt.Models;

public class EDataModel
{
    public EDataModel() { }

    /// <summary>
    /// 施設コード(E-1)
    /// </summary>
    public string SisetuCd { get; set; } = string.Empty;
    /// <summary>
    /// データ識別番号(E-2)
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
    /// 生年月日(西暦)(E-3)
    /// </summary>
    public int Birthday { get; set; } = 0;
    /// <summary>
    /// 外来受診年月日(西暦)(E-4)
    /// </summary>
    public int SinDay { get; set; } = 0;
    /// <summary>
    /// データ区分(E-5)
    /// </summary>
    public string DataKbn { get; set; } = string.Empty;
    /// <summary>
    /// 順序番号(E-6)
    /// </summary>
    public int SeqNo { get; set; } = 0;
    public string SeqNoDsp
    {
        get
        {
            return SeqNo.ToString().PadLeft(4, '0');
        }
    }
    /// <summary>
    /// 病院点数マスタコード(E-7)
    /// </summary>
    public string MasterItemCd { get; set; } = string.Empty;
    /// <summary>
    /// レセプト電算処理システム用コード(E-8)
    /// </summary>
    public string ReceItemCd { get; set; } = string.Empty;
    ///<summary>
    /// 解釈番号（基本） (E-9)
    ///</summary>
    public string KaisyakuNo { get; set; } = string.Empty;
    /// <summary>
    /// 診療行為名称(E-10)
    /// </summary>
    public string ItemName { get; set; } = string.Empty;
    /// <summary>
    /// 行為点数(E-11)
    /// </summary>
    public double KouiTen { get; set; } = 0;
    public string KouiTenDsp
    {
        get
        {
            //if (DataKbn == "SY")
            //{
            //    return "0";
            //}
            //else if (InoutKbn == 1)
            //{
            //    return "0";
            //}
            //else
            //{
            return CalcUtils.DoubleToAlignmentString(KouiTen, 8, 0);
            //}
        }
    }
    /// <summary>
    /// 行為薬剤料(E-12)
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
    /// 行為材料料(E-13)
    /// </summary>
    public double KouiZairyo { get; set; } = 0;
    public string KouiZairyoDsp
    {
        get
        {
            return CalcUtils.DoubleToAlignmentString(KouiZairyo, 8, 0);
        }
    }
    ///<summary>
    ///  円・点区分(E-14)
    ///</summary>
    public int EnTenKbn { get; set; } = 0;
    /// <summary>
    /// 行為回数(E-15)
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
    /// 保険者番号(E-16)
    /// </summary>
    public string HokensyaNo { get; set; } = string.Empty;
    /// <summary>
    /// レセプト種別コード(E-17)
    /// </summary>
    public string ReceSbt { get; set; } = string.Empty;
    /// <summary>
    /// 実施年月日・診療開始日(E-18)
    /// </summary>
    public int JissiDay { get; set; } = 0;
    /// <summary>
    /// レセプト科区分(E-19)
    /// </summary>
    public string ReceKaCd { get; set; } = string.Empty;
    public string ReceKaCdDsp
    {
        get { return ReceKaCd.PadLeft(2, '0'); }
    }
    /// <summary>
    /// 診療科区分(E-20)
    /// </summary>
    public string SinKaCd { get; set; } = string.Empty;
    public string SinKaCdDsp
    {
        get { return (string.IsNullOrEmpty(SinKaCd) ? "000" : SinKaCd.PadLeft(3, '0')); }
    }
    /// <summary>
    /// 医師コード(E-21)
    /// </summary>
    public int DrCd { get; set; } = 0;
    /// <summary>
    /// 病棟コード(E-22)
    /// </summary>
    public long ByotoCd { get { return 9999999999; } }
    /// <summary>
    /// 病棟区分(E-23)
    /// </summary>
    public int ByotoKbn { get { return 9; } }
    /// <summary>
    /// 入外区分(E-24)
    /// </summary>
    public int InoutKbn { get { return 1; } }
    /// <summary>
    /// 施設タイプ(E-25)
    /// </summary>
    public string SisetuType { get { return ""; } }
    /// <summary>
    /// Eファイルの行データ
    /// </summary>
    public string EData
    {
        get
        {
            string ret = string.Empty;

            ret += SisetuCd + "\t";
            ret += DataNoDsp + "\t";
            ret += Birthday.ToString() + "\t";
            ret += SinDay.ToString() + "\t";
            ret += DataKbn + "\t";
            ret += SeqNoDsp + "\t";
            ret += MasterItemCd + "\t";
            ret += ReceItemCd + "\t";
            ret += KaisyakuNo + "\t";

            string refText = "";
            string badText = "";
            string commentData = ItemName;

            if (CIUtil.IsUntilJISKanjiLevel2(commentData, ref refText, ref badText) == false)
            {
                //Log.WriteLogMsg("EDataModel", this, "EData",
                //    string.Format("CommentData is include bad charcter PtId:{0} Comment:{1} badCharcters:{2}",
                //        DataNoDsp, commentData, badText));

                commentData = refText;
            }

            ret += commentData + "\t";
            ret += KouiTenDsp + "\t";
            ret += KouiYakuzaiDsp + "\t";
            ret += KouiZairyoDsp + "\t";
            ret += EnTenKbn + "\t";
            ret += KouiCountDsp + "\t";
            ret += HokensyaNo + "\t";
            ret += ReceSbt + "\t";
            ret += JissiDay + "\t";
            ret += ReceKaCd.ToString() + "\t";
            ret += SinKaCdDsp + "\t";
            ret += DrCd.ToString() + "\t";
            ret += ByotoCd + "\t";
            ret += ByotoKbn + "\t";
            ret += InoutKbn + "\t";
            ret += SisetuType;

            return ret;
        }
    }
}
