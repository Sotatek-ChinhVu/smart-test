using Domain.Models.ReceSeikyu;
using UseCase.ReceSeikyu.SearchReceInf;

namespace Interactor.ReceSeikyu
{
    public class SearchReceInfInteractor : ISearchReceInfInputPort
    {
        private readonly IReceSeikyuRepository _receSeikyuRepository;

        public SearchReceInfInteractor(IReceSeikyuRepository receptionRepository)
        {
            _receSeikyuRepository = receptionRepository;
        }

        public SearchReceInfOutputData Handle(SearchReceInfInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new SearchReceInfOutputData(SearchReceInfStatus.InvalidHpId, Enumerable.Empty<RegisterSeikyuModel>());

                if (inputData.PtNum <= 0)
                    return new SearchReceInfOutputData(SearchReceInfStatus.InvalidPtNum, Enumerable.Empty<RegisterSeikyuModel>());

                if (inputData.SinYm <= 0)
                    return new SearchReceInfOutputData(SearchReceInfStatus.InvalidSinYm, Enumerable.Empty<RegisterSeikyuModel>());

                var data = _receSeikyuRepository.SearchReceInf(inputData.HpId, inputData.PtNum, inputData.SinYm);

                if (data.Any())
                    return new SearchReceInfOutputData(SearchReceInfStatus.Successful, data);
                else
                    return new SearchReceInfOutputData(SearchReceInfStatus.NoData, data);
            }
            finally
            {
                _receSeikyuRepository.ReleaseResource();
            }
        }
    }
}
