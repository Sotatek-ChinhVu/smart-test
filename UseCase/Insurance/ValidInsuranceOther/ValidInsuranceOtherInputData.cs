using Domain.Models.Insurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidPatternOther
{
    public class ValidInsuranceOtherInputData : IInputData<ValidInsuranceOtherOutputData>
    {
        public ValidInsuranceOtherInputData(ValidInsuranceOtherModel validModel)
        {
            ValidModel = validModel;
        }

        public ValidInsuranceOtherModel ValidModel { get; private set; }
    }
}
