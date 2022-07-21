using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    // Show history list of visiting date
    public class FlowSheetModel
    {
        // RaiinListInf && RaiinInf
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int GroupId { get; private set; }
        public string? Result { get; private set; } // Shoken?
        public long RaiinNo { get; private set; } // link to sindate?
        public int SyosaisinKbn { get; private set; }
        public int IsDeleted { get; private set; }
        // Raiin List Detail (for dynamic column??)
        public int KbnCd { get; private set; }
        public string? KbnName { get; private set; }
        // RaiinListMst
        public string? DynamicColName { get; private set; }
    }
}
