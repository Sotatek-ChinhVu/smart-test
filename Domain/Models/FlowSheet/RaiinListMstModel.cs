using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class RaiinListMstModel
    {
        public RaiinListMstModel(int grpId, string grpName, int sortNo, List<RaiinListDetail> raiinListDetailsList)
        {
            GrpId = grpId;
            GrpName = grpName;
            SortNo = sortNo;
            RaiinListDetailsList = raiinListDetailsList;
        }

        public int GrpId { get; private set; }
        public string GrpName { get; private set; }
        public int SortNo { get; private set; }
        public List<RaiinListDetail> RaiinListDetailsList { get; private set; }
    }
}
