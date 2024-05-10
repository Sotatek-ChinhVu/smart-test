using Domain.Models.Reception;
using UseCase.Reception.GetPagingList;

namespace Interactor.Reception;

public class GetReceptionPagingListInteractor : IGetReceptionPagingListInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public GetReceptionPagingListInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public GetReceptionPagingListOutputData Handle(GetReceptionPagingListInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetReceptionPagingListOutputData(GetReceptionPagingListStatus.InvalidHpId);
            }
            if (inputData.SinDate <= 0)
            {
                return new GetReceptionPagingListOutputData(GetReceptionPagingListStatus.InvalidSinDate);
            }

            var result = _receptionRepository.GetPagingList(inputData.HpId, inputData.SinDate, inputData.RaiinNo, inputData.PtId, false, inputData.IsGetFamily, inputData.IsDeleted, inputData.SearchSameVisit, inputData.Limit, inputData.Offset);
            return new GetReceptionPagingListOutputData(GetReceptionPagingListStatus.Success, result.receptionInfos, result.totalItems);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}
