using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class LimitListInfModel
    {
        public LimitListInf LimitListInf { get; }

        public LimitListInfModel(LimitListInf limitListInf)
        {
            LimitListInf = limitListInf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return LimitListInf.HpId; }
            set
            {
                if (LimitListInf.HpId == value) return;
                LimitListInf.HpId = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return LimitListInf.PtId; }
            set
            {
                if (LimitListInf.PtId == value) return;
                LimitListInf.PtId = value;
            }
        }

        /// <summary>
        /// 公費ID
        /// PT_KOHI.KOHI_ID
        /// </summary>
        public int KohiId
        {
            get { return LimitListInf.KohiId; }
            set
            {
                if (LimitListInf.KohiId == value) return;
                LimitListInf.KohiId = value;
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return LimitListInf.SinDate; }
            set
            {
                if (LimitListInf.SinDate == value) return;
                LimitListInf.SinDate = value;
            }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return LimitListInf.SeqNo; }
            set
            {
                if (LimitListInf.SeqNo == value) return;
                LimitListInf.SeqNo = value;
            }
        }

        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        public int HokenPid
        {
            get { return LimitListInf.HokenPid; }
            set
            {
                if (LimitListInf.HokenPid == value) return;
                LimitListInf.HokenPid = value;
            }
        }

        /// <summary>
        /// 計算順番
        ///     自院:診察日 + 診察開始時間 + 来院番号 + 公費優先順位(都道府県番号+優先順位+法別番号) + 保険PID + 0
        /// </summary>
        public string SortKey
        {
            get { return LimitListInf.SortKey; }
            set
            {
                if (LimitListInf.SortKey == value) return;
                LimitListInf.SortKey = value;
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return LimitListInf.RaiinNo; }
            set
            {
                if (LimitListInf.RaiinNo == value) return;
                LimitListInf.RaiinNo = value;
            }
        }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        public int FutanGaku
        {
            get { return LimitListInf.FutanGaku; }
            set
            {
                if (LimitListInf.FutanGaku == value) return;
                LimitListInf.FutanGaku = value;
            }
        }

        /// <summary>
        /// 医療費総額
        /// 
        /// </summary>
        public int TotalGaku
        {
            get { return LimitListInf.TotalGaku; }
            set
            {
                if (LimitListInf.TotalGaku == value) return;
                LimitListInf.TotalGaku = value;
            }
        }

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        public string Biko
        {
            get { return LimitListInf.Biko ?? string.Empty; }
            set
            {
                if (LimitListInf.Biko == value) return;
                LimitListInf.Biko = value;
            }
        }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        public int IsDeleted
        {
            get { return LimitListInf.IsDeleted; }
            set
            {
                if (LimitListInf.IsDeleted == value) return;
                LimitListInf.IsDeleted = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return LimitListInf.CreateDate; }
            set
            {
                if (LimitListInf.CreateDate == value) return;
                LimitListInf.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return LimitListInf.CreateId; }
            set
            {
                if (LimitListInf.CreateId == value) return;
                LimitListInf.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return LimitListInf.CreateMachine ?? string.Empty; }
            set
            {
                if (LimitListInf.CreateMachine == value) return;
                LimitListInf.CreateMachine = value;
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return LimitListInf.UpdateDate; }
            set
            {
                if (LimitListInf.UpdateDate == value) return;
                LimitListInf.UpdateDate = value;
            }
        }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return LimitListInf.UpdateId; }
            set
            {
                if (LimitListInf.UpdateId == value) return;
                LimitListInf.UpdateId = value;
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return LimitListInf.UpdateMachine ?? string.Empty; }
            set
            {
                if (LimitListInf.UpdateMachine == value) return;
                LimitListInf.UpdateMachine = value;
            }
        }
    }
}
