using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientRaiinKubun
{
    public interface IPatientRaiinKubunReponsitory : IRepositoryBase
    {
        public IEnumerable<PatientRaiinKubunModel> GetPatientRaiinKubun(int hpId, long ptId, int raiinNo, int sinDate);

    }
}
