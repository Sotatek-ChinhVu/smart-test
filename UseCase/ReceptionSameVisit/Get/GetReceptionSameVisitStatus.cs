using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.ReceptionSameVisit.Get
{
    public enum GetReceptionSameVisitStatus: byte
    {
        Success = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        InvalidSinDate = 4
    }
}
