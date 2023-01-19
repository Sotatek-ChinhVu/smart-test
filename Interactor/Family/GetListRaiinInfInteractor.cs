using Domain.Models.Family;
using UseCase.Family.GetListRaiinInf;

namespace Interactor.Family;

public class GetListRaiinInfInteractor : IGetListRaiinInfInputPort
{
    private readonly IFamilyRepository _familyRepository;

    public GetListRaiinInfInteractor(IFamilyRepository familyRepository)
    {
        _familyRepository = familyRepository;
    }

    public GetListRaiinInfOutputData Handle(GetListRaiinInfInputData inputData)
    {
        try
        {
            if (inputData.PtId <= 0)
            {
                return new GetListRaiinInfOutputData(GetListRaiinInfStatus.InvalidPtId);
            }
            var listRaiinInf = _familyRepository.GetListRaiinInfByPtId(inputData.HpId, inputData.PtId);
            return new GetListRaiinInfOutputData(listRaiinInf.Select(item => new RaiinInfOutputItem(item)).ToList(), GetListRaiinInfStatus.Successed);
        }
        finally
        {
            _familyRepository.ReleaseResource();
        }
    }
}
