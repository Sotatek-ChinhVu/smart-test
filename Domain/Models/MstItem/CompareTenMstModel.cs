using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MstItem
{
    public class CompareTenMstModel
    {
        public CompareTenMstModel(string itemCd, int hpId, int startDate, string tenName, string tenReceName, string tenOrdUnitName, string tenReceUnitName, int tenSaiketuKbn, string tenMotherName, string tenMotherReceName, string tenMotherOrdUnitName, string tenMotherReceUnitName, int tenMotherSaiketuKbn)
        {
            ItemCd = itemCd;
            HpId = hpId;
            StartDate = startDate;
            TenName = tenName;
            TenReceName = tenReceName;
            TenOrdUnitName = tenOrdUnitName;
            TenReceUnitName = tenReceUnitName;
            TenSaiketuKbn = tenSaiketuKbn;
            TenMotherName = tenMotherName;
            TenMotherReceName = tenMotherReceName;
            TenMotherOrdUnitName = tenMotherOrdUnitName;
            TenMotherReceUnitName = tenMotherReceUnitName;
            TenMotherSaiketuKbn = tenMotherSaiketuKbn;
        }

        public string ItemCd { get; private set; }
        public int HpId { get; private set; }
        public int StartDate { get; private set; }
        public string TenName { get; private set; }
        public string TenReceName { get; private set; }
        public string TenOrdUnitName { get; private set; }
        public string TenReceUnitName { get; private set; }
        public int TenSaiketuKbn { get; private set; }
        public string TenMotherName { get; private set; }
        public string TenMotherReceName { get; private set; }
        public string TenMotherOrdUnitName { get; private set; }
        public string TenMotherReceUnitName { get; private set; }
        public int TenMotherSaiketuKbn { get; private set; }
    }
}
