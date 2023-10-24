using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MstItem
{
    public class SaveCompareTenMstModel
    {
        public SaveCompareTenMstModel(string itemCd, int hpId, int startDate, string nameNew, int tenSaiketuKbnNew)
        {
            ItemCd = itemCd;
            HpId = hpId;
            StartDate = startDate;
            NameNew = nameNew;
            TenSaiketuKbnNew = tenSaiketuKbnNew;
        }

        public string ItemCd { get; private set; }
        public int HpId { get; private set; }
        public int StartDate { get; private set; }
        public string NameNew { get; private set; }
        public int TenSaiketuKbnNew { get; private set; }
    }
}
