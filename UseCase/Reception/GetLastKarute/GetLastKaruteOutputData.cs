using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetLastKarute
{
    public class GetLastKaruteOutputData : IOutputData
    {
        public GetLastKaruteOutputData(GetLastKaruteStatus status, ReceptionModel listReceptions)
        {
            Status = status;
            ListReceptions = listReceptions;
        }

        public GetLastKaruteStatus Status { get; private set; }

        public ReceptionModel ListReceptions { get; private set; }
    }
}
