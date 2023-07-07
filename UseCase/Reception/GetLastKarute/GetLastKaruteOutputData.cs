using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetLastKarute
{
    public class GetLastKaruteOutputData : IOutputData
    {
        public GetLastKaruteOutputData(GetLastKaruteStatus status, ReceptionModel reception)
        {
            Status = status;
            Reception = reception;
        }

        public GetLastKaruteStatus Status { get; private set; }

        public ReceptionModel Reception { get; private set; }
    }
}
