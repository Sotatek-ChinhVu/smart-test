using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.PatientRaiinKubun.Get
{
    public enum GetPatientRaiinKubunStatus: byte
    {

        InvalidSinDate = 5,
        InvalidRaiinNo = 4,
        InvalidPtId = 3,
        InvalidHpId = 2,
        Successed = 1,
    }
}
