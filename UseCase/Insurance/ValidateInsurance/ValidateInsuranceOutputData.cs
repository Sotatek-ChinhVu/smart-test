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


        public ValidateInsuranceStatus Status { get; private set; }

        public List<ValidateInsuranceListItem> ListResult { get; private set; }

        public ValidateInsuranceOutputData(bool result, ValidateInsuranceStatus status, List<ValidateInsuranceListItem> listResult)
        {
            Result = result;
            Status = status;
            ListResult = listResult;
        }
    }
}
