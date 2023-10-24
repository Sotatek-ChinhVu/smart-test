using Domain.Models.MainMenu;

namespace EmrCloudApi.Responses.PatientManagement
{
    public class GetStaConfMenuResponse
    {
        public GetStaConfMenuResponse(List<StatisticMenuModel> staConfMenus, Dictionary<string, string> tenMstItems, Dictionary<string, string> byomeis)
        {
            StaConfMenus = staConfMenus;
            TenMstItems = tenMstItems;
            Byomeis = byomeis;
        }

        public List<StatisticMenuModel> StaConfMenus { get; private set; }
        public Dictionary<string, string> TenMstItems { get; private set; }
        public Dictionary<string, string> Byomeis { get; private set; }
    }
}
