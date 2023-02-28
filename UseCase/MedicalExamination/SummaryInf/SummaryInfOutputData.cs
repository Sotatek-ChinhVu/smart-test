using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.SummaryInf
{
    public class SummaryInfOutputData : IOutputData
    {
        public SummaryInfOutputData(List<SummaryInfItem> header1Infos, List<SummaryInfItem> header2Infos, List<SummaryInfItem> notifications, List<PopUpNotificationItem> notificationPopUps, SummaryInfStatus status)
        {
            Header1Infos = header1Infos;
            Header2Infos = header2Infos;
            Notifications = notifications;
            NotificationPopUps = notificationPopUps;
            Status = status;
        }

        public List<SummaryInfItem> Header1Infos { get; private set; }
        public List<SummaryInfItem> Header2Infos { get; private set; }
        public List<SummaryInfItem> Notifications { get; private set; }
        public List<PopUpNotificationItem> NotificationPopUps { get; private set; }

        public SummaryInfStatus Status { get; private set; }
    }
}
