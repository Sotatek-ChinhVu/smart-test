using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.SetSendaiGeneration.Add
{
    public enum AddSetSendaiGenerationStatus
    {
        Success = 0,
        InvalidHpId = 1,
        InvalidUserId = 2,
        InvalidStartDate = 3,
        Faild = 4
    }
}
