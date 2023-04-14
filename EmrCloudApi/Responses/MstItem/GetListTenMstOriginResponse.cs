using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetListTenMstOriginResponse
    {
        public GetListTenMstOriginResponse(List<TenMstOriginModel> tenMsts, List<int> startDateDisplay)
        {
            TenMsts = tenMsts;
            StartDateDisplay = startDateDisplay;
        }

        public List<TenMstOriginModel> TenMsts { get; private set; }

        public List<int> StartDateDisplay { get; private set; }
    }
}
