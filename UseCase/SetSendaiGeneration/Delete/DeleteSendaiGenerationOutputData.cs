using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.SetSendaiGeneration.Delete
{
    public class DeleteSendaiGenerationOutputData: IOutputData
    {
        public DeleteSendaiGenerationOutputData(bool checkResult, DeleteSendaiGenerationStatus status)
        {
            CheckResult = checkResult;
            Status = status;
        }

        public bool CheckResult { get; private set; }
        public DeleteSendaiGenerationStatus Status { get; private set; }
    }
}
