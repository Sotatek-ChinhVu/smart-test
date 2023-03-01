using Entity.Tenant;
using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class AfterCareReceiptModel
    {
        public PtHokenInf PtHokenInf { get; } = null;
        public PtRousaiTenki PtRousaiTenki { get; } = null;
        public PtInf PtInf { get; } = null;

        private int _rousaiCount;

        private int _ryoyoFirstDate;
        private int _ryoyoLastDate;
        private int? _jituNissu;
        private string _syobyoKeika;
        private int? _syokei;
        private int _syokeiGaku_I;
        private int _syokeiGaku_Ro;
        private int _outputYm;

        public AfterCareReceiptModel(PtHokenInf ptHokenInf, PtRousaiTenki ptRousaiTenki, PtInf ptInf, int rousaiCount, int outputYm)
        {
            PtHokenInf = ptHokenInf;
            PtRousaiTenki = ptRousaiTenki;
            PtInf = ptInf;
            _rousaiCount = rousaiCount;

            _outputYm = outputYm;
        }

        /// <summary>
        /// レコード識別情報
        /// </summary>
        public string RecId
        {
            get { return "AR"; }
        }

        /// <summary>
        /// 帳票種別
        ///     2	アフターケア委託費請求内訳書
        /// </summary>
        public int CyohyoSbt
        {
            get
            {
                return 2;
            }
        }
        /// <summary>
        /// 傷病コード
        /// </summary>
        public string SyobyoCd
        {
            get => PtHokenInf.RousaiSyobyoCd;
        }
        /// <summary>
        /// 健康管理手帳番号
        /// </summary>
        public string KenkoKanriNo
        {
            get
            {            
                return PtHokenInf.RousaiKofuNo ?? string.Empty;
            }
        }
        
        /// <summary>
        /// 傷病年月日
        /// </summary>
        public int SyobyoDate
        {
            get
            {
                return PtHokenInf.RousaiSyobyoDate;
            }
        }
        /// <summary>
        /// 前回の検査年月日
        /// </summary>
        public int ZenkaiKensaDate { get; set; } = 0;

        /// <summary>
        /// 診療年月日
        /// </summary>
        public int SinDate { get; set; } = 0;

        /// <summary>
        /// 検査年月日（健康診断年月日）
        /// </summary>
        public int KensaDate { get; set; } = 0;        
        /// <summary>
        /// 労働者の氏名（カナ）
        /// </summary>
        public string PtKanaName
        {
            get
            {
                return CIUtil.ToWide(CIUtil.KanaUpper((PtInf.KanaName ?? string.Empty).ToUpper()));
            }
        }
        /// <summary>
        /// 傷病の経過
        /// </summary>
        public string SyobyoKeika
        {
            get
            {
                return _syobyoKeika ?? string.Empty;
            }
            set
            {
                _syobyoKeika = value;
            }
        }


        /// <summary>
        /// 小計点数
        /// </summary>
        public int? Syokei
        {
            get
            {
                return _syokei;
            }
            set
            {
                _syokei = value;
            }
        }
        /// <summary>
        /// 小計点数金額換算（イ）
        /// </summary>
        public int SyokeiGaku_I
        {
            get
            {
                return _syokeiGaku_I;
            }
            set
            {
                _syokeiGaku_I = value;
            }
        }
        /// <summary>
        /// 小計金額（ロ）
        /// </summary>
        public int SyokeiGaku_RO
        {
            get
            {
                return _syokeiGaku_Ro;
            }
            set
            {
                _syokeiGaku_Ro = value;
            }
        }
        /// <summary>
        /// 合計額（イ）＋（ロ）＋（ハ）
        /// </summary>
        public int Gokei
        {
            get
            {
                return _syokeiGaku_I + _syokeiGaku_Ro;
            }
        }

        /// <summary>
        /// ARレコード
        /// </summary>
        public string ARRecord
        {
            get
            {
                string ret = "";

                // レコード識別
                ret += RecId;
                //予備１
                ret += ",";
                //予備２
                ret += ",";
                //帳票種別
                ret += "," + CyohyoSbt.ToString();
                //傷病コード
                ret += "," + SyobyoCd.ToString();
                //健康管理手帳番号
                ret += "," + KenkoKanriNo;
                //前回検査日
                ret += "," + CIUtil.ToStringIgnoreZero(ZenkaiKensaDate);
                //予備３
                ret += ",";
                //予備４
                ret += ",";
                //診療年月日
                ret += "," + SinDate.ToString();
                //検査年月日
                if (SyobyoCd.ToString() == "00")
                {
                    ret += ",";
                }
                else
                {
                    ret += "," + CIUtil.ToStringIgnoreZero(KensaDate);
                }
                //予備５
                ret += ",";
                //労働者の氏名（カナ）
                ret += "," + CIUtil.Copy(PtKanaName, 1, 20);
                //予備６
                ret += ",";
                //予備７
                ret += ",";
                //傷病の経過
                ret += "," + CIUtil.Copy(SyobyoKeika, 1, 50);
                //小計点数
                ret += "," + CIUtil.ToStringIgnoreNull(Syokei);
                //小計点数金額換算（イ）
                ret += "," + SyokeiGaku_I.ToString();
                //小計金額（ロ）
                ret += "," + SyokeiGaku_RO.ToString();
                //予備８
                ret += ",";
                //予備９
                ret += ",";
                //合計額（イ）＋（ロ）＋（ハ）
                ret += "," + Gokei.ToString();

                return ret;
            }
        }
    }
}
