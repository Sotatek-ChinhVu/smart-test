using Domain.Models.MstItem;
using Helper.Common;
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

            if (inputData.KouiKbn < 0)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.InvalidKouiKbn);
            }

            if (inputData.SinDate <= 0)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.InvalidSindate);
            }

            if (inputData.PageIndex <= 0)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.InvalidPageIndex);
            }

            if (inputData.PageCount <= 0)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.InvalidPageCount);
            }

            if (inputData.PointFrom < 0)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.InvalidPointFrom);
            }

            if (inputData.PointTo < 0)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.InvalidPointTo);
            }

            var data = _mstItemRepository.SearchTenMst(inputData.Keyword, inputData.KouiKbn, inputData.SinDate, inputData.PageIndex, inputData.PageCount, inputData.GenericOrSameItem, inputData.YJCd, inputData.HpId, inputData.PointFrom, inputData.PointTo, inputData.IsRosai, inputData.IsMirai, inputData.IsExpired, inputData.ItemCodeStartWith);

            return new SearchTenItemOutputData(data.Item1, data.Item2, SearchTenItemStatus.Successed);
        }
    }
}
