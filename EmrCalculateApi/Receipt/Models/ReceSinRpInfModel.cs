using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmrCalculateApi.Ika.Models;
namespace EmrCalculateApi.Receipt.Models
{
    class ReceSinRpInfModel
    {
        private int _isDeleted = 0;
        private long _keyNo = 0;

        private int _hpId;
        private long _ptId;
        private int _sinYm;
        private int _rpNo;
        private int _firstDay;
        private int _hokenKbn;
        private int _sinKouiKbn;
        private int _sinId;
        private string _cdNo;
        private int _santeiKbn;
        private string _kouiData;
        
        public ReceSinRpInfModel(SinRpInfModel sinRpInfModel)
        {
            _hpId = sinRpInfModel.HpId;
            _ptId = sinRpInfModel.PtId;
            _sinYm = sinRpInfModel.SinYm;
            _rpNo = sinRpInfModel.RpNo;
            _firstDay = sinRpInfModel.FirstDay;
            _hokenKbn = sinRpInfModel.HokenKbn;
            _sinKouiKbn = sinRpInfModel.SinKouiKbn;
            _sinId = sinRpInfModel.SinId;
            _cdNo = sinRpInfModel.CdNo;
            _santeiKbn = sinRpInfModel.SanteiKbn;
            _kouiData = sinRpInfModel.KouiData;
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
        /// SEQUENCE
        /// </summary>
        public int RpNo
        {
            get => _rpNo;
        }

        /// <summary>
        /// 初回算定日
        /// </summary>
        public int FirstDay
        {
            get => _firstDay;
            set
            {
                _firstDay = value;
            }
        }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        public int HokenKbn
        {
            get => _hokenKbn;
        }

        /// <summary>
        /// 診療行為区分
        /// 
        /// </summary>
        public int SinKouiKbn
        {
            get => _sinKouiKbn;
        }

        /// <summary>
        /// 診療識別
        /// レセプト電算に記録する診療識別
        /// </summary>
        public int SinId
        {
            get => _sinId;
        }

        /// <summary>
        /// 代表コード表用番号
        /// 
        /// </summary>
        public string CdNo
        {
            get => _cdNo;
        }

        /// <summary>
        /// 算定区分
        /// 1:自費算定
        /// </summary>
        public int SanteiKbn
        {
            get => _santeiKbn;
        }

        /// <summary>
        /// 診療行為インデックス
        /// RP_NOに属する診療行為のKOUI_INDEXを結合したもの
        /// </summary>
        public string KouiData
        {
            get => _kouiData;
            set
            {
                _kouiData = value;
            }
        }

        public string SortKouiData
        {
            get
            {
                string ret = "";

                if (CdNo.Length == 8 && CdNo.Substring(1, 7) == "9999999")
                {
                    ret = _kouiData;
                }

                return ret;
            }
        }

        /// <summary>
        /// 削除区分
        /// </summary>
        public int IsDeleted
        {
            get => _isDeleted;
        }
    }


}
