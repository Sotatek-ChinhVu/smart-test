using Domain.Models.PatientInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.PatientComment
{
    public class GetPatientCommentOutputData : IOutputData
    {
        public PatientInforModel PatientInforModels { get; private set; }

        public GetPatientCommentStatus Status { get; private set; }

        public GetPatientCommentOutputData(PatientInforModel patientInforModels, GetPatientCommentStatus status)
        {
            PatientInforModels = patientInforModels;
            Status = status;
        }

        public GetPatientCommentOutputData(GetPatientCommentStatus status)
        {
            PatientInforModels = new PatientInforModel();
            Status = status;
        }
    }
}
