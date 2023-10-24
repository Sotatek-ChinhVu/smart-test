using Domain.Models.MainMenu;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientManagement.SaveStaConf
{
    public class SaveStaConfMenuInputData : IInputData<SaveStaConfMenuOutputData>
    {
        public SaveStaConfMenuInputData(int hpId, int userId, StatisticMenuModel statisticMenu)
        {
            HpId = hpId;
            UserId = userId;
            StatisticMenu = statisticMenu;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public StatisticMenuModel StatisticMenu { get; private set; }
    }
}
