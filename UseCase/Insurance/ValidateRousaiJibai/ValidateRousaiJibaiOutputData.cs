using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidateRousaiJibai
{
    public class ValidateRousaiJibaiOutputData : IOutputData
    {
        public bool Result { get; private set; }

        public string Message { get; private set; }

        public ValidateRousaiJibaiStatus Status { get; private set; }

        public ValidateRousaiJibaiOutputData(bool result, string message, ValidateRousaiJibaiStatus status)
        {
            Result = result;
            Message = message;
            Status = status;
        }
    }
}
