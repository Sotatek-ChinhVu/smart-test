using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.GroupInf
{
    public class GroupInfModel
    {
        public GroupInfModel(PtGrpNameMstModel? ptGrpNameMst, PtGrpItemModel? ptGrpItem)
        {
            PtGrpNameMst = ptGrpNameMst;
            PtGrpItem = ptGrpItem;
        }
        public PtGrpNameMstModel? PtGrpNameMst { get; private set; }
        public PtGrpItemModel? PtGrpItem { get; private set; }

        public string? GroupCode { get; set; }
        public long PtId { get; set; }
        public List<PtGrpItemModel>? ListItem { get; set; }
        public int SortNo { get; set; }

    }



}
