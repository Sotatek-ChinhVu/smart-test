using Domain.Models.MstItem;
using UseCase.MstItem.SearchTenMstItem;

namespace Interactor.MstItem
{
    public class SearchTenMstItemInteractor : ISearchTenMstItemInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public SearchTenMstItemInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public SearchTenMstItemOutputData Handle(SearchTenMstItemInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new SearchTenMstItemOutputData(new List<TenItemModel>(), 0, SearchTenMstItemStatus.Successed);
            }

            if (inputData.STDDate < 0)
            {
                return new SearchTenMstItemOutputData(new List<TenItemModel>(), 0, SearchTenMstItemStatus.InvalidSindate);
            }

            if (inputData.PageIndex <= 0)
            {
                return new SearchTenMstItemOutputData(new List<TenItemModel>(), 0, SearchTenMstItemStatus.InvalidPageIndex);
            }

            if (inputData.PageCount < 0)
            {
                return new SearchTenMstItemOutputData(new List<TenItemModel>(), 0, SearchTenMstItemStatus.InvalidPageCount);
            }

            if (inputData.Keyword is null)
            {
                return new SearchTenMstItemOutputData(new List<TenItemModel>(), 0, SearchTenMstItemStatus.InValidKeyword);
            }

            try
            {
                var data = _mstItemRepository.SearchTenMst(
                    inputData.HpId, inputData.PageIndex, inputData.PageCount, inputData.Keyword, inputData.PointFrom,
                    inputData.PointTo, inputData.KouiKbn, inputData.OriKouiKbn,
                    inputData.KouiKbns, inputData.IncludeRosai, inputData.IncludeMisai,
                    inputData.STDDate, inputData.ItemCodeStartWith, inputData.IsIncludeUsage,
                    inputData.OnlyUsage, inputData.YJCode, inputData.IsMasterSearch,
                    inputData.IsExpiredSearchIfNoData, inputData.IsAllowSearchDeletedItem,
                    inputData.IsExpired, inputData.IsDeleted, inputData.DrugKbns,
                    inputData.IsSearchSanteiItem, inputData.IsSearchKenSaItem,
                    inputData.ItemFilter, inputData.IsSearch831SuffixOnly, inputData.IsSearchSuggestion);

                return new SearchTenMstItemOutputData(data.tenItemModels, data.totalCount, SearchTenMstItemStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
