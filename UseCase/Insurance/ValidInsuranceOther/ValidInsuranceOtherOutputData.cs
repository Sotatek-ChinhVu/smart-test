using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidPatternOther
{
    public class ValidInsuranceOtherOutputData : IOutputData
    {
        public bool Result { get; private set; }

        public string Message { get; private set; }

        public ValidInsuranceOtherStatus Status { get; private set; }

        public ValidInsuranceOtherOutputData(bool result, string message, ValidInsuranceOtherStatus status)
        {
            Result = result;
            Message = message;
            Status = status;
        }
    }
}
