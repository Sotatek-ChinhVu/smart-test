using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidateInsurance
{
    public class ValidateInsuranceOutputData : IOutputData
    {
        public bool Result { get; private set; }

        public string Message { get; private set; }

        public ValidateInsuranceStatus Status { get; private set; }

        public int IndexItemError { get; private set; }

        public ValidateInsuranceOutputData(bool result, string message, ValidateInsuranceStatus status, int indexItemError)
        {
            Result = result;
            Message = message;
            Status = status;
            IndexItemError = indexItemError;
        }
    }
}
