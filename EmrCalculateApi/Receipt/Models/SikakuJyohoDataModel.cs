using Helper.Common;

namespace EmrCalculateApi.Receipt.Models
{
    public class SikakuJyohoDataModel
    {
        /// <summary>
        /// レコード識別情報
        /// </summary>
        public string RecId
        {
            get { return "SN"; }
        }
        /// <summary>
        /// 負担者種別
        /// 1. 医療保険、国民健康保険、退職者医療又は後期高齢者医療
        /// 2.第１公費負担医療
        /// 3.第２公費負担医療
        /// 4.第３公費負担医療
        /// 5.第４公費負担医療
        /// </summary>
        public int FutansyaSbt { get; set; } = 1;
        /// <summary>
        /// 確認区分
        /// 01.保険医療機関・薬局窓口等
        /// </summary>
        public string KakuninKbn { get; set; } = "01";
        /// <summary>
        /// 保険者番号等
        /// </summary>
        public string HokensyaNo { get; set; } = string.Empty;
        /// <summary>
        /// 被保険者記号
        /// </summary>
        public string Kigo { get; set; } = string.Empty;
        /// <summary>
        /// 被保険者番号
        /// </summary>
        public string Bango { get; set; } = string.Empty;
        /// <summary>
        /// 枝番
        /// </summary>
        public string EdaNo { get; set; } = string.Empty;
        /// <summary>
        /// 受給者番号
        /// </summary>
        public string JyukyusyaNo { get; set; } = string.Empty;
        /// <summary>
        /// 予備
        /// </summary>
        public string Yobi1 { get; set; } = string.Empty;
        public string SNRecord
        {
            get
            {
                string ret = "";
        
                // レコード識別
                ret += RecId;
                // 負担者種別
                ret += "," + FutansyaSbt;
                // 確認区分
                ret += "," + KakuninKbn;
                // 保険者番号
                if (string.IsNullOrEmpty(HokensyaNo))
                {
                    ret += ",";
                }
                else
                {
                    ret += string.Format(",{0,8}", HokensyaNo.ToString());
                }
                // 記号
                ret += "," + CIUtil.ToWide(Kigo.Trim()).Replace("　", "");
                // 番号
                ret += "," + CIUtil.ToWide(Bango.Trim()).Replace("　", "");
                // 枝番
                ret += "," + EdaNo.PadLeft(2, '0');
                // 受給者番号
                ret += "," + JyukyusyaNo;
                // 予備
                ret += ",";
                
                return ret;
            }
        }
    }
}
