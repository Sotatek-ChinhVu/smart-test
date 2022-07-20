using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Async.Core;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinCmtInf
{
    public class GetRaiinCmtInfInputData : IInputData<GetRaiinCmtInfOutputData>
    {
        public GetRaiinCmtInfInputData(long ptId, int sinDate, int raiinNo)
        {
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
        }

        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int RaiinNo { get; set; }
    }
}
