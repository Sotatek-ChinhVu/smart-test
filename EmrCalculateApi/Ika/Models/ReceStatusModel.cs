using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class ReceStatusModel 
    {
        public ReceStatus ReceStatus { get; } = null;

        public ReceStatusModel(ReceStatus receStatus)
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
                //RaisePropertyChanged(() => HpId);
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
                //RaisePropertyChanged(() => PtId);
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
                //RaisePropertyChanged(() => SeikyuYm);
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
                //RaisePropertyChanged(() => HokenId);
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
                //RaisePropertyChanged(() => SinYm);
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
                //RaisePropertyChanged(() => FusenKbn);
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
                //RaisePropertyChanged(() => IsPaperRece);
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
                //RaisePropertyChanged(() => Output);
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
                //RaisePropertyChanged(() => StatusKbn);
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
                //RaisePropertyChanged(() => IsDeleted);
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
                //RaisePropertyChanged(() => CreateDate);
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
                //RaisePropertyChanged(() => CreateId);
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return ReceStatus.CreateMachine; }
            set
            {
                if (ReceStatus.CreateMachine == value) return;
                ReceStatus.CreateMachine = value;
                //RaisePropertyChanged(() => CreateMachine);
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
                //RaisePropertyChanged(() => UpdateDate);
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
                //RaisePropertyChanged(() => UpdateId);
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return ReceStatus.UpdateMachine; }
            set
            {
                if (ReceStatus.UpdateMachine == value) return;
                ReceStatus.UpdateMachine = value;
                //RaisePropertyChanged(() => UpdateMachine);
            }
        }


    }

}
