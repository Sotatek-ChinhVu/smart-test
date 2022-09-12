using Domain.Models.MstItem;
using UseCase.MstItem.GetDosageDrugList;
using UseCase.MstItem.SearchOTC;

namespace Interactor.MstItem
{
    public class SearchOTCInteractor : ISearchOTCInputPort
    {
        private readonly IMstItemRepository _inputItemRepository;

        public SearchOTCInteractor(IMstItemRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }

        public SearchOTCOutputData Handle(SearchOTCInputData inputData)
        {
            try
            {
                var datas = _inputItemRepository.SearchOTCModels(inputData.SearchValue,inputData.PageIndex,inputData.PageSize);
                if (!(datas?.Count > 0))
                {
                    return new SearchOTCOutputData(new List<SearchOTCModel>(), SearchOTCStatus.NoData);
                }

                return new SearchOTCOutputData(datas, SearchOTCStatus.Successed);
            }
            catch
            {
                return new SearchOTCOutputData(new List<SearchOTCModel>(), SearchOTCStatus.Fail);
            }
        }
    }
}
