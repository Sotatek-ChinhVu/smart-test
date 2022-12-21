using Domain.Models.MstItem;
using Infrastructure.Repositories;
using UseCase.MstItem.SearchOTC;

namespace Interactor.MstItem
{
    public class SearchOtcInteractor : ISearchOTCInputPort
    {
        private readonly IMstItemRepository _inputItemRepository;

        public SearchOtcInteractor(IMstItemRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }

        public SearchOTCOutputData Handle(SearchOTCInputData inputData)
        {
            try
            {
                (List<OtcItemModel> otcItemList, int total) = _inputItemRepository.SearchOTCModels(inputData.SearchValue, inputData.PageIndex, inputData.PageSize);

                if (otcItemList == null || total <= 0)
                {
                    return new SearchOTCOutputData(new List<OtcItemModel>(), 0, SearchOTCStatus.NoData);
                }

                return new SearchOTCOutputData(otcItemList, total, SearchOTCStatus.Successed);
            }
            catch
            {
                return new SearchOTCOutputData(new List<OtcItemModel>(), 0, SearchOTCStatus.Fail);
            }
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}
