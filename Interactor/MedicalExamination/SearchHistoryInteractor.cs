using Domain.Models.HistoryOrder;
using UseCase.MedicalExamination.SearchHistory;

namespace Interactor.MedicalExamination
{
    public class SearchHistoryInteractor : ISearchHistoryInputPort
    {
        private readonly IHistoryOrderRepository _historyRepository;
        private readonly int defaultIndex = -1;

        public SearchHistoryInteractor(IHistoryOrderRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public SearchHistoryOutputData Handle(SearchHistoryInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new SearchHistoryOutputData(new(), defaultIndex, SearchHistoryStatus.InvalidHpId);
                }
                if (inputData.UserId <= 0)
                {
                    return new SearchHistoryOutputData(new(), defaultIndex, SearchHistoryStatus.InvalidUserId);
                }
                if (inputData.PtId <= 0)
                {
                    return new SearchHistoryOutputData(new(), defaultIndex, SearchHistoryStatus.InvalidPtId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new SearchHistoryOutputData(new(), defaultIndex, SearchHistoryStatus.InvalidSinDate);
                }
                if (inputData.CurrentIndex < 0)
                {
                    return new SearchHistoryOutputData(new(), defaultIndex, SearchHistoryStatus.InvalidCurrentIndex);
                }
                if (inputData.FilterId < 0)
                {
                    return new SearchHistoryOutputData(new(), defaultIndex, SearchHistoryStatus.InvalidFilterId);
                }
                if (!(inputData.IsDeleted >= 0 && inputData.IsDeleted <= 2))
                {
                    return new SearchHistoryOutputData(new(), defaultIndex, SearchHistoryStatus.InvalidIsDeleted);
                }
                if (!(inputData.SearchType >= 0 && inputData.SearchType <= 2))
                {
                    return new SearchHistoryOutputData(new(), defaultIndex, SearchHistoryStatus.InvalidSearchType);
                }

                var result = _historyRepository.Search(inputData.HpId, inputData.UserId, inputData.PtId, inputData.SinDate, inputData.CurrentIndex, inputData.FilterId, inputData.IsDeleted, inputData.KeyWord, inputData.SearchType, inputData.IsNext);

                return new SearchHistoryOutputData(new ReceptionItem(result.Item2), result.Item1, SearchHistoryStatus.Successed);
            }
            catch
            {
                return new SearchHistoryOutputData(new(), defaultIndex, SearchHistoryStatus.Failed);
            }
            finally
            {
                _historyRepository.ReleaseResource();
            }
        }
    }
}
