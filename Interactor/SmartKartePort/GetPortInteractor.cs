using Domain.Models.SmartKartePort;
using UseCase.SmartKartePort.GetPort;

namespace Interactor.SmartKartePort
{
    public class GetPortInteractor : IGetPortInputPort
    {
        private readonly ISmartKartePortRepository _smartKartePortRepository;
        public GetPortInteractor(ISmartKartePortRepository smartKartePortRepository)
        {
            _smartKartePortRepository = smartKartePortRepository;
        }

        public GetPortOutputData Handle(GetPortInputData input)
        {
            try
            {
                var data = _smartKartePortRepository.GetSignalPort(input.MachineName, input.Ip);
                if (data != null)
                {
                    return new GetPortOutputData(data, GetPortStatus.Success);
                }
                else
                {
                    return new GetPortOutputData(new SmartKarteAppSignalRPortModel(), GetPortStatus.Nodata);
                }
            }
            catch
            {
                return new GetPortOutputData(new SmartKarteAppSignalRPortModel(), GetPortStatus.Faild);
            }
            finally
            {
                _smartKartePortRepository.ReleaseResource();
            }
        }
    }
}
