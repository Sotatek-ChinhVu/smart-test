using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class HpInfModel
    {
        public HpInf HpInf { get; } = null;

        public HpInfModel(HpInf hpInf)
        {
            HpInf = hpInf;
        }

        /// <summary>
        /// 医療機関情報
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return HpInf.HpId; }
        }

        /// <summary>
        /// 開始日
        ///     yyyymmdd
        /// </summary>
        public int StartDate
        {
            get { return HpInf.StartDate; }
        }

        /// <summary>
        /// 医療機関コード   
        /// </summary>
        public string HpCd
        {
            get { return HpInf.HpCd ?? string.Empty; }
        }

        /// <summary>
        /// 労災医療機関コード   
        /// </summary>
        public string RousaiHpCd
        {
            get { return HpInf.RousaiHpCd ?? string.Empty; }
        }

        /// <summary>
        /// 医療機関名   
        /// </summary>
        public string HpName
        {
            get { return HpInf.HpName ?? string.Empty; }
        }

        /// <summary>
        /// レセ医療機関名   
        /// </summary>
        public string ReceHpName
        {
            get { return HpInf.ReceHpName ?? string.Empty; }
        }

        /// <summary>
        /// 開設者氏名   
        /// </summary>
        public string KaisetuName
        {
            get { return HpInf.KaisetuName ?? string.Empty; }
        }

        /// <summary>
        /// 郵便番号   
        /// </summary>
        public string PostCd
        {
            get { return HpInf.PostCd ?? string.Empty; }
        }

        /// <summary>
        /// 都道府県番号   
        /// </summary>
        public int PrefNo
        {
            get { return HpInf.PrefNo; }
        }

        /// <summary>
        /// 医療機関所在地１   
        /// </summary>
        public string Address1
        {
            get { return HpInf.Address1 ?? string.Empty; }
        }

        /// <summary>
        /// 医療機関所在地２   
        /// </summary>
        public string Address2
        {
            get { return HpInf.Address2 ?? string.Empty; }
        }

        /// <summary>
        /// 電話番号   
        /// </summary>
        public string Tel
        {
            get { return HpInf.Tel ?? string.Empty; }
        }

    }

}
