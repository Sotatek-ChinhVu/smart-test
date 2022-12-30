using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class SinRpNoInfModel
    {
        public SinRpNoInf SinRpNoInf { get; } = null;

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

        public SinRpNoInfModel(SinRpNoInf sinRpNoInf)
        {
            SinRpNoInf = sinRpNoInf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SinRpNoInf.HpId; }
            set
            {
                if (SinRpNoInf.HpId == value) return;
                SinRpNoInf.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return SinRpNoInf.PtId; }
            set
            {
                if (SinRpNoInf.PtId == value) return;
                SinRpNoInf.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return SinRpNoInf.SinYm; }
            set
            {
                if (SinRpNoInf.SinYm == value) return;
                SinRpNoInf.SinYm = value;
                //RaisePropertyChanged(() => SinYm);
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDay
        {
            get { return SinRpNoInf.SinDay; }
            set
            {
                if (SinRpNoInf.SinDay == value) return;
                SinRpNoInf.SinDay = value;
                //RaisePropertyChanged(() => SinDay);
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return SinRpNoInf.RaiinNo; }
            set
            {
                if (SinRpNoInf.RaiinNo == value) return;
                SinRpNoInf.RaiinNo = value;
                //RaisePropertyChanged(() => RaiinNo);
            }
        }

        /// <summary>
        /// 剤番号
        /// SEQUENCE
        /// </summary>
        public int RpNo
        {
            get { return SinRpNoInf.RpNo; }
            set
            {
                if (SinRpNoInf.RpNo == value) return;
                SinRpNoInf.RpNo = value;
                //RaisePropertyChanged(() => RpNo);
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return SinRpNoInf.CreateDate; }
            set
            {
                if (SinRpNoInf.CreateDate == value) return;
                SinRpNoInf.CreateDate = value;
                //RaisePropertyChanged(() => CreateDate);
            }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return SinRpNoInf.CreateId; }
            set
            {
                if (SinRpNoInf.CreateId == value) return;
                SinRpNoInf.CreateId = value;
                //RaisePropertyChanged(() => CreateId);
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return SinRpNoInf.CreateMachine ?? string.Empty; }
            set
            {
                if (SinRpNoInf.CreateMachine == value) return;
                SinRpNoInf.CreateMachine = value;
                //RaisePropertyChanged(() => CreateMachine);
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return SinRpNoInf.UpdateDate; }
            set
            {
                if (SinRpNoInf.UpdateDate == value) return;
                SinRpNoInf.UpdateDate = value;
                //RaisePropertyChanged(() => UpdateDate);
            }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return SinRpNoInf.UpdateId; }
            set
            {
                if (SinRpNoInf.UpdateId == value) return;
                SinRpNoInf.UpdateId = value;
                //RaisePropertyChanged(() => UpdateId);
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return SinRpNoInf.UpdateMachine ?? string.Empty; }
            set
            {
                if (SinRpNoInf.UpdateMachine == value) return;
                SinRpNoInf.UpdateMachine = value;
                //RaisePropertyChanged(() => UpdateMachine);
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

        public long KeyNo
        {
            get { return _keyNo; }
            set { _keyNo = value; }
        }
    }

}
