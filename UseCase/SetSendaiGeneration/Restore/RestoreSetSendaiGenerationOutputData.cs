using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.SetSendaiGeneration.Restore
{
    public class RestoreSetSendaiGenerationOutputData: IOutputData
    {
        public RestoreSetSendaiGenerationOutputData(bool result, RestoreSetSendaiGenerationStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; set; }
        public RestoreSetSendaiGenerationStatus Status { get; set; }
    }
}
