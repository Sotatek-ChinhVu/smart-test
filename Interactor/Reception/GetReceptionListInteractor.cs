using Domain.Models.Reception;
using UseCase.Reception.GetList;

namespace Interactor.Reception;

public class GetReceptionListInteractor : IGetReceptionListInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public GetReceptionListInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public GetReceptionListOutputData Handle(GetReceptionListInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetReceptionListOutputData(GetReceptionListStatus.InvalidHpId);
            }
            if (inputData.SinDate <= 0)
            {
                return new GetReceptionListOutputData(GetReceptionListStatus.InvalidSinDate);
            }

            var receptionInfos = _receptionRepository.GetList(inputData.HpId, inputData.SinDate, inputData.RaiinNo, inputData.PtId, false, inputData.IsGetFamily);
            return new GetReceptionListOutputData(GetReceptionListStatus.Success, receptionInfos);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}
