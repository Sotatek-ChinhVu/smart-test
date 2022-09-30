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

                if (inputData.PostCode1.Length > 3)
                    return new SearchPostCodeOutputData(new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidPostCode);

                if (inputData.PostCode2.Length > 4)
                    return new SearchPostCodeOutputData(new List<PostCodeMstModel>(), SearchPostCodeStatus.InvalidPostCode);

                string address = CIUtil.ToHalfsize(inputData.Address);
                var listPostCode = _postCodeMstRepository.PostCodeMstModels(inputData.HpId, inputData.PostCode1, inputData.PostCode2, address);
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
