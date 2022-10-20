using Domain.Models.Reception;
using UseCase.Reception.GetLastRaiinInfs;

namespace Interactor.Reception;

public class GetLastRaiinInfsInteractor : IGetLastRaiinInfsInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public GetLastRaiinInfsInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public GetLastRaiinInfsOutputData Handle(GetLastRaiinInfsInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetLastRaiinInfsOutputData(GetLastRaiinInfsStatus.InvalidHpId);
            }
            else if (inputData.PtId <= 0)
            {
                return new GetLastRaiinInfsOutputData(GetLastRaiinInfsStatus.InvalidPtId);
            }
            else if (inputData.SinDate <= 10000101 || inputData.SinDate > 99999999)
            {
                return new GetLastRaiinInfsOutputData(GetLastRaiinInfsStatus.InvalidSinDate);
            }

            var data = _receptionRepository.GetLastRaiinInfs(inputData.HpId, inputData.PtId, inputData.SinDate);
            return new GetLastRaiinInfsOutputData(GetLastRaiinInfsStatus.Successed, data);
        }
        catch (Exception)
        {
            return new GetLastRaiinInfsOutputData(GetLastRaiinInfsStatus.Failed);
        }
    }
}
