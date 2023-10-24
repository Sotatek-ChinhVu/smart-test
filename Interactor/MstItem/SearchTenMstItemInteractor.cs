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
                var result = (new List<TenItemModel>(), 0);
                if (!inputData.IsSearchSuggestion)
                {
                    result = _mstItemRepository.SearchTenMasterItem(
                    inputData.HpId, inputData.PageIndex, inputData.PageCount, inputData.Keyword, inputData.PointFrom,
                    inputData.PointTo, inputData.KouiKbn, inputData.OriKouiKbn,
                    inputData.KouiKbns, inputData.IncludeRosai, inputData.IncludeMisai,
                    inputData.STDDate, inputData.ItemCodeStartWith, inputData.IsIncludeUsage,
                    inputData.OnlyUsage, inputData.YJCode, inputData.IsMasterSearch,
                    inputData.IsExpiredSearchIfNoData, inputData.IsAllowSearchDeletedItem,
                    inputData.IsExpired, inputData.IsDeleted, inputData.DrugKbns,
                    inputData.IsSearchSanteiItem, inputData.IsSearchKenSaItem,
                    inputData.ItemFilter, inputData.IsSearch831SuffixOnly, inputData.IsSearchGazoDensibaitaiHozon,
                    inputData.SortType, inputData.SortCol);
                }
                else
                {
                    result = _mstItemRepository.SearchSuggestionTenMstItem(
                    inputData.HpId, inputData.PageIndex, inputData.PageCount, inputData.Keyword,
                    inputData.KouiKbn, inputData.OriKouiKbn, inputData.KouiKbns,
                    inputData.IncludeMisai, inputData.IncludeRosai, inputData.STDDate, inputData.ItemCodeStartWith,
                    inputData.IsIncludeUsage, inputData.IsDeleted, inputData.DrugKbns,
                    inputData.ItemFilter, inputData.IsSearch831SuffixOnly);
                }

                return new SearchTenMstItemOutputData(result.Item1, result.Item2, SearchTenMstItemStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
