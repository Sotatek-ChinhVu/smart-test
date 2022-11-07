namespace EmrCloudApi.Tenant.Requests.FlowSheet
{
    public class GetListHolidayRequest
    {
        public int HpId { get; set; }

        public int HolidayFrom { get; set; }

        public int HolidayTo { get; set; }
    }
}
