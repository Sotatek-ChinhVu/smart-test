using Domain.Models.Family;
using UseCase.Family.GetListFamily;

namespace Interactor.Family;

public class GetListFamilyInteractor : IGetListFamilyInputPort
{
    private readonly IFamilyRepository _familyRepository;

    public GetListFamilyInteractor(IFamilyRepository familyRepository)
    {
        _familyRepository = familyRepository;
    }

    public GetListFamilyOutputData Handle(GetListFamilyInputData inputData)
    {
        try
        {
            if (inputData.SinDate.ToString().Length != 8)
            {
                return new GetListFamilyOutputData(GetListFamilyStatus.InvalidSindate);
            }
            else if (inputData.PtId <= 0)
            {
                return new GetListFamilyOutputData(GetListFamilyStatus.InvalidPtId);
            }
            return new GetListFamilyOutputData(GetListFamilyOutputItem(inputData), GetListFamilyStatus.Successed);
        }
        finally
        {
            _familyRepository.ReleaseResource();
        }
    }

    private List<FamilyOutputItem> GetListFamilyOutputItem(GetListFamilyInputData inputData)
    {
        var listData = _familyRepository.GetListFamily(inputData.HpId, inputData.PtId, inputData.SinDate);
        return listData.Select(item => new FamilyOutputItem(item)).ToList();
    }
}
