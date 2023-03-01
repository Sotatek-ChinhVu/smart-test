using Entity.Tenant;

namespace EmrCalculateApi.ReceFutan.Models
{
    public class ReceFutanKbnModel
    {
        public ReceFutanKbn ReceFutanKbn { get; }

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
            set
            {
                if (ReceFutanKbn.HpId == value) return;
                ReceFutanKbn.HpId = value;
            }
        }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        public int SeikyuYm
        {
            get { return ReceFutanKbn.SeikyuYm; }
            set
            {
                if (ReceFutanKbn.SeikyuYm == value) return;
                ReceFutanKbn.SeikyuYm = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return ReceFutanKbn.PtId; }
            set
            {
                if (ReceFutanKbn.PtId == value) return;
                ReceFutanKbn.PtId = value;
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return ReceFutanKbn.SinYm; }
            set
            {
                if (ReceFutanKbn.SinYm == value) return;
                ReceFutanKbn.SinYm = value;
            }
        }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return ReceFutanKbn.HokenId; }
            set
            {
                if (ReceFutanKbn.HokenId == value) return;
                ReceFutanKbn.HokenId = value;
            }
        }

        /// <summary>
        /// 保険組合せID
        /// 患者別に保険情報を識別するための固有の番号
        /// </summary>
        public int HokenPid
        {
            get { return ReceFutanKbn.HokenPid; }
            set
            {
                if (ReceFutanKbn.HokenPid == value) return;
                ReceFutanKbn.HokenPid = value;
            }
        }

        /// <summary>
        /// 負担区分コード
        /// 
        /// </summary>
        public string FutanKbnCd
        {
            get { return ReceFutanKbn.FutanKbnCd; }
            set
            {
                if (ReceFutanKbn.FutanKbnCd == value) return;
                ReceFutanKbn.FutanKbnCd = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return ReceFutanKbn.CreateDate; }
            set
            {
                if (ReceFutanKbn.CreateDate == value) return;
                ReceFutanKbn.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return ReceFutanKbn.CreateId; }
            set
            {
                if (ReceFutanKbn.CreateId == value) return;
                ReceFutanKbn.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return ReceFutanKbn.CreateMachine ?? string.Empty; }
            set
            {
                if (ReceFutanKbn.CreateMachine == value) return;
                ReceFutanKbn.CreateMachine = value;
            }
        }

        /// <summary>
        /// 保険有無
        /// </summary>
        public bool IsHoken;

        /// <summary>
        /// 公費１有無
        /// </summary>
        public bool IsKohi1;

        /// <summary>
        /// 公費２有無
        /// </summary>
        public bool IsKohi2;

        /// <summary>
        /// 公費３有無
        /// </summary>
        public bool IsKohi3;

        /// <summary>
        /// 公費４有無
        /// </summary>
        public bool IsKohi4;

    }

}
