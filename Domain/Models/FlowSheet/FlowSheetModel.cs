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
        // RaiinListInf && RaiinInf
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int GroupId { get; private set; }
        public string Result { get; private set; } // Shoken?
        public long RaiinNo { get; private set; } // link to sindate?
        public int SyosaisinKbn { get; private set; }
        public int IsDeleted { get; private set; }
        // Raiin List Detail (for dynamic column??)
        public int KbnCd { get; private set; }
        public string? KbnName { get; private set; }
        // RaiinListMst => col name, raiin list inf .GrpId => result
        public Dictionary<RaiinListMstModel, int> DynamicColNameAndResult { get; private set; }
        public FlowSheetModel   (int hpId, long ptId, int sinDate, int groupId, string result, 
                                long raiinNo, int syosaisinKbn, int isDeleted, int kbnCd, string? kbnName, 
                                Dictionary<RaiinListMstModel, int> dynamicColNameAndResult)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            GroupId = groupId;
            Result = result != null ? result : "" ;
            RaiinNo = raiinNo;
            SyosaisinKbn = syosaisinKbn;
            IsDeleted = isDeleted;
            KbnCd = kbnCd;
            KbnName = kbnName;
            DynamicColNameAndResult = dynamicColNameAndResult;
        }
    }
}
