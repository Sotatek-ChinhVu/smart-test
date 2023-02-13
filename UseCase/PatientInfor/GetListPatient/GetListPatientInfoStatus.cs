using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.PatientInfor.GetListPatient
{
    public enum GetListPatientInfoStatus
    {
        Success = 1,
        InvalidPtId,
        InvalidHpId,
        InvalidPageIndex,
        InvalidPageSize
    }
}
