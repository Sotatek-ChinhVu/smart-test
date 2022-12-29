using Helper.Constants;
using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class OdrInfDataModel
    {
        private int _hpId = 0;
        private long _ptId = 0;
        private int _sinDate = 0;
        private long _raiinNo = 0;
        private long _rpNo = 0;
        private long _rpEdaNo = 0;
        private int _hokenId = 0;
        private int _odrKouiKbn = 0;
        private int _inoutKbn = 0;
        private int _sikyuKbn = 0;
        private int _syohoSbt = 0;
        private int _santeiKbn = 0;
        private int _daysCnt = 0;
        private int _sortNo = 0;
        private int _isDeleted = 0;

        public OdrInfDataModel(OdrInf odrInf)
        {
            _hpId = odrInf.HpId;
            _ptId = odrInf.PtId;
            _sinDate = odrInf.SinDate;
            _raiinNo = odrInf.RaiinNo;
            _rpNo = odrInf.RpNo;
            _rpEdaNo = odrInf.RpEdaNo;
            _hokenId = odrInf.HokenPid;
            _odrKouiKbn = odrInf.OdrKouiKbn;
             _inoutKbn = odrInf.InoutKbn;
             _sikyuKbn = odrInf.SikyuKbn;
             _syohoSbt = odrInf.SyohoSbt;
             _santeiKbn = odrInf.SanteiKbn;
             _daysCnt = odrInf.DaysCnt;
             _sortNo = odrInf.SortNo;
             _isDeleted = odrInf.IsDeleted;
        }

        public OdrInfDataModel(
            int hpId, long ptId, int sinDate, long raiinNo, 
            long rpNo, long rpEdaNo, 
            int hokenId, int odrKouiKbn, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, 
            int daysCnt, int sortNo, int isDeleted)
        {
            _hpId = hpId;
            _ptId = ptId;
            _sinDate = sinDate;
            _raiinNo = raiinNo;
            _rpNo = rpNo;
            _rpEdaNo = rpEdaNo;
            _hokenId = hokenId;
            _odrKouiKbn = odrKouiKbn;
            _inoutKbn = inoutKbn;
            _sikyuKbn = sikyuKbn;
            _syohoSbt = syohoSbt;
            _santeiKbn = santeiKbn;
            _daysCnt = daysCnt;
            _sortNo = sortNo;
            _isDeleted = isDeleted;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return _hpId; }
        }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return _ptId; }
        }

        /// <summary>
        /// 診療日
        ///     yyyymmdd
        /// </summary>
        public int SinDate
        {
            get { return _sinDate; }
        }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo
        {
            get { return _raiinNo; }
        }

        /// <summary>
        /// 剤番号
        /// </summary>
        public long RpNo
        {
            get { return _rpNo; }
            set { _rpNo = value; }
        }

        /// <summary>
        /// 剤枝番
        ///     剤に変更があった場合、カウントアップ
        /// </summary>
        public long RpEdaNo
        {
            get { return _rpEdaNo; }
            set { _rpEdaNo = value; }
        }

        /// <summary>
        /// 保険組合せID
        /// </summary>
        public int HokenPid
        {
            get { return _hokenId; }
        }

        /// <summary>
        /// オーダー行為区分
        /// </summary>
        public int OdrKouiKbn
        {
            get { return _odrKouiKbn; }
        }

        /// <summary>
        /// 院内院外区分
        ///     0: 院内
        ///     1: 院外
        /// </summary>
        public int InoutKbn
        {
            get { return _inoutKbn; }
        }

        /// <summary>
        /// 至急区分
        ///     0: 通常 
        ///     1: 至急
        /// </summary>
        public int SikyuKbn
        {
            get { return _sikyuKbn; }
        }

        /// <summary>
        /// 処方種別
        ///     0: 日数判断
        ///     1: 臨時
        ///     2: 常態
        /// </summary>
        public int SyohoSbt
        {
            get { return _syohoSbt; }
        }

        /// <summary>
        /// 算定区分
        ///     1: 算定外
        ///     2: 自費算定
        /// </summary>
        public int SanteiKbn
        {
            get { return _santeiKbn; }
        }

        /// <summary>
        /// 日数回数
        ///     処方日数
        /// </summary>
        public int DaysCnt
        {
            get { return _daysCnt; }
        }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo
        {
            get { return _sortNo; }
        }

        /// <summary>
        /// 削除区分
        ///     1:削除
        ///     2:未確定削除
        /// </summary>
        public int IsDeleted
        {
            get { return _isDeleted; }
        }

    }

}
