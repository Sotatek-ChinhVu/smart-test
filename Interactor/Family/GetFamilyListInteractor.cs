using Domain.Models.Family;
using UseCase.Family.GetListFamily;

namespace Interactor.Family;

public class GetFamilyListInteractor : IGetFamilyListInputPort
{
    private readonly IFamilyRepository _familyRepository;

    public GetFamilyListInteractor(IFamilyRepository familyRepository)
    {
        _familyRepository = familyRepository;
    }

    public GetFamilyListOutputData Handle(GetFamilyListInputData inputData)
    {
        try
        {
            if (inputData.SinDate.ToString().Length != 8)
            {
                return new GetFamilyListOutputData(GetFamilyListStatus.InvalidSindate);
            }
            else if (inputData.PtId <= 0)
            {
                return new GetFamilyListOutputData(GetFamilyListStatus.InvalidPtId);
            }
            return new GetFamilyListOutputData(GetListFamilyOutputItem(inputData), GetFamilyListStatus.Successed);
        }
        finally
        {
            _familyRepository.ReleaseResource();
        }
    }

    private List<FamilyOutputItem> GetListFamilyOutputItem(GetFamilyListInputData inputData)
    {
        var listData = _familyRepository.GetFamilyList(inputData.HpId, inputData.PtId, inputData.SinDate);
        return listData.Select(item => new FamilyOutputItem(item)).ToList();
    }
}
