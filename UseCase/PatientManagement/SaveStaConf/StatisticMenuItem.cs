using Domain.Models.MainMenu;

namespace UseCase.PatientManagement.SaveStaConf
{
    public class StatisticMenuItem
    {
        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public StatisticMenuModel StatisticMenu { get; private set; } 
    }
}
