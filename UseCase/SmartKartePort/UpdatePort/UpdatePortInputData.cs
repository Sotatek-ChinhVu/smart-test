using Domain.Models.SmartKartePort;
using UseCase.Core.Sync.Core;

namespace UseCase.SmartKartePort.UpdatePort
{
    public sealed class UpdatePortInputData : IInputData<UpdatePortOutputData>
    {
        public UpdatePortInputData(int userId, SmartKarteAppSignalRPortModel signalRPortModel)
        {
            UserId = userId;
            SignalRPortModel = signalRPortModel;
        }
        public int UserId { get; private set; }
        public SmartKarteAppSignalRPortModel SignalRPortModel { get; private set;}
    }
}
