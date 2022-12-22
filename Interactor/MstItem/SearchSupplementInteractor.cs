using Domain.Models.MstItem;
using UseCase.MstItem.SearchSupplement;

namespace Interactor.MstItem
{
    public class SearchSupplementInteractor : ISearchSupplementInputPort
    {
        private readonly IMstItemRepository _inputItemRepository;

        public SearchSupplementInteractor(IMstItemRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }
        public SearchSupplementOutputData Handle(SearchSupplementInputData inputData)
        {
            try
            {
                var datas = _inputItemRepository.GetListSupplement(inputData.SearchValue, inputData.PageIndex, inputData.PageSize);
                if (datas.Item1 == null || !(datas.Item2 > 0))
                {
                    return new SearchSupplementOutputData(new List<SearchSupplementModel>(), 0, SearchSupplementStatus.NoData);
                }

                return new SearchSupplementOutputData(datas.Item1, datas.Item2, SearchSupplementStatus.Successed);
            }
            catch
            {
                return new SearchSupplementOutputData(new List<SearchSupplementModel>(), 0, SearchSupplementStatus.Fail);
            }
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}
