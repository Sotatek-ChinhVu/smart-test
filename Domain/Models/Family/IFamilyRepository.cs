using Domain.Common;

namespace Domain.Models.Family;

public interface IFamilyRepository : IRepositoryBase
{
    List<FamilyModel> GetListFamily(int hpId, long ptId, int sinDate);

    bool SaveListFamily(int hpId, int userId, long ptId, List<FamilyModel> listFamily);

    List<FamilyModel> GetListFamilyForValidate(int hpId, List<long> listFamilyId);

    bool CheckExistListFamilyReki(int hpId, List<long> listFamilyRekiId);
}
