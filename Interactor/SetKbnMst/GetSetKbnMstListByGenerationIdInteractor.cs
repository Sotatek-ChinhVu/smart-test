using Domain.Models.SetKbnMst;
using UseCase.SetKbnMst.GetList;
using UseCase.SetKbnMst.GetSetKbnMstListByGenerationId;

namespace Interactor.SetKbnMst;

public class GetSetKbnMstListByGenerationIdInteractor : IGetSetKbnMstListByGenerationIdInputPort
{
    private readonly ISetKbnMstRepository _setKbnMstRepository;

    public GetSetKbnMstListByGenerationIdInteractor(ISetKbnMstRepository setKbnMstRepository)
    {
        _setKbnMstRepository = setKbnMstRepository;
    }

    public GetSetKbnMstListByGenerationIdOutputData Handle(GetSetKbnMstListByGenerationIdInputData inputData)
    {
        try
        {
            var result = _setKbnMstRepository.GetSetKbnMstListByGenerationId(inputData.HpId, inputData.GenerationId);
            return new GetSetKbnMstListByGenerationIdOutputData(result, GetSetKbnMstListByGenerationIdStatus.Successed);
        }
        finally
        {
            _setKbnMstRepository.ReleaseResource();
        }
    }
}
