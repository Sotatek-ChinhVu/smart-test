using Domain.Models.InsuranceInfor;

namespace Domain.Models.Insurance
{
    public interface IInsuranceRepository
    {
        IEnumerable<InsuranceModel> GetInsuranceListById(int hpId, long ptId, int sinDate);
        IEnumerable<InsuranceModel> GetInsuranceListById(int hpId, long ptId, int sinDate, int prefCd);
    }
}
