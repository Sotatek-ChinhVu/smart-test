using Domain.Models.MainMenu;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientManagement.SaveStaConf
{
    public class SaveStaConfMenuInputData : IInputData<SaveStaConfMenuOutputData>
    {
        public SaveStaConfMenuInputData(StaConfModel staConf)
        {
            StaConf = staConf;
        }

        public StaConfModel StaConf { get; set; }
    }
}
