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
                return new SearchInputItemOutputData( new List<InputItemModel>(), 0, SearchInputItemStatus.Successed);
            }

            if (inputData.KouiKbn < 0)
            {
                return new SearchInputItemOutputData(new List<InputItemModel>(), 0, SearchInputItemStatus.InvalidKouiKbn);
            }

            if (inputData.SinDate <= 0)
            {
                return new SearchInputItemOutputData(new List<InputItemModel>(), 0, SearchInputItemStatus.InvalidSindate);
            }

            if (inputData.PageIndex <= 0)
            {
                return new SearchInputItemOutputData(new List<InputItemModel>(), 0, SearchInputItemStatus.InvalidPageIndex);
            }

            if (inputData.PageCount <= 0)
            {
                return new SearchInputItemOutputData(new List<InputItemModel>(), 0, SearchInputItemStatus.InvalidPageCount);
            }

            if (inputData.PointFrom < 0)
            {
                return new SearchInputItemOutputData(new List<InputItemModel>(), 0, SearchInputItemStatus.InvalidPointFrom);
            }

            if (inputData.PointTo < 0)
            {
                return new SearchInputItemOutputData(new List<InputItemModel>(), 0, SearchInputItemStatus.InvalidPointTo);
            }

            var data = _inputItemRepository.SearchDataInputItem(inputData.Keyword, inputData.KouiKbn, inputData.SinDate, inputData.PageIndex, inputData.PageCount, inputData.GenericOrSameItem, inputData.YJCd, inputData.HpId, inputData.PointFrom, inputData.PointTo, inputData.IsRosai, inputData.IsMirai, inputData.IsExpired );
            var listTenMst = data.Skip((inputData.PageIndex - 1) * inputData.PageCount).Take(inputData.PageCount);


            return new SearchInputItemOutputData(listTenMst.ToList(), data.Count(), SearchInputItemStatus.Successed);
        }
    }
}
