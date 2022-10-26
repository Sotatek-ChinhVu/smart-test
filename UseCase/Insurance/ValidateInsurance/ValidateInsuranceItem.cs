using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Insurance.ValidateInsurance
{
    public class ValidateInsuranceItem
    {
        public ValidateInsuranceItem(bool result, string message, ValidateInsuranceStatus status)
        {
            Result = result;
            Message = message;
            Status = status;
        }

        public bool Result { get; private set; }

        public string Message { get; private set; }

        public ValidateInsuranceStatus Status { get; private set; }
    }

    public class ValidateInsuranceListItem
    {
        public ValidateInsuranceListItem(List<ValidateInsuranceItem> listValidate)
        {
            ListValidate = listValidate;
        }

        public List<ValidateInsuranceItem> ListValidate { get; private set; }
    }
}
