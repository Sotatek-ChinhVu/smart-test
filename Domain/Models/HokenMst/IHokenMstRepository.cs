using Domain.Common;
using Domain.Models.InsuranceMst;

namespace Domain.Models.HokenMst
{
    public interface IHokenMstRepository : IRepositoryBase
    {
        HokenMasterModel GetHokenMaster(int hpId, int hokenNo, int hokenEdaNo, int prefNo, int sinDate);
        List<HokenMasterModel> CheckExistHokenEdaNo(int hokenNo, int hpId);
        List<HokenMstModel> FindHokenMst(int hpId);
    }
}
