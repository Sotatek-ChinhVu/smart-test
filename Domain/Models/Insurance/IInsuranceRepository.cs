using Domain.Models.InsuranceInfor;

namespace Domain.Models.Insurance
{
    public interface IInsuranceRepository
    {
        InsuranceDataModel GetInsuranceListById(int hpId, long ptId, int sinDate);

        IEnumerable<InsuranceModel> GetListHokenPattern(int hpId, long ptId, bool allowDisplayDeleted, bool isAllHoken = true, bool isHoken = true, bool isJihi = true, bool isRosai = true, bool isJibai = true);

        bool CheckHokenPIdList(List<int> hokenPIds, List<int> hpIds, List<long> ptIds);
    }
}
