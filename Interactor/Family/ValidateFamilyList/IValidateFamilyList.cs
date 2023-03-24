using UseCase.Family;

namespace Interactor.Family.ValidateFamilyList;

public interface IValidateFamilyList
{
    ValidateFamilyListStatus ValidateData(int hpId, long ptId, List<FamilyItem> listFamily);

    void ReleaseResource();
}
