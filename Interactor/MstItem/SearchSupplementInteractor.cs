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
                var datas = _inputItemRepository.GetListSupplement(inputData.SearchValue, inputData.PageIndex, inputData.PageSize);
                if (datas == null || !(datas?.Total > 0))
                {
                    return new SearchSupplementOutputData(new List<SearchSupplementBaseModel>(), 0, SearchSupplementStatus.NoData);
                }

                return new SearchSupplementOutputData(datas.Model, datas.Total, SearchSupplementStatus.Successed);
            }
            catch
            {
                return new SearchSupplementOutputData(new List<SearchSupplementBaseModel>(), 0, SearchSupplementStatus.Fail);
            }
        }
    }
}
