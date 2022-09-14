using Domain.Models.PatientComment;
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
        public List<PatientCommentModel> patientCommentModels { get; set; }

        public GetPatientCommentStatus Status { get; set; }

        public GetPatientCommentOutputData(List<PatientCommentModel> commentModels, GetPatientCommentStatus status)
        {
            patientCommentModels = commentModels;
            Status = status;
        }
    }
}
