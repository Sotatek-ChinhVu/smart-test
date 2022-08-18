using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.InputItem.UpdateAdopted
{
    public enum UpdateAdoptedInputItemStatus: byte
    {
        Successed = 1,
        InvalidValueAdopted = 2,
        InvalidItemCd = 3,
        InvalidSinDate = 4
    }
}
