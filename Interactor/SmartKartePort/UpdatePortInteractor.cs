using Domain.Models.SmartKartePort;
using UseCase.SmartKartePort.UpdatePort;

namespace Interactor.SmartKartePort
{
    public class UpdatePortInteractor : IUpdatePortInputPort
    {
        private readonly ISmartKartePortRepository _smartKartePortRepository;
        public UpdatePortInteractor(ISmartKartePortRepository smartKartePortRepository)
        {
            _smartKartePortRepository = smartKartePortRepository;
        }

        public UpdatePortOutputData Handle(UpdatePortInputData input)
        {
            try
            {
                var update = _smartKartePortRepository.UpdateSignalRPort(input.UserId, input.SignalRPortModel);
                return new UpdatePortOutputData(UpdatePortStatus.Success);
            }
            catch
            {
                return new UpdatePortOutputData(UpdatePortStatus.Faild);
            }
            finally
            {
                _smartKartePortRepository.ReleaseResource();
            }
        }
    }
}
