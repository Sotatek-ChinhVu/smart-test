using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceInfor.Get
{
    public class GetInsuranceInforInputData: IInputData<GetInsuranceInforOutputData>
    {
        public long PtId { get; private set; }
        public int HokenId { get; private set; }

        public GetInsuranceInforInputData(long ptId, int hokenId)
        {
            PtId = ptId;
            HokenId = hokenId;
        }
    }
}
