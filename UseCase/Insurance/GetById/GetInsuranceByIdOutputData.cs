using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetById
{
    public class GetInsuranceByIdOutputData : IOutputData
    {
        public InsuranceModel? Data { get; private set; }

        public GetInsuranceByIdStatus Status { get; private set; }
        public GetInsuranceByIdOutputData(InsuranceModel? data, GetInsuranceByIdStatus status)
        {
            Data = data;
            Status = status;
        }
    }
}
