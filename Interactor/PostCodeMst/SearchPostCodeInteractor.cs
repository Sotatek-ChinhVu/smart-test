using Domain.Models.PostCodeMst;
using Helper.Common;
using UseCase.PostCodeMst.Search;

namespace Interactor.PostCodeMst
{
    public class SearchPostCodeInteractor : ISearchPostCodeInputPort
    {
        private readonly IPostCodeMstRepository _postCodeMstRepository;

        public SearchPostCodeInteractor(IPostCodeMstRepository postCodeMstRepository)
        {
            _postCodeMstRepository = postCodeMstRepository;
        }

        public SearchPostCodeOutputData Handle(SearchPostCodeInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new SearchPostCodeOutputData(new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidHpId);

                if (inputData.PageIndex < 0)
                    return new SearchPostCodeOutputData(new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidPageIndex);

                if (inputData.PageCount < 0)
                    return new SearchPostCodeOutputData(new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidPageCount);

                string postcode1 = CIUtil.ToHalfsize(inputData.PostCode1);
                if (postcode1.Length > 3)
                    return new SearchPostCodeOutputData(new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidPostCode);

                string postcode2 = CIUtil.ToHalfsize(inputData.PostCode2);
                if (postcode2.Length > 4)
                    return new SearchPostCodeOutputData(new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidPostCode);

                string address = CIUtil.ToHalfsize(inputData.Address);

                var listPostCode = _postCodeMstRepository.PostCodeMstModels(inputData.HpId, postcode1, postcode2, address, inputData.PageIndex, inputData.PageCount);
                if (!listPostCode.Any())
                    return new SearchPostCodeOutputData(new List<PostCodeMstModel>(), SearchPostCodeStatus.NoData);

                return new SearchPostCodeOutputData(listPostCode, SearchPostCodeStatus.Success);
            }
            catch (Exception)
            {
                return new SearchPostCodeOutputData(new List<PostCodeMstModel>(), SearchPostCodeStatus.Failed);
            }
        }
    }
}
