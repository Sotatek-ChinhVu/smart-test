using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidMainInsurance
{
    public class ValidMainInsuranceOutputData : IOutputData
    {
        public bool Result { get; private set; }

        public string Message { get; private set; }

        public ValidMainInsuranceStatus Status { get; private set; }

        public ValidMainInsuranceOutputData(bool result, string message, ValidMainInsuranceStatus status)
        {
            Result = result;
            Message = message;
            Status = status;
        }
    }
}
