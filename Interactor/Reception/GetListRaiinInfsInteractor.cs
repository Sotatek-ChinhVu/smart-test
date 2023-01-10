using Domain.Models.Reception;
using UseCase.Reception.GetLastRaiinInfs;
using UseCase.Reception.GetListRaiinInfs;

namespace Interactor.RaiinFilterMst;

public class GetListRaiinInfsInteractor : IGetListRaiinInfsInputPort
{
    private readonly IReceptionRepository _raiinInfRepository;

    public GetListRaiinInfsInteractor(IReceptionRepository raiinInfRepository)
    {
        _raiinInfRepository = raiinInfRepository;
    }

    public GetListRaiinInfsOutputData Handle(GetListRaiinInfsInputData inputData)
    {
        var raiinInfs = _raiinInfRepository.GetListRaiinInf(inputData.HpId, inputData.PtId);
        var status = raiinInfs.Any() ? GetListRaiinInfsStatus.Success : GetListRaiinInfsStatus.NoData;
        return new GetListRaiinInfsOutputData(status, raiinInfs);
    }
}