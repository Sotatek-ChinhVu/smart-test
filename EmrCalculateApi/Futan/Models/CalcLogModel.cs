using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class CalcLogModel
    {
        public CalcLog CalcLog { get; }

        public CalcLogModel(CalcLog calcLog)
        {
            CalcLog = calcLog;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return CalcLog.HpId; }
            set
            {
                if (CalcLog.HpId == value) return;
                CalcLog.HpId = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return CalcLog.PtId; }
            set
            {
                if (CalcLog.PtId == value) return;
                CalcLog.PtId = value;
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return CalcLog.SinDate; }
            set
            {
                if (CalcLog.SinDate == value) return;
                CalcLog.SinDate = value;
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return CalcLog.RaiinNo; }
            set
            {
                if (CalcLog.RaiinNo == value) return;
                CalcLog.RaiinNo = value;
            }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return CalcLog.SeqNo; }
            set
            {
                if (CalcLog.SeqNo == value) return;
                CalcLog.SeqNo = value;
            }
        }

        /// <summary>
        /// ログ種別
        /// 0:通常 1:注意 2:警告
        /// </summary>
        public int LogSbt
        {
            get { return CalcLog.LogSbt; }
            set
            {
                if (CalcLog.LogSbt == value) return;
                CalcLog.LogSbt = value;
            }
        }

        /// <summary>
        /// ログ
        /// 
        /// </summary>
        public string Text
        {
            get { return CalcLog.Text; }
            set
            {
                if (CalcLog.Text == value) return;
                CalcLog.Text = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return CalcLog.CreateDate; }
            set
            {
                if (CalcLog.CreateDate == value) return;
                CalcLog.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return CalcLog.CreateId; }
            set
            {
                if (CalcLog.CreateId == value) return;
                CalcLog.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return CalcLog.CreateMachine ?? string.Empty; }
            set
            {
                if (CalcLog.CreateMachine == value) return;
                CalcLog.CreateMachine = value;
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return CalcLog.UpdateDate; }
            set
            {
                if (CalcLog.UpdateDate == value) return;
                CalcLog.UpdateDate = value;
            }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return CalcLog.UpdateId; }
            set
            {
                if (CalcLog.UpdateId == value) return;
                CalcLog.UpdateId = value;
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return CalcLog.UpdateMachine ?? string.Empty; }
            set
            {
                if (CalcLog.UpdateMachine == value) return;
                CalcLog.UpdateMachine = value;
            }
        }
    }
}
