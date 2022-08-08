using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.ReceptionInsurance.Get
{
    public enum GetReceptionInsuranceStatus : byte
    {
        InvalidSinDate = 4,
        InvalidPtId = 3,
        InvalidHpId = 2,
        Successed = 1
    }
}
