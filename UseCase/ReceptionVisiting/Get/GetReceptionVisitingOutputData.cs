using Domain.Models.Reception;

using UseCase.Core.Sync.Core;

namespace UseCase.ReceptionVisiting.Get
{
    public class GetReceptionVisitingOutputData : IOutputData
    {
        public List<ReceptionModel> ListVisiting { get; private set; }

        public GetReceptionVisitingStatus Status { get; private set; }

        public GetReceptionVisitingOutputData(List<ReceptionModel> listVisiting, GetReceptionVisitingStatus status)
        {
            ListVisiting = listVisiting;
            Status = status;
        }
    }
}
