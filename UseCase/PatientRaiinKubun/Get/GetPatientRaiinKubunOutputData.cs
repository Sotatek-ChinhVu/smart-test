using Domain.Models.PatientRaiinKubun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientRaiinKubun.Get
{
    public class GetPatientRaiinKubunOutputData : IOutputData
    {
        public GetPatientRaiinKubunOutputData(List<PatientRaiinKubunModel> listPatientRaiinKubun, GetPatientRaiinKubunStatus status)
        {
            ListPatientRaiinKubun = listPatientRaiinKubun;
            Status = status;
        }

        public List<PatientRaiinKubunModel> ListPatientRaiinKubun { get; private set; }

        public GetPatientRaiinKubunStatus Status { get; private set; }

    }
}
