using Domain.Models.MainMenu;
using UseCase.PatientManagement.SaveStaConf;

namespace Interactor.PatientManagement
{
    public class SaveStaConfMenuInteractor : ISaveStaConfMenuInputPort
    {
        private readonly IStatisticRepository _statisticRepository;

        public SaveStaConfMenuInteractor(IStatisticRepository statisticRepository)
        {
            _statisticRepository = statisticRepository;
        }

        public SaveStaConfMenuOutputData Handle(SaveStaConfMenuInputData inputData)
        {
            try
            {
                var result = _statisticRepository.SaveStaConfMenu(inputData.HpId, inputData.UserId, inputData.StatisticMenu);

                return new SaveStaConfMenuOutputData(result ? SaveStaConfMenuStatus.Successed : SaveStaConfMenuStatus.Failed);
            }
            finally
            {
                _statisticRepository.ReleaseResource();
            }

        }
    }
}
