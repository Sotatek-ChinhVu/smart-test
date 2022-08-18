using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.PatientInformation.GetById
{
    public enum GetPatientInforByIdStatus : byte
    {
        Successed = 1,
        DataNotExist = 2,
        InvalidHpId = 3,
        InvalidPtId = 4,
        InvalidSinDate = 5,
        InvalidRaiinNo = 6
    }
}