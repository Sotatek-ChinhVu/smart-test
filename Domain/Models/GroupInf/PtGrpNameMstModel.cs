using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.GroupInf
{
    public class PtGrpNameMstModel
    {
        public PtGrpNameMstModel(int hpId, int grpId, int sortNo, string? grpName)
        {
            HpId = hpId;
            GrpId = grpId;
            SortNo = sortNo;
            GrpName = grpName;
        }

        public int HpId { get; private set; }
        public int GrpId { get; private set; }
        public int SortNo { get; private set; }
        public string? GrpName { get; private set; }
    }
}
