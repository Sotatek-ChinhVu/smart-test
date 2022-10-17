using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidPatternExpirated
{
    public class ValidPatternExpiratedOutputData : IOutputData
    {
        public bool Result { get; private set; }

        public string Message { get; private set; }

        public ValidPatternExpiratedStatus Status { get; private set; }

        public ValidPatternExpiratedOutputData(bool result, string message, ValidPatternExpiratedStatus status)
        {
            Result = result;
            Message = message;
            Status = status;
        }
    }
}
