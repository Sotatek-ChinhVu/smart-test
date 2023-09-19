using Domain.Models.MainMenu;
using UseCase.PatientManagement.GetStaConf;

namespace Interactor.PatientManagement
{
    public class GetStaConfMenuInteractor : IGetStaConfMenuInputPort
    {
        private readonly IStatisticRepository _statisticRepository;

        public GetStaConfMenuInteractor(IStatisticRepository statisticRepository)
        {
            _statisticRepository = statisticRepository;
        }

        public GetStaConfMenuOutputData Handle(GetStaConfMenuInputData inputData)
        {
            try
            {
                var staMenu = _statisticRepository.GetStatisticMenuModels(inputData.HpId);

                return new GetStaConfMenuOutputData(staMenu, staMenu.Any() ? GetStaConfMenuStatus.Successed : GetStaConfMenuStatus.NoData);
            }
            finally
            {
                _statisticRepository.ReleaseResource();
            }
        }
    }
}
