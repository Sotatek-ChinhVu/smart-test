using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class ReceFutanKbnModel 
    {
        public ReceFutanKbn ReceFutanKbn { get; } = null;

        public ReceFutanKbnModel(ReceFutanKbn receFutanKbn)
        {
            ReceFutanKbn = receFutanKbn;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return ReceFutanKbn.HpId; }
        }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        public int SeikyuYm
        {
            get { return ReceFutanKbn.SeikyuYm; }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return ReceFutanKbn.PtId; }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return ReceFutanKbn.SinYm; }
        }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return ReceFutanKbn.HokenId; }
        }

        /// <summary>
        /// 保険組合せID
        /// 患者別に保険情報を識別するための固有の番号
        /// </summary>
        public int HokenPid
        {
            get { return ReceFutanKbn.HokenPid; }
        }

        /// <summary>
        /// 負担区分コード
        /// 
        /// </summary>
        public string FutanKbnCd
        {
            get { return ReceFutanKbn.FutanKbnCd; }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return ReceFutanKbn.CreateDate; }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return ReceFutanKbn.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return ReceFutanKbn.CreateMachine; }
        }


    }

}
