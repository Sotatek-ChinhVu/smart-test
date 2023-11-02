using Domain.Models.SmartKartePort;
using UseCase.Core.Sync.Core;

namespace UseCase.SmartKartePort.GetPort
{
    public sealed class GetPortOutputData : IOutputData
    {
        public GetPortOutputData(SmartKarteAppSignalRPortModel signalRPort, GetPortStatus status)
        {
            SignalRPort = signalRPort;
            Status = status;
        }
        public SmartKarteAppSignalRPortModel SignalRPort { get; private set; }
        public GetPortStatus Status { get; private set; }
    }
}
