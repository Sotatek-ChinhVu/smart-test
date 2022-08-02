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
        public int GrpId { get; private set; }
        public string GrpName { get; private set; } = string.Empty;
        public int SortNo { get; private set; }
        public List<RaiinListDetail> RaiinListDetailsList { get; private set; }
    }
}
