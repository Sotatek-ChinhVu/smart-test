using Domain.Models.InsuranceMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.Get
{
    public class GetInsuranceMstOutputData : IOutputData
    {
        public InsuranceMstModel InsuranceMstData { get; private set; }

        public GetInsuranceMstStatus Status { get; private set; }

        public GetInsuranceMstOutputData(InsuranceMstModel data, GetInsuranceMstStatus status)
        {
            InsuranceMstData = data;
            Status = status;
        }
    }
}
