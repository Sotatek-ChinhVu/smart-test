using Domain.Models.MstItem;
using UseCase.MstItem.GetDosageDrugList;
using UseCase.MstItem.SearchOTC;
using static Domain.Models.MstItem.SearchOTCModel;

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
                var datas = _inputItemRepository.SearchOTCModels(inputData.SearchValue, inputData.PageIndex, inputData.PageSize);
                if (datas?.Model == null || !(datas?.Total > 0))
                {
                    return new SearchOTCOutputData(new List<SearchOTCBaseModel>(), 0, SearchOTCStatus.NoData);
                }

                return new SearchOTCOutputData(datas.Model, datas.Total, SearchOTCStatus.Successed);
            }
            catch
            {
                return new SearchOTCOutputData(new List<SearchOTCBaseModel>(), 0, SearchOTCStatus.Fail);
            }
        }
    }
}
