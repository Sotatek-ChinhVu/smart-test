using UseCase.Core.Sync.Core;

namespace UseCase.UserConf.UpdateAdoptedByomeiConfig
{
    public class UpdateAdoptedByomeiConfigOutputData : IOutputData
    {

        public UpdateAdoptedByomeiConfigStatus Status { get; private set; }

        public UpdateAdoptedByomeiConfigOutputData(UpdateAdoptedByomeiConfigStatus status)
        {
            Status = status;
        }
    }
}
