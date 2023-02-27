using Domain.Models.Family;
using UseCase.Family.GetRaiinInfList;

namespace Interactor.Family;

public class GetListRaiinInfInteractor : IGetRaiinInfListInputPort
{
    private readonly IFamilyRepository _familyRepository;

    public GetListRaiinInfInteractor(IFamilyRepository familyRepository)
    {
        _familyRepository = familyRepository;
    }

    public GetRaiinInfListOutputData Handle(GetRaiinInfListInputData inputData)
    {
        try
        {
            if (inputData.PtId <= 0)
            {
                return new GetRaiinInfListOutputData(GetRaiinInfListStatus.InvalidPtId);
            }
            var raiinInfList = _familyRepository.GetRaiinInfListByPtId(inputData.HpId, inputData.PtId);
            return new GetRaiinInfListOutputData(raiinInfList.Select(item => new RaiinInfOutputItem(item)).ToList(), GetRaiinInfListStatus.Successed);
        }
        finally
        {
            _familyRepository.ReleaseResource();
        }
    }
}
