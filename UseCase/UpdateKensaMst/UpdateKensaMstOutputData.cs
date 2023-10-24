using UseCase.Core.Sync.Core;

namespace UseCase.UpdateKensaMst
{
    public class UpdateKensaMstOutputData : IOutputData
    {
        public UpdateKensaMstOutputData(UpdateKensaMstStatus status)
        {
            Status = status;
        }

        public UpdateKensaMstStatus Status { get; private set; }
    }
}
