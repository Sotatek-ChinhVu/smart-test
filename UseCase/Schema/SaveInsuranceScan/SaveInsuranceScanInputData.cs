using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;

namespace Schema.Insurance.SaveInsuranceScan
{
    public class SaveInsuranceScanInputData : IInputData<SaveInsuranceScanOutputData>
    {
        public SaveInsuranceScanInputData(int hpId, int userId,IEnumerable<InsuranceScanModel> insuranceScans)
        {
            HpId = hpId;
            UserId = userId;
            InsuranceScans = insuranceScans;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public IEnumerable<InsuranceScanModel> InsuranceScans { get; private set; }
    }
}
