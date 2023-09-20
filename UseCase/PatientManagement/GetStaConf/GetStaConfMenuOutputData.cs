using Domain.Models.MainMenu;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientManagement.GetStaConf
{
    public class GetStaConfMenuOutputData : IOutputData
    {
        public GetStaConfMenuOutputData(List<StatisticMenuModel> statisticMenus, Dictionary<string, string> tenMstItems, GetStaConfMenuStatus status)
        {
            StatisticMenus = statisticMenus;
            TenMstItems = tenMstItems;
            Status = status;
        }

        public List<StatisticMenuModel> StatisticMenus { get; private set; }
        public Dictionary<string, string> TenMstItems { get; private set; }
        public GetStaConfMenuStatus Status { get; private set; }
    }
}
