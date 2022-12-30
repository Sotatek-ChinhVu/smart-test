using Entity.Tenant;
using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class HokenDataModel
    {
        public PtHokenInf PtHokenInf { get; } = null;

        private int? _jituNissu;
        private int? _totalTen;
        private int? _futanKingaku;
        

        public HokenDataModel(PtHokenInf ptHokenInf)
        {
            PtHokenInf = ptHokenInf;
        }

        /// <summary>
        /// レコード識別情報
        /// </summary>
        public string RecId
        {
            get { return "HO"; }
        }
        /// <summary>
        /// 保険者番号
        /// </summary>
        public string HokensyaNo
        {
            get { return PtHokenInf.HokensyaNo ?? string.Empty; }
        }
        /// <summary>
        /// 被保険者証等の記号
        /// </summary>
        public string Kigo
        {
            get { return CIUtil.ToWide(PtHokenInf.Kigo ?? string.Empty); }
        }
        /// <summary>
        /// 被保険者証等の番号
        /// </summary>
        public string Bango
        {
            get { return CIUtil.ToWide(PtHokenInf.Bango ?? string.Empty); }
        }
        /// <summary>
        /// 被保険者証等の枝番
        /// </summary>
        public string EdaNo
        {
            get
            {
                string ret = "";
                if (string.IsNullOrEmpty(PtHokenInf.EdaNo) == false)
                {
                    ret = PtHokenInf.EdaNo.PadLeft(2, '0');
                }
                return ret;
            }
        }
        /// <summary>
        /// 診療実日数
        /// </summary>
        public int? JituNissu
        {
            get { return _jituNissu; }
            set { _jituNissu = value; }
        }
        /// <summary>
        /// 合計点数
        /// </summary>
        public int? TotalTen
        {
            get { return _totalTen; }
            set { _totalTen = value; }
        }
        /// <summary>
        /// 職務上の事由
        /// </summary>
        public int SyokumuKbn
        {
            get { return PtHokenInf.SyokumuKbn; }
        }
        /// <summary>
        /// 負担金額    医療保険
        /// </summary>
        public int? FutanKingaku
        {
            get { return _futanKingaku; }
            set { _futanKingaku = value; }
        }
        /// <summary>
        /// 減免区分
        /// </summary>
        public int GenmenKbn
        {
            get
            {
                int ret = 0;
                if (PtHokenInf.GenmenKbn <= 3)
                {
                    ret = PtHokenInf.GenmenKbn;
                }
                return ret;
            }
        }
        /// <summary>
        /// 減免割合
        /// </summary>
        public int GenmenRate
        {
            get{return PtHokenInf.GenmenRate; }
        }
        /// <summary>
        /// 減免額
        /// </summary>
        public int GenmenGaku
        {
            get { return PtHokenInf.GenmenGaku; }
        }

        public int HokenId
        {
            get { return PtHokenInf.HokenId;  }
        }

        public int HokenNo
        {
            get { return PtHokenInf.HokenNo; }
        }
        /// <summary>
        /// 本人家族区分
        ///     0:本人
        ///     1:家族
        /// </summary>
        public int HonkeKbn
        {
            get { return PtHokenInf.HonkeKbn; }
        }

        /// <summary>
        /// 労災交付番号
        /// </summary>
        public string RousaiKofu
        {
            get => PtHokenInf.RousaiKofuNo ?? "";
        }
        /// <summary>
        /// 傷病開始日
        /// </summary>
        public int RousaiSyobyoDate
        {
            get => PtHokenInf.RousaiSyobyoDate;
        }
        /// <summary>
        /// HOレコード
        /// </summary>
        public string HoRecord
        {
            get
            {
                string ret = "";

                // 保険No=100は主保険なし
                // レコード識別
                ret += RecId;
                // 保険者番号
                ret += string.Format(",{0,8}", HokensyaNo.ToString());
                // 記号
                ret += "," + CIUtil.ToWide(Kigo.Trim()).Replace("　", "");
                // 番号
                ret += "," + CIUtil.ToWide(Bango.Trim()).Replace("　", "");
                // 診療実日数
                ret += "," + JituNissu?.ToString() ?? "";
                // 合計点数
                ret += "," + TotalTen?.ToString() ?? "";
                // 予備
                ret += ",";
                // 食事療養・生活療養    回数
                ret += ",";
                // 食事療養・生活療養    合計金額
                ret += ",";
                // 職務上の事由
                ret += "," + CIUtil.ToStringIgnoreZero(SyokumuKbn);
                // 証明書番号
                ret += ",";
                // 負担金額 医療保険
                ret += "," + CIUtil.ToStringIgnoreNull(FutanKingaku);
                // 負担金額 減免区分
                ret += "," + CIUtil.ToStringIgnoreZero(GenmenKbn);
                // 負担金額 減額割合
                ret += "," + CIUtil.ToStringIgnoreZero(GenmenRate);
                // 負担金額 減額金額
                ret += "," + CIUtil.ToStringIgnoreZero(GenmenGaku);

                return ret;
            }
        }
    }
}
