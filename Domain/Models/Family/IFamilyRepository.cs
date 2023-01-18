using Domain.Common;

namespace Domain.Models.Family;

public interface IFamilyRepository : IRepositoryBase
{
    List<FamilyModel> GetListFamily(int hpId, long ptId, int sinDate);

    bool SaveListFamily(int hpId, int userId, List<FamilyModel> listFamily);

    List<FamilyModel> GetListByPtIdId(int hpId, long ptId);

    List<FamilyModel> GetListByFamilyPtId(int hpId, List<long> listPtId);

    bool CheckExistListFamilyReki(int hpId, List<long> listFamilyRekiId);
}
