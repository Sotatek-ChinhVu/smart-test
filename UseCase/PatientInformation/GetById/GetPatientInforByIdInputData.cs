using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInformation.GetById
{
    public class GetPatientInforByIdInputData : IInputData<GetPatientInforByIdOutputData>
    {
        public long PtId { get; private set; }

        public GetPatientInforByIdInputData(long ptId)
        {
            PtId = ptId;
        }
    }
}
