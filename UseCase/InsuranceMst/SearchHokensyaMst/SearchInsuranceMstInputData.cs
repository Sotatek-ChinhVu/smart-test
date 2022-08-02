using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.Get
{
    public class SearchInsuranceMstInputData : IInputData<GetInsuranceMstOutputData>
    {
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int HokenId { get; private set; }

        public SearchInsuranceMstInputData(int hpId, long ptId, int sinDate, int hokenId)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            HokenId = hokenId;
        }
    }
}
