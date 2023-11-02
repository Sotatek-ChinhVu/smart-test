using UseCase.Core.Sync.Core;

namespace UseCase.SmartKartePort.GetPort
{
    public sealed class GetPortInputData : IInputData<GetPortOutputData>
    {
        public GetPortInputData(string machineName, string ip) 
        {
            MachineName = machineName;
            Ip = ip;
        }
        public string MachineName {  get; private set; }
        public string Ip {  get; private set; }
    }
}
