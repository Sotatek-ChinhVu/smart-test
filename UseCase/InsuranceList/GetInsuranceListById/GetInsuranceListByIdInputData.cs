using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceList.GetInsuranceListById
{
    public class GetInsuranceListByIdInputData: IInputData<GetInsuranceListByIdOutputData>
    {
        public long PtId { get; private set; }
        public GetInsuranceListByIdInputData(long ptId)
        {
            PtId = ptId;
        }
    }
}
