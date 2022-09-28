using Domain.Models.PostCodeMst;
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
                var listPostCode = _postCodeMstRepository.PostCodeMstModels(inputData.PostCode1, inputData.PostCode2, inputData.Address);

                if (listPostCode == null || listPostCode.Count == 0)
                    return new SearchPostCodeOutputData(new(), SearchPostCodeStatus.NoData);

                return new SearchPostCodeOutputData(listPostCode, SearchPostCodeStatus.Success);
            }
            catch (Exception)
            {
                return new SearchPostCodeOutputData(new List<PostCodeMstModel>(), SearchPostCodeStatus.Failed);
            }
        }
    }
}
