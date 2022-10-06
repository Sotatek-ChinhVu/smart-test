using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidKohi
{
    public class ValidKohiOutputData : IOutputData
    {
        public bool Result { get; private set; }

        public string Message { get; private set; }

        public ValidKohiStatus Status { get; private set; }

        public ValidKohiOutputData(bool result, string message, ValidKohiStatus status)
        {
            Result = result;
            Message = message;
            Status = status;
        }
    }
}
