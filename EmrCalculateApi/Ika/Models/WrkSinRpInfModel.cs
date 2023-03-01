using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class WrkSinRpInfModel
    {
        public WrkSinRpInf WrkSinRpInf { get; } = null;

        public WrkSinRpInfModel(WrkSinRpInf wrkSinRpInf)
        {
            WrkSinRpInf = wrkSinRpInf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return WrkSinRpInf.HpId; }
            set
            {
                if (WrkSinRpInf.HpId == value) return;
                WrkSinRpInf.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return WrkSinRpInf.PtId; }
            set
            {
                if (WrkSinRpInf.PtId == value) return;
                WrkSinRpInf.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return WrkSinRpInf.SinDate; }
            set
            {
                if (WrkSinRpInf.SinDate == value) return;
                WrkSinRpInf.SinDate = value;
                //RaisePropertyChanged(() => SinDate);
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return WrkSinRpInf.RaiinNo; }
            set
            {
                if (WrkSinRpInf.RaiinNo == value) return;
                WrkSinRpInf.RaiinNo = value;
                //RaisePropertyChanged(() => RaiinNo);
            }
        }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        public int HokenKbn
        {
            get { return WrkSinRpInf.HokenKbn; }
            set
            {
                if (WrkSinRpInf.HokenKbn == value) return;
                WrkSinRpInf.HokenKbn = value;
                //RaisePropertyChanged(() => HokenKbn);
            }
        }

        /// <summary>
        /// 剤番号
        /// 
        /// </summary>
        public int RpNo
        {
            get { return WrkSinRpInf.RpNo; }
            set
            {
                if (WrkSinRpInf.RpNo == value) return;
                WrkSinRpInf.RpNo = value;
                //RaisePropertyChanged(() => RpNo);
            }
        }

        /// <summary>
        /// 診療行為区分
        /// 
        /// </summary>
        public int SinKouiKbn
        {
            get { return WrkSinRpInf.SinKouiKbn; }
            set
            {
                if (WrkSinRpInf.SinKouiKbn == value) return;
                WrkSinRpInf.SinKouiKbn = value;
                //RaisePropertyChanged(() => SinKouiKbn);
            }
        }

        /// <summary>
        /// 診療識別
        /// レセプト電算に記録する診療識別
        /// </summary>
        public int SinId
        {
            get { return WrkSinRpInf.SinId; }
            set
            {
                if (WrkSinRpInf.SinId == value) return;
                WrkSinRpInf.SinId = value;
                //RaisePropertyChanged(() => SinId);
            }
        }

        /// <summary>
        /// 代表コード表用番号
        /// </summary>
        public string CdNo
        {
            get { return WrkSinRpInf.CdNo ?? ""; }
            set
            {
                if (WrkSinRpInf.CdNo == value) return;
                WrkSinRpInf.CdNo = value;
                //RaisePropertyChanged(() => CdNo);
            }
        }

        /// <summary>
        /// 算定区分
        /// 1:自費算定
        /// </summary>
        public int SanteiKbn
        {
            get { return WrkSinRpInf.SanteiKbn; }
            set
            {
                if (WrkSinRpInf.SanteiKbn == value) return;
                WrkSinRpInf.SanteiKbn = value;
                //RaisePropertyChanged(() => SanteiKbn);
            }
        }

        /// <summary>
        /// 削除フラグ
        ///     1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return WrkSinRpInf.IsDeleted; }
            set
            {
                if (WrkSinRpInf.IsDeleted == value) return;
                WrkSinRpInf.IsDeleted = value;
                //RaisePropertyChanged(() => IsDeleted);
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return WrkSinRpInf.CreateDate; }
            set
            {
                if (WrkSinRpInf.CreateDate == value) return;
                WrkSinRpInf.CreateDate = value;
                //RaisePropertyChanged(() => CreateDate);
            }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return WrkSinRpInf.CreateId; }
            set
            {
                if (WrkSinRpInf.CreateId == value) return;
                WrkSinRpInf.CreateId = value;
                //RaisePropertyChanged(() => CreateId);
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return WrkSinRpInf.CreateMachine ?? string.Empty; }
            set
            {
                if (WrkSinRpInf.CreateMachine == value) return;
                WrkSinRpInf.CreateMachine = value;
                //RaisePropertyChanged(() => CreateMachine);
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return WrkSinRpInf.UpdateDate; }
            set
            {
                if (WrkSinRpInf.UpdateDate == value) return;
                WrkSinRpInf.UpdateDate = value;
                //RaisePropertyChanged(() => UpdateDate);
            }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return WrkSinRpInf.UpdateId; }
            set
            {
                if (WrkSinRpInf.UpdateId == value) return;
                WrkSinRpInf.UpdateId = value;
                //RaisePropertyChanged(() => UpdateId);
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return WrkSinRpInf.UpdateMachine ?? string.Empty; }
            set
            {
                if (WrkSinRpInf.UpdateMachine == value) return;
                WrkSinRpInf.UpdateMachine = value;
                //RaisePropertyChanged(() => UpdateMachine);
            }
        }


    }


}
