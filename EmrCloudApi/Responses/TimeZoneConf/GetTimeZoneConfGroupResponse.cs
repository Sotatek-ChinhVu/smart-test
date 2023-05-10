using Domain.Models.TimeZone;

namespace EmrCloudApi.Responses.TimeZoneConf
{
    public class GetTimeZoneConfGroupResponse
    {
        public GetTimeZoneConfGroupResponse(List<TimeZoneConfGroupModel> datas, bool isHavePermission)
        {
            Datas = datas;
            IsHavePermission = isHavePermission;
        }

        public List<TimeZoneConfGroupModel> Datas { get; private set; }

        public bool IsHavePermission { get; private set; }
    }
}
