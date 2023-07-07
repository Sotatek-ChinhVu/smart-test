using Domain.Models.Reception;
using UseCase.Reception.GetLastKarute;

namespace Interactor.Reception
{
    public class GetLastKaruteInteractor : IGetLastKaruteInputPort
    {
        private readonly IReceptionRepository _receptionRepository;

        public GetLastKaruteInteractor(IReceptionRepository receptionRepository)
        {
            _receptionRepository = receptionRepository;
        }

        public GetLastKaruteOutputData Handle(GetLastKaruteInputData inputData)
        {
            try
            {
                if (inputData.PtNum < 0)
                {
                    return new GetLastKaruteOutputData(GetLastKaruteStatus.InvalidPtNum, new());
                }

                var reception = _receptionRepository.GetLastKarute(inputData.HpId, inputData.PtNum);

                if (reception == null)
                {
                    return new GetLastKaruteOutputData(GetLastKaruteStatus.NoData, new());
                }

                return new GetLastKaruteOutputData(GetLastKaruteStatus.Successed, reception ?? new());
            }
            finally
            {
                _receptionRepository.ReleaseResource();
            }
        }
    }
}
