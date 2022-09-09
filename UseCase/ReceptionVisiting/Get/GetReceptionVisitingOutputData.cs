using Domain.Models.ReceptionVisitingModel;

using UseCase.Core.Sync.Core;

namespace UseCase.ReceptionVisiting.Get
{
    public class GetReceptionVisitingOutputData : IOutputData
    {
        public List<ReceptionVisitingModel> ListVisiting { get; private set; }

        public GetReceptionVisitingStatus Status { get; private set; }

        public GetReceptionVisitingOutputData(List<ReceptionVisitingModel> listVisiting, GetReceptionVisitingStatus status)
        {
            ListVisiting = listVisiting;
            Status = status;
        }
    }
}
