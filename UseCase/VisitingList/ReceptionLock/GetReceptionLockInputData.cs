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
        public GetReceptionLockInputData(int hpId, int sinDate, long ptId, long raiinNo, string functionCd)
        {
            HpId = hpId;
            SinDate = sinDate;
            PtId = ptId;
            RaiinNo = raiinNo;
            FunctionCd = functionCd;
        }

        public int HpId { get; private set; }
        public int SinDate { get; private set; }
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public string FunctionCd { get; private set; }
    }
}
