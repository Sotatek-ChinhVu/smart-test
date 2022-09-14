using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetList
{
    public class GetInsuranceListByIdOutputData : IOutputData
    {
        public InsuranceDataModel Data { get; private set; }

        public GetInsuranceListStatus Status { get; private set; }

        public GetInsuranceListByIdOutputData(InsuranceDataModel data, GetInsuranceListStatus status)
        {
            Data = data;
            Status = status;
        }
    }
}