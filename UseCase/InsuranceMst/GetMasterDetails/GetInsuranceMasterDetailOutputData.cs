using Domain.Models.InsuranceMst;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.GetMasterDetails
{
    public class GetInsuranceMasterDetailOutputData : IOutputData
    {
        public IEnumerable<InsuranceMasterDetailModel> InsuranceMstData { get; private set; }

        public GetInsuranceMasterDetailStatus Status { get; private set; }

        public GetInsuranceMasterDetailOutputData(IEnumerable<InsuranceMasterDetailModel> data, GetInsuranceMasterDetailStatus status)
        {
            InsuranceMstData = data;
            Status = status;
        }
    }
}
