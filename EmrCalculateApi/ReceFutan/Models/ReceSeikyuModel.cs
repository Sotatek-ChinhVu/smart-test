using Entity.Tenant;

namespace EmrCalculateApi.ReceFutan.Models
{
    public class ReceSeikyuModel
    {
        public ReceSeikyu ReceSeikyu { get; }

        public ReceSeikyuModel(ReceSeikyu receSeikyu)
        {
            ReceSeikyu = receSeikyu;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return ReceSeikyu.HpId; }
            set
            {
                if (ReceSeikyu.HpId == value) return;
                ReceSeikyu.HpId = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return ReceSeikyu.PtId; }
            set
            {
                if (ReceSeikyu.PtId == value) return;
                ReceSeikyu.PtId = value;
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return ReceSeikyu.SinYm; }
            set
            {
                if (ReceSeikyu.SinYm == value) return;
                ReceSeikyu.SinYm = value;
            }
        }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return ReceSeikyu.HokenId; }
            set
            {
                if (ReceSeikyu.HokenId == value) return;
                ReceSeikyu.HokenId = value;
            }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return ReceSeikyu.SeqNo; }
            set
            {
                if (ReceSeikyu.SeqNo == value) return;
                ReceSeikyu.SeqNo = value;
            }
        }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        public int SeikyuYm
        {
            get { return ReceSeikyu.SeikyuYm; }
            set
            {
                if (ReceSeikyu.SeikyuYm == value) return;
                ReceSeikyu.SeikyuYm = value;
            }
        }

        /// <summary>
        /// 請求区分
        /// 1:月遅れ 2:返戻 3:オンライン返戻
        /// </summary>
        public int SeikyuKbn
        {
            get { return ReceSeikyu.SeikyuKbn; }
            set
            {
                if (ReceSeikyu.SeikyuKbn == value) return;
                ReceSeikyu.SeikyuKbn = value;
            }
        }

        /// <summary>
        /// 前回請求保険ID
        /// 
        /// </summary>
        public int PreHokenId
        {
            get { return ReceSeikyu.PreHokenId; }
            set
            {
                if (ReceSeikyu.PreHokenId == value) return;
                ReceSeikyu.PreHokenId = value;
            }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt
        {
            get { return ReceSeikyu.Cmt; }
            set
            {
                if (ReceSeikyu.Cmt == value) return;
                ReceSeikyu.Cmt = value;
            }
        }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        public int IsDeleted
        {
            get { return ReceSeikyu.IsDeleted; }
            set
            {
                if (ReceSeikyu.IsDeleted == value) return;
                ReceSeikyu.IsDeleted = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return ReceSeikyu.CreateDate; }
            set
            {
                if (ReceSeikyu.CreateDate == value) return;
                ReceSeikyu.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return ReceSeikyu.CreateId; }
            set
            {
                if (ReceSeikyu.CreateId == value) return;
                ReceSeikyu.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return ReceSeikyu.CreateMachine ?? string.Empty; }
            set
            {
                if (ReceSeikyu.CreateMachine == value) return;
                ReceSeikyu.CreateMachine = value;
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return ReceSeikyu.UpdateDate; }
            set
            {
                if (ReceSeikyu.UpdateDate == value) return;
                ReceSeikyu.UpdateDate = value;
            }
        }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return ReceSeikyu.UpdateId; }
            set
            {
                if (ReceSeikyu.UpdateId == value) return;
                ReceSeikyu.UpdateId = value;
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return ReceSeikyu.UpdateMachine ?? string.Empty; }
            set
            {
                if (ReceSeikyu.UpdateMachine == value) return;
                ReceSeikyu.UpdateMachine = value;
            }
        }

    }

}
