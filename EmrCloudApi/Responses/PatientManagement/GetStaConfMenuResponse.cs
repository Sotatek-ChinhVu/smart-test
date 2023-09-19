using Domain.Models.MainMenu;

namespace EmrCloudApi.Responses.PatientManagement
{
    public class GetStaConfMenuResponse
    {
        public GetStaConfMenuResponse(List<StatisticMenuModel> staConfMenus)
        {
            StaConfMenus = staConfMenus;
        }

        public List<StatisticMenuModel> StaConfMenus { get; private set; }
    }
}
