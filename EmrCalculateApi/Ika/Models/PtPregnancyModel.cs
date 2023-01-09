using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class PtPregnancyModel
    {
        public PtPregnancy PtPregnancy { get; } = null;

        public PtPregnancyModel(PtPregnancy ptPregnancy)
        {
            PtPregnancy = ptPregnancy;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return PtPregnancy.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return PtPregnancy.PtId; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return PtPregnancy.SeqNo; }
        }

        /// <summary>
        /// 妊娠開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return PtPregnancy.StartDate; }
        }

        /// <summary>
        /// 妊娠終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return PtPregnancy.EndDate; }
        }

        /// <summary>
        /// 月経日
        /// YYYYMMDD(最終月経日)
        /// </summary>
        public int PeriodDate
        {
            get { return PtPregnancy.PeriodDate; }
        }

        /// <summary>
        /// 月経予定日
        /// 
        /// </summary>
        public int PeriodDueDate
        {
            get { return PtPregnancy.PeriodDueDate; }
        }

        /// <summary>
        /// 排卵日
        /// YYYYMMDD(最終排卵日)
        /// </summary>
        public int OvulationDate
        {
            get { return PtPregnancy.OvulationDate; }
        }

        /// <summary>
        /// 排卵予定日
        /// 
        /// </summary>
        public int OvulationDueDate
        {
            get { return PtPregnancy.OvulationDueDate; }
        }

        /// <summary>
        /// 削除フラグ
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return PtPregnancy.IsDeleted; }
        }

        ///// <summary>
        ///// 作成日時
        ///// 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return PtPregnancy.CreateDate; }
        //    set
        //    {
        //        if (PtPregnancy.CreateDate == value) return;
        //        PtPregnancy.CreateDate = value;
        //        //RaisePropertyChanged(() => CreateDate);
        //    }
        //}

        ///// <summary>
        ///// 作成者
        ///// 
        ///// </summary>
        //public int CreateId
        //{
        //    get { return PtPregnancy.CreateId; }
        //    set
        //    {
        //        if (PtPregnancy.CreateId == value) return;
        //        PtPregnancy.CreateId = value;
        //        //RaisePropertyChanged(() => CreateId);
        //    }
        //}

        ///// <summary>
        ///// 作成端末
        ///// 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return PtPregnancy.CreateMachine; }
        //    set
        //    {
        //        if (PtPregnancy.CreateMachine == value) return;
        //        PtPregnancy.CreateMachine = value;
        //        //RaisePropertyChanged(() => CreateMachine);
        //    }
        //}

        ///// <summary>
        ///// 更新日時
        ///// 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return PtPregnancy.UpdateDate; }
        //    set
        //    {
        //        if (PtPregnancy.UpdateDate == value) return;
        //        PtPregnancy.UpdateDate = value;
        //        //RaisePropertyChanged(() => UpdateDate);
        //    }
        //}

        ///// <summary>
        ///// 更新者
        ///// 
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return PtPregnancy.UpdateId; }
        //    set
        //    {
        //        if (PtPregnancy.UpdateId == value) return;
        //        PtPregnancy.UpdateId = value;
        //        //RaisePropertyChanged(() => UpdateId);
        //    }
        //}

        ///// <summary>
        ///// 更新端末
        ///// 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return PtPregnancy.UpdateMachine; }
        //    set
        //    {
        //        if (PtPregnancy.UpdateMachine == value) return;
        //        PtPregnancy.UpdateMachine = value;
        //        //RaisePropertyChanged(() => UpdateMachine);
        //    }
        //}


    }

}
