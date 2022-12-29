using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class IpnKasanExcludeItemModel
    {
        public IpnKasanExcludeItem IpnKasanExcludeItem { get; } = null;

        public IpnKasanExcludeItemModel(IpnKasanExcludeItem ipnKasanExcludeItem)
        {
            IpnKasanExcludeItem = ipnKasanExcludeItem;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return IpnKasanExcludeItem.HpId; }
        }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return IpnKasanExcludeItem.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return IpnKasanExcludeItem.StartDate; }
        }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return IpnKasanExcludeItem.EndDate; }
        }


    }

}
