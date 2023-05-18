using Domain.Models.MstItem;
using UseCase.MstItem.SearchTenMstItemSpecialNote;

namespace Interactor.MstItem
{
    public class SearchTenMstSpecialNoteInteractor : ISearchTenMstItemSpecialNoteInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public SearchTenMstSpecialNoteInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public SearchTenMstItemSpecialNoteOutputData Handle(SearchTenMstItemSpecialNoteInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new SearchTenMstItemSpecialNoteOutputData(new List<TenItemModel>(), 0, SearchTenMstItemSpecialNoteStatus.Successed);
            }

            if (inputData.ItemCondition.STDDate <= 0)
            {
                return new SearchTenMstItemSpecialNoteOutputData(new List<TenItemModel>(), 0, SearchTenMstItemSpecialNoteStatus.InvalidSindate);
            }

            if (inputData.PageIndex <= 0)
            {
                return new SearchTenMstItemSpecialNoteOutputData(new List<TenItemModel>(), 0, SearchTenMstItemSpecialNoteStatus.InvalidPageIndex);
            }

            if (inputData.PageCount < 0)
            {
                return new SearchTenMstItemSpecialNoteOutputData(new List<TenItemModel>(), 0, SearchTenMstItemSpecialNoteStatus.InvalidPageCount);
            }

            if (inputData.ItemCondition.Keyword is null)
            {
                return new SearchTenMstItemSpecialNoteOutputData(new List<TenItemModel>(), 0, SearchTenMstItemSpecialNoteStatus.InValidKeyword);
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

                return new SearchTenMstItemSpecialNoteOutputData(data.tenItemModels, data.totalCount, SearchTenMstItemSpecialNoteStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
