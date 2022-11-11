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

        public int TypeMessage { get; private set; }

        public ValidPatternExpiratedStatus Status { get; private set; }

        public ValidPatternExpiratedOutputData(bool result, string message, int typeMessage, ValidPatternExpiratedStatus status)
        {
            Result = result;
            Message = message;
            TypeMessage = typeMessage;
            Status = status;
        }
    }
}
