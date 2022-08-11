using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientRaiinKubun.Get
{
    public class GetPatientRaiinKubunInputData : IInputData<GetPatientRaiinKubunOutputData>
    {
        public GetPatientRaiinKubunInputData(int hpId, long ptId, int raiinNo, int sinDate)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int RaiinNo { get; private set; }

        public int SinDate { get; private set; }

        
    }
}
