using Domain.Models.MstItem;
using Helper.Common;
using UseCase.MstItem.SearchPostCode;

namespace Interactor.MstItem
{
    public class SearchPostCodeInteractor : ISearchPostCodeInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public SearchPostCodeInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public SearchPostCodeOutputData Handle(SearchPostCodeInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new SearchPostCodeOutputData(0, new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidHpId);

                if (inputData.PageIndex < 0)
                    return new SearchPostCodeOutputData(0, new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidPageIndex);

                if (inputData.PageSize < 0)
                    return new SearchPostCodeOutputData(0, new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidPageSize);

                string postcode1 = CIUtil.ToHalfsize(inputData.PostCode1);
                if (postcode1.Length > 3)
                    return new SearchPostCodeOutputData(0, new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidPostCode);

                string postcode2 = CIUtil.ToHalfsize(inputData.PostCode2);
                if (postcode2.Length > 4)
                    return new SearchPostCodeOutputData(0, new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidPostCode);

                string address = CIUtil.ToHalfsize(inputData.Address);

                var listPostCode = _mstItemRepository.PostCodeMstModels(inputData.HpId, postcode1, postcode2, address, inputData.PageIndex, inputData.PageSize);

                if (listPostCode.Item1 == 0)
                    return new SearchPostCodeOutputData(0, new List<PostCodeMstModel>(), SearchPostCodeStatus.NoData);

                return new SearchPostCodeOutputData(listPostCode.Item1, listPostCode.Item2, SearchPostCodeStatus.Success);
            }
            catch (Exception)
            {
                return new SearchPostCodeOutputData(0, new List<PostCodeMstModel>(), SearchPostCodeStatus.Failed);
            }
        }
    }
}
