using Entity.Tenant;

namespace Reporting.Receipt.Models
{
    public class CoReceStatusModel
    {
        public ReceStatus ReceStatus { get; }

        public CoReceStatusModel(ReceStatus receStatus)
        {
            ReceStatus = receStatus;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return ReceStatus.HpId; }
            set
            {
                if (ReceStatus.HpId == value) return;
                ReceStatus.HpId = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return ReceStatus.PtId; }
            set
            {
                if (ReceStatus.PtId == value) return;
                ReceStatus.PtId = value;
            }
        }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        public int SeikyuYm
        {
            get { return ReceStatus.SeikyuYm; }
            set
            {
                if (ReceStatus.SeikyuYm == value) return;
                ReceStatus.SeikyuYm = value;
            }
        }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return ReceStatus.HokenId; }
            set
            {
                if (ReceStatus.HokenId == value) return;
                ReceStatus.HokenId = value;
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return ReceStatus.SinYm; }
            set
            {
                if (ReceStatus.SinYm == value) return;
                ReceStatus.SinYm = value;
            }
        }

        /// <summary>
        /// 付箋区分
        /// 
        /// </summary>
        public int FusenKbn
        {
            get { return ReceStatus.FusenKbn; }
            set
            {
                if (ReceStatus.FusenKbn == value) return;
                ReceStatus.FusenKbn = value;
            }
        }

        /// <summary>
        /// 紙レセフラグ
        /// 1:紙レセプト
        /// </summary>
        public int IsPaperRece
        {
            get { return ReceStatus.IsPaperRece; }
            set
            {
                if (ReceStatus.IsPaperRece == value) return;
                ReceStatus.IsPaperRece = value;
            }
        }

        /// <summary>
        /// 出力フラグ
        /// 1:出力済み
        /// </summary>
        public int Output
        {
            get { return ReceStatus.Output; }
            set
            {
                if (ReceStatus.Output == value) return;
                ReceStatus.Output = value;
            }
        }

        /// <summary>
        /// 状態区分
        /// 0:未確認 1:システム保留 2:保留1 3:保留2 4:保留3 8:仮確認 9:確認済
        /// </summary>
        public int StatusKbn
        {
            get { return ReceStatus.StatusKbn; }
            set
            {
                if (ReceStatus.StatusKbn == value) return;
                ReceStatus.StatusKbn = value;
            }
        }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return ReceStatus.IsDeleted; }
            set
            {
                if (ReceStatus.IsDeleted == value) return;
                ReceStatus.IsDeleted = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return ReceStatus.CreateDate; }
            set
            {
                if (ReceStatus.CreateDate == value) return;
                ReceStatus.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return ReceStatus.CreateId; }
            set
            {
                if (ReceStatus.CreateId == value) return;
                ReceStatus.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return ReceStatus.CreateMachine ?? string.Empty; }
            set
            {
                if (ReceStatus.CreateMachine == value) return;
                ReceStatus.CreateMachine = value;
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return ReceStatus.UpdateDate; }
            set
            {
                if (ReceStatus.UpdateDate == value) return;
                ReceStatus.UpdateDate = value;
            }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return ReceStatus.UpdateId; }
            set
            {
                if (ReceStatus.UpdateId == value) return;
                ReceStatus.UpdateId = value;
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return ReceStatus.UpdateMachine ?? string.Empty; }
            set
            {
                if (ReceStatus.UpdateMachine == value) return;
                ReceStatus.UpdateMachine = value;
            }
        }
    }
}
