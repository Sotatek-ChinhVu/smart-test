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
                if (inputData.HpId <= 0)
                {
                    return new GetLastKaruteOutputData(GetLastKaruteStatus.InvalidHpId, new());
                }
                else if (inputData.PtNum <= 0)
                {
                    return new GetLastKaruteOutputData(GetLastKaruteStatus.InvalidPtNum, new());
                }

                var receptionModel = _receptionRepository.GetLastKarute(inputData.HpId, inputData.PtNum);
                return new GetLastKaruteOutputData(GetLastKaruteStatus.Successed, receptionModel);
            }
            finally
            {
                _receptionRepository.ReleaseResource();
            }
        }
    }
}
