using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class SinKouiCountModel
    {
        public SinKouiCount SinKouiCount { get; } = null;

        /// <summary>
        /// 更新情報
        ///     0: 変更なし
        ///     1: 追加
        ///     2: 削除
        /// </summary>
        int _updateState = 0;
        long _keyNo = 0;

        public SinKouiCountModel(SinKouiCount sinKouiCount)
        {
            SinKouiCount = sinKouiCount;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SinKouiCount.HpId; }
            set
            {
                if (SinKouiCount.HpId == value) return;
                SinKouiCount.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return SinKouiCount.PtId; }
            set
            {
                if (SinKouiCount.PtId == value) return;
                SinKouiCount.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return SinKouiCount.SinYm; }
            set
            {
                if (SinKouiCount.SinYm == value) return;
                SinKouiCount.SinYm = value;
                //RaisePropertyChanged(() => SinYm);
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDay
        {
            get { return SinKouiCount.SinDay; }
            set
            {
                if (SinKouiCount.SinDay == value) return;
                SinKouiCount.SinDay = value;
                //RaisePropertyChanged(() => SinDay);
            }
        }
        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinDate
        {
            get { return SinKouiCount.SinDate; }
            set
            {
                if (SinKouiCount.SinDate == value) return;
                SinKouiCount.SinDate = value;
                //RaisePropertyChanged(() => SinDate);
            }
        }
        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return SinKouiCount.RaiinNo; }
            set
            {
                if (SinKouiCount.RaiinNo == value) return;
                SinKouiCount.RaiinNo = value;
                //RaisePropertyChanged(() => RaiinNo);
            }
        }

        /// <summary>
        /// 剤番号
        /// SEQUENCE SIN_KOUI.RP_NO
        /// </summary>
        public int RpNo
        {
            get { return SinKouiCount.RpNo; }
            set
            {
                if (SinKouiCount.RpNo == value) return;
                SinKouiCount.RpNo = value;
                //RaisePropertyChanged(() => RpNo);
            }
        }

        /// <summary>
        /// 連番
        /// SIN_KOUI.SEQ_NO
        /// </summary>
        public int SeqNo
        {
            get { return SinKouiCount.SeqNo; }
            set
            {
                if (SinKouiCount.SeqNo == value) return;
                SinKouiCount.SeqNo = value;
                //RaisePropertyChanged(() => SeqNo);
            }
        }

        /// <summary>
        /// 回数
        /// 来院ごとの回数
        /// </summary>
        public int Count
        {
            get { return SinKouiCount.Count; }
            set
            {
                if (SinKouiCount.Count == value) return;
                SinKouiCount.Count = value;
                //RaisePropertyChanged(() => Count);
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return SinKouiCount.CreateDate; }
            set
            {
                if (SinKouiCount.CreateDate == value) return;
                SinKouiCount.CreateDate = value;
                //RaisePropertyChanged(() => CreateDate);
            }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return SinKouiCount.CreateId; }
            set
            {
                if (SinKouiCount.CreateId == value) return;
                SinKouiCount.CreateId = value;
                //RaisePropertyChanged(() => CreateId);
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return SinKouiCount.CreateMachine ?? string.Empty; }
            set
            {
                if (SinKouiCount.CreateMachine == value) return;
                SinKouiCount.CreateMachine = value;
                //RaisePropertyChanged(() => CreateMachine);
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return SinKouiCount.UpdateDate; }
            set
            {
                if (SinKouiCount.UpdateDate == value) return;
                SinKouiCount.UpdateDate = value;
                //RaisePropertyChanged(() => UpdateDate);
            }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return SinKouiCount.UpdateId; }
            set
            {
                if (SinKouiCount.UpdateId == value) return;
                SinKouiCount.UpdateId = value;
                //RaisePropertyChanged(() => UpdateId);
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return SinKouiCount.UpdateMachine ?? string.Empty; }
            set
            {
                if (SinKouiCount.UpdateMachine == value) return;
                SinKouiCount.UpdateMachine = value;
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

        /// <summary>
        /// キー番号
        /// </summary>
        public long KeyNo
        {
            get { return _keyNo; }
            set { _keyNo = value; }
        }
    }

}
