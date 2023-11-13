using Domain.Models.SetGenerationMst;
using UseCase.SetMst.GetListSetGenerationMst;

namespace Interactor.SetMst;

public class GetSetGenerationMstListInteractor : IGetSetGenerationMstListInputPort
{
    private readonly ISetGenerationMstRepository _setGenerationMstRepository;

    public GetSetGenerationMstListInteractor(ISetGenerationMstRepository setGenerationMstRepository)
    {
        _setGenerationMstRepository = setGenerationMstRepository;
    }

    public GetSetGenerationMstListOutputData Handle(GetSetGenerationMstListInputData inputData)
    {
        try
        {
            var result = _setGenerationMstRepository.GetSetGenerationMstList(inputData.HpId);
            return new GetSetGenerationMstListOutputData(GetSetGenerationMstListStatus.Successed, result);
        }
        finally
        {
            _setGenerationMstRepository.ReleaseResource();
        }
    }
}
