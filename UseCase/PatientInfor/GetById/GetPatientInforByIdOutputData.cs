using Domain.Models.PatientInfor;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
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
        public PatientInforModel? PatientInfor { get; private set; }
        public GetPatientInforByIdStatus Status { get; private set; }
        public GetPatientInforByIdOutputData(PatientInforModel? data, GetPatientInforByIdStatus getPatientInforByIdStatus)
        {
            PatientInfor = data;
            Status = getPatientInforByIdStatus;
        }
    }
}