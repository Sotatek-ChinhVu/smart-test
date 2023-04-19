using Domain.Models.MstItem;
using EmrCloudApi.Requests.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetListTenMstOriginResponse
    {
        public GetListTenMstOriginResponse(List<TenMstOriginModelDto> tenMsts, List<int> startDateDisplay)
        {
            TenMsts = tenMsts;
            StartDateDisplay = startDateDisplay;
        }

        public List<TenMstOriginModelDto> TenMsts { get; private set; }

        public List<int> StartDateDisplay { get; private set; }
    }
}
