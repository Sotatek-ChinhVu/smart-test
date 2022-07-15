using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.InsuranceInfor.Get
{
    public enum GetInsuranceInforStatus: byte
    {
        DataNotExist = 0,
        Successed = 1,
        PtIdInvalid = 2,
        HokenIdInvalid = 3
    }
}
