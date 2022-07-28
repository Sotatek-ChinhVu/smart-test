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
        public List<InsuranceModel> ListData { get; private set; }

        public GetInsuranceListStatus Status { get; private set; }
        public GetInsuranceListByIdOutputData(List<InsuranceModel> listData, GetInsuranceListStatus status)
        {
            ListData = listData;
            Status = status;
        }
    }
}