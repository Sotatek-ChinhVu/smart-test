using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Insurance.ValidPatternOther
{
    public enum ValidInsuranceOtherStatus: byte
    {
        Success = 0,
        InvalidSindate = 1,
        InvalidPtBirthday = 2,
        InvalidAge75 = 3,
        InvalidAge65 = 4,
        InvalidDuplicatePattern = 5
    }
}
