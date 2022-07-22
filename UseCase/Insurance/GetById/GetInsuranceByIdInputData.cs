using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetById
{
    public class GetInsuranceByIdInputData : IInputData<GetInsuranceByIdOutputData>
    {
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int HokenPid { get; private set; }
        public GetInsuranceByIdInputData(int hpId, long ptId, int sinDate, int hokenPid)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            HokenPid = hokenPid;
        }
    }
}
