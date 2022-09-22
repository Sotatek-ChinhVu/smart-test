using Domain.Models.Reception;

using UseCase.Core.Sync.Core;

namespace UseCase.ReceptionVisiting.Get
{
    public class GetReceptionVisitingOutputData : IOutputData
    {
        public ReceptionModel ListVisiting { get; private set; }

        public GetReceptionVisitingStatus Status { get; private set; }

        public GetReceptionVisitingOutputData(ReceptionModel listVisiting, GetReceptionVisitingStatus status)
        {
            ListVisiting = listVisiting;
            Status = status;
        }

        public GetReceptionVisitingOutputData(GetReceptionVisitingStatus status)
        {
            ListVisiting = new ReceptionModel();
            Status = status;
        }
    }
}
