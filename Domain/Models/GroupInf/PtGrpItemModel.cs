using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.GroupInf
{
    public class PtGrpItemModel
    {
        public PtGrpItemModel(int hpId, int grpId, string grpCode, long seqNo, string grpCodeName, int sortNo)
        {
            HpId = hpId;
            GrpId = grpId;
            GrpCode = grpCode;
            SeqNo = seqNo;
            GrpCodeName = grpCodeName;
            SortNo = sortNo;
        }

        public int HpId { get; private set; }
        public int GrpId { get; private set; }
        public string GrpCode { get; private set; }
        public long SeqNo { get; private set; }
        public string GrpCodeName { get; private set; }
        public int SortNo { get; private set; }
    }
}
