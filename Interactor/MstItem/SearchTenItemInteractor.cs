using Domain.Models.MstItem;
using UseCase.MstItem.SearchTenItem;

namespace Interactor.MstItem
{
    public class SearchTenItemInteractor : ISearchTenItemInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public SearchTenItemInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public SearchTenItemOutputData Handle(SearchTenItemInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.Successed);
            }

            if (inputData.ItemCondition.STDDate <= 0)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.InvalidSindate);
            }

            if (inputData.PageIndex <= 0)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.InvalidPageIndex);
            }

            if (inputData.PageCount < 0)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.InvalidPageCount);
            }

            if (inputData.ItemCondition.Keyword is null)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.InValidKeyword);
            }

            try
            {
                var data = _mstItemRepository.SearchTenMst(inputData.HpId, inputData.PageIndex, inputData.PageCount, inputData.ItemCondition.Keyword, inputData.ItemCondition.PointFrom,
                    inputData.ItemCondition.PointTo, inputData.ItemCondition.KouiKbn, inputData.ItemCondition.OriKouiKbn,
                    inputData.ItemCondition.KouiKbns, inputData.ItemCondition.IncludeRosai, inputData.ItemCondition.IncludeMisai,
                    inputData.ItemCondition.STDDate, inputData.ItemCondition.ItemCodeStartWith, inputData.ItemCondition.IsIncludeUsage,
                    inputData.ItemCondition.OnlyUsage, inputData.ItemCondition.YJCode, inputData.ItemCondition.IsMasterSearch,
                    inputData.ItemCondition.IsExpiredSearchIfNoData, inputData.ItemCondition.IsAllowSearchDeletedItem,
                    inputData.ItemCondition.IsExpired, inputData.ItemCondition.IsDeleted, inputData.ItemCondition.DrugKbns,
                    inputData.ItemCondition.IsSearchSanteiItem, inputData.ItemCondition.IsSearchKenSaItem,
                    inputData.ItemCondition.ItemFilter, inputData.ItemCondition.IsSearch831SuffixOnly);

                return new SearchTenItemOutputData(data.tenItemModels, data.totalCount, SearchTenItemStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
