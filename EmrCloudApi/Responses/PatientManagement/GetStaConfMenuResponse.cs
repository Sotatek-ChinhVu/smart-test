using Domain.Models.MainMenu;

namespace EmrCloudApi.Responses.PatientManagement
{
    public class GetStaConfMenuResponse
    {
        public GetStaConfMenuResponse(List<StatisticMenuModel> staConfMenus, Dictionary<string, string> tenMstItems)
        {
            StaConfMenus = staConfMenus;
            TenMstItems = tenMstItems;
        }

        public List<StatisticMenuModel> StaConfMenus { get; private set; }
        public Dictionary<string, string> TenMstItems { get; private set; }
    }
}
