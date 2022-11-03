using Domain.Models.Insurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidateInsurance
{
    public class ValidateInsuranceInputData: IInputData<ValidateInsuranceOutputData>
    {
        public ValidateInsuranceInputData(int hpId, int sinDate, int ptBirthday, List<ValidateInsuranceDto> listDataModel)
        {
            HpId = hpId;
            SinDate = sinDate;
            PtBirthday = ptBirthday;
            ListDataModel = listDataModel;
        }
        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public int PtBirthday { get; private set; }

        public List<ValidateInsuranceDto> ListDataModel { get; private set; }
    }
}
