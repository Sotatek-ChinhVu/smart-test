using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.VisitingList.PatientComment
{
    public class GetPatientCommentOutputData : IOutputData
    {
        public PatientInforModel PatientInforModels { get; set; }

        public GetPatientCommentStatus Status { get; set; }

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
