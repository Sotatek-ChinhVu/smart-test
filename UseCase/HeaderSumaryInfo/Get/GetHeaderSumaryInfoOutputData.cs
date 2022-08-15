using UseCase.Core.Sync.Core;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class GetHeaderSumaryInfoOutputData : IOutputData
    {
        public GetHeaderSumaryInfoOutputData(List<PtInfNotificationItem>? header1Info, List<PtInfNotificationItem>? header2Info, List<PtInfNotificationItem>? notification, GetHeaderSumaryInfoStatus status)
        {
            Header1Info = header1Info;
            Header2Info = header2Info;
            Notification = notification;
            Status = status;
        }

        public List<PtInfNotificationItem>? Header1Info { get; private set; }
        public List<PtInfNotificationItem>? Header2Info { get; private set; }
        public List<PtInfNotificationItem>? Notification { get; private set; }
        public GetHeaderSumaryInfoStatus Status { get; private set; }
    }
}
