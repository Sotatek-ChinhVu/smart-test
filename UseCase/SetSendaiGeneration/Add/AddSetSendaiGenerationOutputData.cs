using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.SetSendaiGeneration.Add
{
    public class AddSetSendaiGenerationOutputData: IOutputData
    {
        public AddSetSendaiGenerationOutputData(bool result, AddSetSendaiGenerationStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; private set; }
        public AddSetSendaiGenerationStatus Status { get; private set; }
    }
}
