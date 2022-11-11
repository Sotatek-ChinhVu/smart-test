using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetComboList
{
    public class GetInsuranceComboListOutputData : IOutputData
    {
        public List<GetInsuranceComboItemOuputData> Data { get; private set; }

        public GetInsuranceComboListStatus Status { get; private set; }

        public GetInsuranceComboListOutputData(List<GetInsuranceComboItemOuputData> data, GetInsuranceComboListStatus status)
        {
            Data = data;
            Status = status;
        }
    }
}