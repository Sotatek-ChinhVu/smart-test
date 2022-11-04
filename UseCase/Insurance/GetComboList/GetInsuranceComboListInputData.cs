using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetComboList
{
    public class GetInsuranceComboListInputData : IInputData<GetInsuranceComboListOutputData>
    {
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public GetInsuranceComboListInputData(int hpId, long ptId, int sinDate)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
        }
    }
}