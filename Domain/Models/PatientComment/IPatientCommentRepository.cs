using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientComment
{
    public interface IPatientCommentRepository
    {
        public List<PatientCommentModel> patientCommentModels(int hpId, long pdId);
    }
}
