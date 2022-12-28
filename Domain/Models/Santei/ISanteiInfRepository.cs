using Domain.Common;

namespace Domain.Models.Santei;

public interface ISanteiInfRepository : IRepositoryBase
{
    List<SanteiInfModel> GetListSanteiInfModel(int hpId, long ptId, int sinDate);

    List<SanteiInfDetailModel> GetListSanteiInfDetailModel(int hpId, long ptId, int sinDate);

    List<string> GetListSanteiByomeis(int hpId, long ptId, int sinDate);

    bool SaveListSanteiInf(int hpId, int userId, SanteiInfModel model);

    bool SaveListSanteiInfDetail(int hpId, int userId, SanteiInfDetailModel model);
}
