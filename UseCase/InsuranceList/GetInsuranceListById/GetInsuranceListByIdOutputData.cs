using Domain.Models.InsuranceList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceList.GetInsuranceListById
{
    public class GetInsuranceListByIdOutputData: IOutputData
    {
        public List<InsuranceListModel> ListData { get; private set; }

        public GetInsuranceListByIdStatus Status { get; private set; }
        public GetInsuranceListByIdOutputData(List<InsuranceListModel> listData, GetInsuranceListByIdStatus status)
        {
            ListData = listData;
            Status = status;
        }
    }
}
