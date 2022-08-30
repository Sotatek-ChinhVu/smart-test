using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class LimitCntListInfModel
    {
        public LimitCntListInf LimitCntListInf { get; }

        public LimitCntListInfModel(LimitCntListInf limitCntListInf)
        {
            LimitCntListInf = limitCntListInf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return LimitCntListInf.HpId; }
            set
            {
                if (LimitCntListInf.HpId == value) return;
                LimitCntListInf.HpId = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return LimitCntListInf.PtId; }
            set
            {
                if (LimitCntListInf.PtId == value) return;
                LimitCntListInf.PtId = value;
            }
        }

        /// <summary>
        /// 公費ID
        /// PT_KOHI.KOHI_ID
        /// </summary>
        public int KohiId
        {
            get { return LimitCntListInf.KohiId; }
            set
            {
                if (LimitCntListInf.KohiId == value) return;
                LimitCntListInf.KohiId = value;
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return LimitCntListInf.SinDate; }
            set
            {
                if (LimitCntListInf.SinDate == value) return;
                LimitCntListInf.SinDate = value;
            }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return LimitCntListInf.SeqNo; }
            set
            {
                if (LimitCntListInf.SeqNo == value) return;
                LimitCntListInf.SeqNo = value;
            }
        }

        /// <summary>
        /// 保険組合せID
        /// 0:他院 1以上:自院
        /// </summary>
        public int HokenPid
        {
            get { return LimitCntListInf.HokenPid; }
            set
            {
                if (LimitCntListInf.HokenPid == value) return;
                LimitCntListInf.HokenPid = value;
            }
        }

        /// <summary>
        /// 計算順番
        ///     自院:診察日 + 診察開始時間 + 来院番号 + 公費優先順位(都道府県番号+優先順位+法別番号) + 保険PID + 0
        /// </summary>
        public string SortKey
        {
            get { return LimitCntListInf.SortKey; }
            set
            {
                if (LimitCntListInf.SortKey == value) return;
                LimitCntListInf.SortKey = value;
            }
        }

        /// <summary>
        /// 親来院番号
        /// 0:他院 1以上:自院
        /// </summary>
        public long OyaRaiinNo
        {
            get { return LimitCntListInf.OyaRaiinNo; }
            set
            {
                if (LimitCntListInf.OyaRaiinNo == value) return;
                LimitCntListInf.OyaRaiinNo = value;
            }
        }

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        public string Biko
        {
            get { return LimitCntListInf.Biko; }
            set
            {
                if (LimitCntListInf.Biko == value) return;
                LimitCntListInf.Biko = value;
            }
        }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        public int IsDeleted
        {
            get { return LimitCntListInf.IsDeleted; }
            set
            {
                if (LimitCntListInf.IsDeleted == value) return;
                LimitCntListInf.IsDeleted = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return LimitCntListInf.CreateDate; }
            set
            {
                if (LimitCntListInf.CreateDate == value) return;
                LimitCntListInf.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return LimitCntListInf.CreateId; }
            set
            {
                if (LimitCntListInf.CreateId == value) return;
                LimitCntListInf.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return LimitCntListInf.CreateMachine ?? string.Empty; }
            set
            {
                if (LimitCntListInf.CreateMachine == value) return;
                LimitCntListInf.CreateMachine = value;
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return LimitCntListInf.UpdateDate; }
            set
            {
                if (LimitCntListInf.UpdateDate == value) return;
                LimitCntListInf.UpdateDate = value;
            }
        }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return LimitCntListInf.UpdateId; }
            set
            {
                if (LimitCntListInf.UpdateId == value) return;
                LimitCntListInf.UpdateId = value;
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return LimitCntListInf.UpdateMachine ?? string.Empty; }
            set
            {
                if (LimitCntListInf.UpdateMachine == value) return;
                LimitCntListInf.UpdateMachine = value;
            }
        }
    }
}
