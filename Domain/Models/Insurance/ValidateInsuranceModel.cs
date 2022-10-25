using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class ValidateInsuranceModel
    {
        public ValidateInsuranceModel(HokenInfModel selectedHokenInf, InsuranceModel selectedHokenPattern, HokenMstModel selectedHokenMst)
        {
            SelectedHokenInf = selectedHokenInf;
            SelectedHokenPattern = selectedHokenPattern;
            SelectedHokenMst = selectedHokenMst;
        }
        public HokenInfModel SelectedHokenInf { get; private set; }

        public InsuranceModel SelectedHokenPattern { get; private set; }

        public HokenMstModel SelectedHokenMst { get; private set; }

        public bool IsHaveSelectedHokenPattern => SelectedHokenPattern != null;

        public bool IsHaveSelectedHokenInf => SelectedHokenInf != null;

        public bool IsHaveSelectedHokenMst => SelectedHokenMst != null;
    }
}
