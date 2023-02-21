using Domain.Common;

namespace Domain.Models.Family;

public interface IFamilyRepository : IRepositoryBase
{
    List<FamilyModel> GetFamilyList(int hpId, long ptId, int sinDate);

    List<FamilyModel> GetFamilyReverserList(int hpId, long familyPtId, List<long> ptIdInputList);

    bool SaveFamilyList(int hpId, int userId, List<FamilyModel> familyList);

    List<FamilyModel> GetListByPtId(int hpId, long ptId);

    bool CheckExistFamilyRekiList(int hpId, List<long> familyRekiIdList);
}
