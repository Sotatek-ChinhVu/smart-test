using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.RevertDeleteNoRecept
{
    public class RevertDeleteNoReceptOutputData : IOutputData
    {
        public RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus status, List<ReceptionRowModel> receptionModel)
        {
            Status = status;
            this.receptionModel = receptionModel;
        }

        public RevertDeleteNoReceptStatus Status { get; private set; }

        public List<ReceptionRowModel> receptionModel { get; private set; }
    }
}
