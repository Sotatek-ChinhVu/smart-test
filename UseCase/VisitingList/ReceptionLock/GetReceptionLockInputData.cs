using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.VisitingList.ReceptionLock
{
    public class GetReceptionLockInputData : IInputData<GetReceptionLockOutputData>
    {
        public GetReceptionLockInputData(long sinDate, long ptId, long raiinNo, string functionCd)
        {
            SinDate = sinDate;
            PtId = ptId;
            RaiinNo = raiinNo;
            FunctionCd = functionCd;
        }

        public long SinDate { get; set; }
        public long PtId { get; set; }
        public long RaiinNo { get; private set; }
        public string FunctionCd { get; set; } = string.Empty;
    }
}
