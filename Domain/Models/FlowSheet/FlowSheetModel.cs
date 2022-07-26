using Domain.Models.RaiinListMst;
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
        // KarteInf && RaiinInf && RaiinListTag & RaiinListCmt
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int RaiinListTag { get; private set; }
        public string Result { get; private set; } // First line of karte
        public long RaiinNo { get; private set; } // link to sindate?
        public int SyosaisinKbn { get; private set; }
        public string? RaiinListComment { get; private set; }
        public int IsDeleted { get; private set; }
        // Raiin List Detail && RaiinListInf (for dynamic column)
        public int KbnCd { get; private set; }
        public int GrpId { get; private set; } // dynamic column result
        public FlowSheetModel   (int hpId, long ptId, int sinDate, int raiinListTag, string firstLineOfKarte, 
                                long raiinNo, int syosaisinKbn, int isDeleted, int kbnCd)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinListTag = raiinListTag;
            Result = firstLineOfKarte ?? string.Empty;
            RaiinNo = raiinNo;
            SyosaisinKbn = syosaisinKbn;
            IsDeleted = isDeleted;
            KbnCd = kbnCd;
        }
    }
}
