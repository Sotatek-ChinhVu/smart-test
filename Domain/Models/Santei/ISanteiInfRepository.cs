using Domain.Common;

namespace Domain.Models.Santei;

public interface ISanteiInfRepository : IRepositoryBase
{
    List<SanteiInfModel> GetListSanteiInf(int hpId, long ptId, int sinDate);

    List<SanteiInfDetailModel> GetListSanteiInfDetails(int hpId, long ptId);

    List<SanteiInfModel> GetOnlyListSanteiInf(int hpId, long ptId);

    public bool CheckExistItemCd(int hpId, List<string> listItemCds);

    bool SaveSantei(int hpId, int userId, List<SanteiInfModel> listSanteiInfModels);

    List<SanteiInfModel> GetCalculationInfo(int hpId, long ptId, int sinDate);
}
