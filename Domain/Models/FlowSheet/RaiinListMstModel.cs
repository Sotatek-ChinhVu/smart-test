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
        public int GrpId { get; set; }
        public string GrpName { get; set; } = string.Empty;
        public int SortNo { get; set; }
        public List<RaiinListDetail>? RaiinListDetailsList { get; set; }
    }
}
