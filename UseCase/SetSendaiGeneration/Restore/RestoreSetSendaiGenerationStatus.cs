using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.SetSendaiGeneration.Restore
{
    public enum RestoreSetSendaiGenerationStatus
    {
        Success = 0,
        InvalidRestoreGenerationId = 1,
        InvalidHpId = 2,
        InvalidUserId = 3,
        Faild = 4
    }
}
