using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class HolidayMstModel 
    {
        public HolidayMst HolidayMst { get; } = null;

        public HolidayMstModel(HolidayMst holidayMst)
        {
            HolidayMst = holidayMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return HolidayMst.HpId; }
        }

        /// <summary>
        /// 診療日
        /// </summary>
        public int SinDate
        {
            get { return HolidayMst.SinDate; }
        }

        /// <summary>
        /// 連番
        ///     診療日内の連番
        /// </summary>
        public int SeqNo
        {
            get { return HolidayMst.SeqNo; }
        }

        /// <summary>
        /// 休日区分
        /// </summary>
        public int HolidayKbn
        {
            get { return HolidayMst.HolidayKbn; }
        }

        /// <summary>
        /// 休診区分
        /// </summary>
        public int KyusinKbn
        {
            get { return HolidayMst.KyusinKbn; }
        }

        /// <summary>
        /// 休日名
        /// </summary>
        public string HolidayName
        {
            get { return HolidayMst.HolidayName ?? string.Empty; }
        }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return HolidayMst.IsDeleted; }
        }

        ///// <summary>
        ///// 作成日時 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return HolidayMst.CreateDate; }
        //    set
        //    {
        //        if (HolidayMst.CreateDate == value) return;
        //        HolidayMst.CreateDate = value;
        //        //RaisePropertyChanged(() => CreateDate);
        //    }
        //}

        ///// <summary>
        ///// 作成者
        ///// </summary>
        //public int CreateId
        //{
        //    get { return HolidayMst.CreateId; }
        //    set
        //    {
        //        if (HolidayMst.CreateId == value) return;
        //        HolidayMst.CreateId = value;
        //        //RaisePropertyChanged(() => CreateId);
        //    }
        //}

        ///// <summary>
        ///// 作成端末 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return HolidayMst.CreateMachine; }
        //    set
        //    {
        //        if (HolidayMst.CreateMachine == value) return;
        //        HolidayMst.CreateMachine = value;
        //        //RaisePropertyChanged(() => CreateMachine);
        //    }
        //}

        ///// <summary>
        ///// 更新日時 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return HolidayMst.UpdateDate; }
        //    set
        //    {
        //        if (HolidayMst.UpdateDate == value) return;
        //        HolidayMst.UpdateDate = value;
        //        //RaisePropertyChanged(() => UpdateDate);
        //    }
        //}

        ///// <summary>
        ///// 更新者
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return HolidayMst.UpdateId; }
        //    set
        //    {
        //        if (HolidayMst.UpdateId == value) return;
        //        HolidayMst.UpdateId = value;
        //        //RaisePropertyChanged(() => UpdateId);
        //    }
        //}

        ///// <summary>
        ///// 更新端末 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return HolidayMst.UpdateMachine; }
        //    set
        //    {
        //        if (HolidayMst.UpdateMachine == value) return;
        //        HolidayMst.UpdateMachine = value;
        //        //RaisePropertyChanged(() => UpdateMachine);
        //    }
        //}
    }

}
