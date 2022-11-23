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

        public int TypeMessage { get; private set; }

        public ValidateRousaiJibaiStatus Status { get; private set; }

        public ValidateRousaiJibaiOutputData(bool result, string message, int typeMessage, ValidateRousaiJibaiStatus status)
        {
            Result = result;
            Message = message;
            TypeMessage = typeMessage;
            Status = status;
        }
    }
}
