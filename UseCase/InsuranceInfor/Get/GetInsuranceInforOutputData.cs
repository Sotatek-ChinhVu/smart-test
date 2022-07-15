using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceInfor.Get
{
    public class GetInsuranceInforOutputData: IOutputData
    {
        public InsuranceInforModel? InsuranceInfor { get; private set; }
        public GetInsuranceInforStatus Status { get; private set; } 
        public GetInsuranceInforOutputData(InsuranceInforModel? insuranceInfor, GetInsuranceInforStatus status)
        {
            InsuranceInfor = insuranceInfor;
            Status = status;
        }
    }
}
