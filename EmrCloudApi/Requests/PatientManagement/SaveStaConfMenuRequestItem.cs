using Domain.Models.MainMenu;

namespace EmrCloudApi.Requests.PatientManagement
{
    public class SaveStaConfMenuRequestItem
    {
        public SaveStaConfMenuRequestItem(StatisticMenuModel staConfMenu)
        {
            StaConfMenu = staConfMenu;
        }
        public SaveStaConfMenuRequestItem()
        {

        }

        public StatisticMenuModel StaConfMenu { get; private set; } = new();
    }
}
