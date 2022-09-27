using Domain.Models.Reception;

using UseCase.Core.Sync.Core;

namespace UseCase.ReceptionVisiting.Get
{
    public class GetReceptionVisitingOutputData : IOutputData
    {
        public ReceptionModel ReceptionModel { get; private set; }

        public GetReceptionVisitingStatus Status { get; private set; }

        public GetReceptionVisitingOutputData(ReceptionModel receptionModel, GetReceptionVisitingStatus status)
        {
            ReceptionModel = receptionModel;
            Status = status;
        }

        public GetReceptionVisitingOutputData(GetReceptionVisitingStatus status)
        {
            ReceptionModel = new ReceptionModel();
            Status = status;
        }
    }
}
