using Domain.Models.PatientInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInformation.GetById
{
    public class GetPatientInforByIdOutputData : IOutputData
    {
        public PatientInfor? PatientInfor { get; private set; }
        public GetPatientInforByIdOutputData(PatientInfor? data)
        {
            PatientInfor = data;
        }
    }
}
