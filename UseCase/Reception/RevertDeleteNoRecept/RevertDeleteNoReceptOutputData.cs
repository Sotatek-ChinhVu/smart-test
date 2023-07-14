using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.RevertDeleteNoRecept
{
    public class RevertDeleteNoReceptOutputData : IOutputData
    {
        public RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus status, ReceptionModel receptionModel)
        {
            Status = status;
            this.receptionModel = receptionModel;
        }

        public RevertDeleteNoReceptStatus Status { get; private set; }

        public ReceptionModel receptionModel { get; private set; }
    }
}
