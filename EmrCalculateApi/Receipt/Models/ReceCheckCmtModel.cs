using Entity.Tenant;
using EmrCalculateApi.Ika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
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
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return ReceStatus.PtId; }
        }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        public int SeikyuYm
        {
            get { return ReceStatus.SeikyuYm; }
        }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return ReceStatus.HokenId; }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return ReceStatus.SinYm; }
        }

        /// <summary>
        /// 付箋区分
        /// 
        /// </summary>
        public int FusenKbn
        {
            get { return ReceStatus.FusenKbn; }
        }

        /// <summary>
        /// 紙レセフラグ
        /// 1:紙レセプト
        /// </summary>
        public int IsPaperRece
        {
            get { return ReceStatus.IsPaperRece; }
        }

        /// <summary>
        /// 出力フラグ
        /// 1:出力済み
        /// </summary>
        public int Output
        {
            get { return ReceStatus.Output; }
        }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return ReceStatus.IsDeleted; }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return ReceStatus.CreateDate; }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return ReceStatus.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return ReceStatus.CreateMachine ?? string.Empty; }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return ReceStatus.UpdateDate; }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return ReceStatus.UpdateId; }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return ReceStatus.UpdateMachine ?? string.Empty; }
        }

    }

}
