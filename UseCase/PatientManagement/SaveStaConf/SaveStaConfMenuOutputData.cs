using UseCase.Core.Sync.Core;

namespace UseCase.PatientManagement.SaveStaConf
{
    public class SaveStaConfMenuOutputData : IOutputData
    {
        public SaveStaConfMenuOutputData(SaveStaConfMenuStatus status)
        {
            Status = status;
        }

        public SaveStaConfMenuStatus Status { get; private set; }
    }
}
