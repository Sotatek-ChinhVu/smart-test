using Domain.Models.InputItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.InputItem.Search;

namespace Interactor.InputItem
{
    public class SearchInputItemInteractor : ISearchInputItemInputPort
    {
        private readonly IInputItemRepository _inputItemRepository;
        public SearchInputItemInteractor(IInputItemRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }

        public SearchInputItemOutputData Handle(SearchInputItemInputData inputData)
        {
            if(inputData.HpId < 0)
            {
                return new SearchInputItemOutputData( new List<InputItemModel>(), SearchInputItemStatus.Successed);
            }

            if (inputData.KouiKbn < 0)
            {
                return new SearchInputItemOutputData(new List<InputItemModel>(), SearchInputItemStatus.InvalidKouiKbn);
            }

            if (inputData.SinDate < 0)
            {
                return new SearchInputItemOutputData(new List<InputItemModel>(), SearchInputItemStatus.InvalidSindate);
            }

            if (inputData.IsSearchInline)
            {
                // search list inline
                if (!String.IsNullOrEmpty(inputData.Keyword))
                {
                    return new SearchInputItemOutputData(new List<InputItemModel>(), SearchInputItemStatus.InValidKeyword);
                }

                if (inputData.StartIndex < 0)
                {
                    return new SearchInputItemOutputData(new List<InputItemModel>(), SearchInputItemStatus.InvalidStartIndex);
                }

                if (inputData.PageCount <= 0)
                {
                    return new SearchInputItemOutputData(new List<InputItemModel>(), SearchInputItemStatus.InvalidPageCount);
                }
            }

            var data = _inputItemRepository.SearchDataInputItem(inputData.Keyword, inputData.KouiKbn, inputData.SinDate, inputData.StartIndex, inputData.PageCount, inputData.IsSearchInline, inputData.YJCd, inputData.HpId, inputData.PointFrom, inputData.PointTo, inputData.IsRosai, inputData.IsMirai, inputData.IsExpired );

            return new SearchInputItemOutputData(data.ToList(), SearchInputItemStatus.Successed);
        }
    }
}
