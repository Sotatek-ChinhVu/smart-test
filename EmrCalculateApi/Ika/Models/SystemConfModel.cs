using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class SystemConfModel
    {
        public SystemConf SystemConf { get; } = null;

        public SystemConfModel(SystemConf systemConf)
        {
            SystemConf = systemConf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return SystemConf.HpId; }
        }

        /// <summary>
        /// 分類コード
        /// </summary>
        public int GrpCd
        {
            get { return SystemConf.GrpCd; }
        }

        /// <summary>
        /// 分類枝番
        /// </summary>
        public int GrpEdaNo
        {
            get { return SystemConf.GrpEdaNo; }
        }

        /// <summary>
        /// 設定値
        /// </summary>
        public double Val
        {
            get { return SystemConf.Val; }
        }

        /// <summary>
        /// パラメーター
        /// </summary>
        public string Param
        {
            get { return SystemConf.Param ?? string.Empty; }
        }

        /// <summary>
        /// 備考
        /// </summary>
        public string Biko
        {
            get { return SystemConf.Biko ?? string.Empty; }
        }

        ///// <summary>
        ///// 作成日時 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return SystemConf.CreateDate; }
        //    set
        //    {
        //        if (SystemConf.CreateDate == value) return;
        //        SystemConf.CreateDate = value;
        //        //RaisePropertyChanged(() => CreateDate);
        //    }
        //}

        ///// <summary>
        ///// 作成者  
        ///// </summary>
        //public int CreateId
        //{
        //    get { return SystemConf.CreateId; }
        //    set
        //    {
        //        if (SystemConf.CreateId == value) return;
        //        SystemConf.CreateId = value;
        //        //RaisePropertyChanged(() => CreateId);
        //    }
        //}

        ///// <summary>
        ///// 作成端末   
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return SystemConf.CreateMachine; }
        //    set
        //    {
        //        if (SystemConf.CreateMachine == value) return;
        //        SystemConf.CreateMachine = value;
        //        //RaisePropertyChanged(() => CreateMachine);
        //    }
        //}

        ///// <summary>
        ///// 更新日時   
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return SystemConf.UpdateDate; }
        //    set
        //    {
        //        if (SystemConf.UpdateDate == value) return;
        //        SystemConf.UpdateDate = value;
        //        //RaisePropertyChanged(() => UpdateDate);
        //    }
        //}

        ///// <summary>
        ///// 更新者   
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return SystemConf.UpdateId; }
        //    set
        //    {
        //        if (SystemConf.UpdateId == value) return;
        //        SystemConf.UpdateId = value;
        //        //RaisePropertyChanged(() => UpdateId);
        //    }
        //}

        ///// <summary>
        ///// 更新端末   
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return SystemConf.UpdateMachine; }
        //    set
        //    {
        //        if (SystemConf.UpdateMachine == value) return;
        //        SystemConf.UpdateMachine = value;
        //        //RaisePropertyChanged(() => UpdateMachine);
        //    }
        //}

    }

}
