using Domain.Models.SmartKartePort;

namespace EmrCloudApi.Responses.SmartKartePort
{
    public class GetPortResponse
    {
        public GetPortResponse(SmartKarteAppSignalRPortModel smartKarteAppSignalRPort)
        {
            SmartKarteAppSignalRPort = smartKarteAppSignalRPort;
        }

        public SmartKarteAppSignalRPortModel SmartKarteAppSignalRPort { get; private set; }
    }
}
