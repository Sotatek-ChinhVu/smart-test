using UseCase.MedicalExamination.SummaryInf;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class SummaryInfResponse
    {
        public SummaryInfResponse(List<SummaryInfItem> header1Infos, List<SummaryInfItem> header2Infos, List<SummaryInfItem> notifications, List<PopUpNotificationItem> notificationPopUps)
        {
            Header1Infos = header1Infos;
            Header2Infos = header2Infos;
            Notifications = notifications;
            NotificationPopUps = notificationPopUps;
        }

        public List<SummaryInfItem> Header1Infos { get; private set; }
        public List<SummaryInfItem> Header2Infos { get; private set; }
        public List<SummaryInfItem> Notifications { get; private set; }
        public List<PopUpNotificationItem> NotificationPopUps { get; private set; }
    }
}
