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
        public List<PatientInforModel> PatientInforModels { get; set; }

        public GetPatientCommentStatus Status { get; set; }

        public GetPatientCommentOutputData(List<PatientInforModel> patientInforModels, GetPatientCommentStatus status)
        {
            PatientInforModels = patientInforModels;
            Status = status;
        }
    }
}
