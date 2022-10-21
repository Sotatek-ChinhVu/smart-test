using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Reception.GetReceptionDefault
{
    public enum GetReceptionDefaultStatus: byte
    {
        Successed = 0,
        InvalidHpId = 1,
        InvalidPtId = 2,
        InvalidSindate = 3,
        InvalidDefautDoctorSetting = 4,
        Failed = 5
    }
}
