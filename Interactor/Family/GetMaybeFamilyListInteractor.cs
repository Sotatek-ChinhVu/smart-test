using Domain.Models.Family;
using UseCase.Family.GetMaybeFamilyList;
using UseCase.Family;

namespace Interactor.Family;

public class GetMaybeFamilyListInteractor : IGetMaybeFamilyListInputPort
{
    private readonly IFamilyRepository _familyRepository;

    public GetMaybeFamilyListInteractor(IFamilyRepository familyRepository)
    {
        _familyRepository = familyRepository;
    }

    public GetMaybeFamilyListOutputData Handle(GetMaybeFamilyListInputData inputData)
    {
        try
        {
            if (inputData.SinDate.ToString().Length != 8)
            {
                return new GetMaybeFamilyListOutputData(GetMaybeFamilyListStatus.InvalidSindate);
            }
            else if (inputData.PtId <= 0)
            {
                return new GetMaybeFamilyListOutputData(GetMaybeFamilyListStatus.InvalidPtId);
            }
            return new GetMaybeFamilyListOutputData(GetFamilyItemList(inputData), GetMaybeFamilyListStatus.Successed);
        }
        finally
        {
            _familyRepository.ReleaseResource();
        }
    }

    private List<FamilyItem> GetFamilyItemList(GetMaybeFamilyListInputData inputData)
    {
        var listData = _familyRepository.GetMaybeFamilyList(inputData.HpId, inputData.PtId, inputData.SinDate);
        return listData.Select(item => new FamilyItem(item)).ToList();
    }
}
