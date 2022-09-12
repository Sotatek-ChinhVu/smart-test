using Domain.Models.MstItem;
using UseCase.MstItem.GetDosageDrugList;

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
                var datas = _inputItemRepository.GetListSupplement(inputData.SearchValue,inputData.PageIndex,inputData.PageSize);
                if (!(datas?.Count > 0))
                {
                    return new SearchSupplementOutputData(new List<SearchSupplementModel>(), SearchSupplementStatus.NoData);
                }

                return new SearchSupplementOutputData(datas, SearchSupplementStatus.Successed);
            }
            catch
            {
                return new SearchSupplementOutputData(new List<SearchSupplementModel>(), SearchSupplementStatus.Fail);
            }
        }
    }
}
