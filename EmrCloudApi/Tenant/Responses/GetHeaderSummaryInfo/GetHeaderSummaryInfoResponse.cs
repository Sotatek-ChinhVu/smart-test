using UseCase.HeaderSumaryInfo.Get;
using UseCase.OrdInfs.GetListTrees;

namespace EmrCloudApi.Tenant.Responses.GetHeaderSummaryInfo
{
    public class GetHeaderSummaryInfoResponse
    {
        public GetHeaderSummaryInfoResponse(List<PtInfNotificationItem>? header1Info, List<PtInfNotificationItem>? header2Info, List<PtInfNotificationItem>? notification)
        {
            Header1Info = header1Info;
            Header2Info = header2Info;
            Notification = notification;
        }

        public List<PtInfNotificationItem>? Header1Info { get; private set; }
        public List<PtInfNotificationItem>? Header2Info { get; private set; }
        public List<PtInfNotificationItem>? Notification { get; private set; }

    }
}
