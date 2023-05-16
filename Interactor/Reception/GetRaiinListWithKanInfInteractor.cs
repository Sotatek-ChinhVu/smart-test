using Domain.Models.Reception;
using UseCase.Reception.GetRaiinListWithKanInf;

namespace Interactor.Reception;

public class GetRaiinListWithKanInfInteractor : IGetRaiinListWithKanInfInputPort
{
    private readonly IReceptionRepository _receptionRepository;
    public GetRaiinListWithKanInfInteractor(IReceptionRepository receptionCommentRepository)
    {
        _receptionRepository = receptionCommentRepository;
    }

    public GetRaiinListWithKanInfOutputData Handle(GetRaiinListWithKanInfInputData inputData)
    {
        try
        {
            if (inputData.PtId <= 0)
            {
                return new GetRaiinListWithKanInfOutputData(new(), GetRaiinListWithKanInfStatus.InvalidPtId);
            }
            var raiinList = _receptionRepository.GetRaiinListWithKanInf(inputData.HpId, inputData.PtId);
            return new GetRaiinListWithKanInfOutputData(raiinList, GetRaiinListWithKanInfStatus.Successed);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}
