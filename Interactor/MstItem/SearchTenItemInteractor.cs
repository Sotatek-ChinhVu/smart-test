﻿using Domain.Models.MstItem;
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

            if (inputData.PageCount < 0)
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

            if (inputData.Keyword is null)
            {
                return new SearchTenItemOutputData(new List<TenItemModel>(), 0, SearchTenItemStatus.InValidKeyword);
            }

            try
            {
                var data = _mstItemRepository.SearchTenMst(inputData.Keyword, inputData.KouiKbn, inputData.SinDate, inputData.PageIndex, inputData.PageCount, inputData.GenericOrSameItem, inputData.YJCd, inputData.HpId, inputData.PointFrom, inputData.PointTo, inputData.IsRosai, inputData.IsMirai, inputData.IsExpired, inputData.ItemCodeStartWith, inputData.IsMasterSearch, inputData.IsSearch831SuffixOnly, inputData.IsSearchSanteiItem, inputData.SearchFollowUsage, inputData.KouiKbns, inputData.MasterSBT);

                return new SearchTenItemOutputData(data.tenItemModels, data.totalCount, SearchTenItemStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
