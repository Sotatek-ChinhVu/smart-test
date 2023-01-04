using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class AutoSanteiMstModel 
    {
        public AutoSanteiMst AutoSanteiMst { get; } = null;

        public AutoSanteiMstModel(AutoSanteiMst autoSanteiMst)
        {
            AutoSanteiMst = autoSanteiMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return AutoSanteiMst?.HpId ?? 0; }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return AutoSanteiMst?.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 連番
        /// ○
        /// </summary>
        public int SeqNo
        {
            get { return AutoSanteiMst?.SeqNo ?? 0; }
        }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return AutoSanteiMst?.StartDate ?? 0; }
        }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return AutoSanteiMst?.EndDate ?? 0; }
        }

        ///// <summary>
        ///// 作成日時
        ///// 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return AutoSanteiMst.CreateDate; }
        //    set
        //    {
        //        if (AutoSanteiMst.CreateDate == value) return;
        //        AutoSanteiMst.CreateDate = value;
        //        //RaisePropertyChanged(() => CreateDate);
        //    }
        //}

        ///// <summary>
        ///// 作成者ID
        ///// 
        ///// </summary>
        //public int CreateId
        //{
        //    get { return AutoSanteiMst.CreateId; }
        //    set
        //    {
        //        if (AutoSanteiMst.CreateId == value) return;
        //        AutoSanteiMst.CreateId = value;
        //        //RaisePropertyChanged(() => CreateId);
        //    }
        //}

        ///// <summary>
        ///// 作成端末
        ///// 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return AutoSanteiMst.CreateMachine; }
        //    set
        //    {
        //        if (AutoSanteiMst.CreateMachine == value) return;
        //        AutoSanteiMst.CreateMachine = value;
        //        //RaisePropertyChanged(() => CreateMachine);
        //    }
        //}

        ///// <summary>
        ///// 更新日時
        ///// 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return AutoSanteiMst.UpdateDate; }
        //    set
        //    {
        //        if (AutoSanteiMst.UpdateDate == value) return;
        //        AutoSanteiMst.UpdateDate = value;
        //        //RaisePropertyChanged(() => UpdateDate);
        //    }
        //}

        ///// <summary>
        ///// 更新者ID
        ///// 
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return AutoSanteiMst.UpdateId; }
        //    set
        //    {
        //        if (AutoSanteiMst.UpdateId == value) return;
        //        AutoSanteiMst.UpdateId = value;
        //        //RaisePropertyChanged(() => UpdateId);
        //    }
        //}

        ///// <summary>
        ///// 更新端末
        ///// 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return AutoSanteiMst.UpdateMachine; }
        //    set
        //    {
        //        if (AutoSanteiMst.UpdateMachine == value) return;
        //        AutoSanteiMst.UpdateMachine = value;
        //        //RaisePropertyChanged(() => UpdateMachine);
        //    }
        //}


    }

}
