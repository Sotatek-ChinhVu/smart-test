using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Reception.Get
{
    public enum GetReceptionStatus : byte
    {
        InvalidRaiinNo = 0,
        Successed = 1,
        ReceptionNotExisted = 2,
    }
}
