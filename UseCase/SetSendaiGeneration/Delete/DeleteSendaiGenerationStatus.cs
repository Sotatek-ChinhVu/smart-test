using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.SetSendaiGeneration.Delete
{
    public enum DeleteSendaiGenerationStatus
    {
        Success = 1,
        InvalidRowIndex = 2,
        InvalidRowIndex0 = 3,
        InvalidGenerationId = 4,
        InvalidUserId = 5,
        Faild = 6
    }
}
