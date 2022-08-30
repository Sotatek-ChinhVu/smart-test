using Domain.Models.ReceptionInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceptionInsurance.Get
{
    public class GetReceptionInsuranceOutputData : IOutputData
    {
        public List<ReceptionInsuranceModel> ListReceptionInsurance { get; private set; }

        public GetReceptionInsuranceStatus Status { get; private set; }

        public GetReceptionInsuranceOutputData(List<ReceptionInsuranceModel> listReceptionInsurance, GetReceptionInsuranceStatus status)
        {
            ListReceptionInsurance = listReceptionInsurance;
            Status = status;
        }
    }
}
