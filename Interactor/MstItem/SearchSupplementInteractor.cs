using Domain.Models.MstItem;
using UseCase.MstItem.SearchSupplement;

namespace Interactor.MstItem
{
    public class SearchSupplementInteractor : ISearchSupplementInputPort
    {
        private readonly IMstItemRepository _inputItemRepository;

        public SearchSupplementInteractor(IMstItemRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }
        public SearchSupplementOutputData Handle(SearchSupplementInputData inputData)
        {
            try
            {
                var supplements = _inputItemRepository.GetListSupplement(inputData.SearchValue);

                var result = new List<SearchSupplementModel>();
                foreach (var supplementModelItem in supplements)
                {
                    var supplementModel = result.Where(u => u.IndexCd == supplementModelItem.IndexCd).FirstOrDefault();
                    if (result.Count == 0 || supplementModel == null)
                    {
                        result.Add(supplementModelItem);
                        result[result.Count - 1].SeibunGroupByIndexCd = supplementModelItem.Seibun;
                    }
                    else if (supplementModel != null)
                    {
                        supplementModel.SeibunGroupByIndexCd = (supplementModel.SeibunGroupByIndexCd + ", " + supplementModelItem.Seibun);
                    }
                }

                if (!result.Any())
                {
                    return new SearchSupplementOutputData(new List<SearchSupplementModel>(), 0, SearchSupplementStatus.NoData);
                }
                var total = result.Count();
                result = result.AsEnumerable()
                               .Skip((inputData.PageIndex - 1) * inputData.PageSize)
                               .Take(inputData.PageSize).ToList();
                return new SearchSupplementOutputData(result, total, SearchSupplementStatus.Successed);
            }
            catch
            {
                return new SearchSupplementOutputData(new List<SearchSupplementModel>(), 0, SearchSupplementStatus.Fail);
            }
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}
