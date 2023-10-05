using Domain.Models.Online;
using UseCase.Online.SaveOnlineConfirmation;

namespace Interactor.Online
{
    public class SaveOnlineConfirmationInteractor : ISaveOnlineConfirmationInputPort
    {
        private readonly IOnlineRepository _onlineRepository;

        public SaveOnlineConfirmationInteractor(IOnlineRepository onlineRepository)
        {
            _onlineRepository = onlineRepository;
        }

        public SaveOnlineConfirmationOutputData Handle(SaveOnlineConfirmationInputData inputData)
        {
            try
            {
                var result = _onlineRepository.SaveOnlineConfirmation(inputData.UserId, inputData.QualificationInf, inputData.ModelStatus);

                return new SaveOnlineConfirmationOutputData(result ? SaveOnlineConfirmationStatus.Successed : SaveOnlineConfirmationStatus.Failed);
            }
            finally
            {
                _onlineRepository.ReleaseResource();
            }
        }
    }
}
