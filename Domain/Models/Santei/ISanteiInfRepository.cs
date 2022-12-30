using Domain.Common;

namespace Domain.Models.Santei;

public interface ISanteiInfRepository : IRepositoryBase
{
    List<SanteiInfModel> GetListSanteiInf(int hpId, long ptId, int sinDate);

    List<SanteiInfDetailModel> GetListSanteiInfDetails(int hpId, long ptId);

    List<SanteiInfModel> GetOnlyListSanteiInf(int hpId, long ptId);

    List<string> GetListSanteiByomeis(int hpId, long ptId, int sinDate, int hokenPid);

    public bool CheckExistItemCd(int hpId, List<string> listItemCds);

    bool SaveSantei(int hpId, int userId, List<SanteiInfModel> listSanteiInfModels, List<SanteiInfDetailModel> listSanteiInfDetailModels);
}
