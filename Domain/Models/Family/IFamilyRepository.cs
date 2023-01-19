using Domain.Common;

namespace Domain.Models.Family;

public interface IFamilyRepository : IRepositoryBase
{
    List<FamilyModel> GetListFamily(int hpId, long ptId, int sinDate);

    List<FamilyModel> GetListFamilyReverser(int hpId, long familyPtId, List<long> listPtIdInput);

    bool SaveListFamily(int hpId, int userId, List<FamilyModel> listFamily);

    List<FamilyModel> GetListByPtId(int hpId, long ptId);

    bool CheckExistListFamilyReki(int hpId, List<long> listFamilyRekiId);

    List<RaiinInfModel> GetListRaiinInfByPtId(int hpId, long ptId);
}
