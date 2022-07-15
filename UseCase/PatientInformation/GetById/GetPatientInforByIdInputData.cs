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
        public PtId PtId { get; private set; }

        public GetPatientInforByIdInputData(int ptId)
        {
            PtId = PtId.From(ptId);
        }
    }
}
