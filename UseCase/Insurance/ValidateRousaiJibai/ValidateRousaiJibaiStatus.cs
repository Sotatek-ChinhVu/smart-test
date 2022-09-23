using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Insurance.ValidateRousaiJibai
{
    public enum ValidateRousaiJibaiStatus: byte
    {
        InvalidSuccess = 0,
        InvalidHokenKbn = 1,
        InvalidSinDate = 2,
        InvalidSelectedHokenInfRousaiSaigaiKbn = 3,
        InvalidSelectedHokenInfRousaiSyobyoDate = 4,
        InvalidSelectedHokenInfRyoyoStartDate = 5,
        InvalidSelectedHokenInfRyoyoEndDate = 6,
        InvalidSelectedHokenInfStartDate = 7,
        InvalidSelectedHokenInfEndDate = 8,
        InvalidSelectedHokenInfConfirmDate = 9
    }
}
