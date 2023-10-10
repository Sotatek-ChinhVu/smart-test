using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetSetNameMntResponse
    {
        public GetSetNameMntResponse(List<SetNameMntModel> data)
        {
            Data = data;
        }

        public List<SetNameMntModel> Data { get; private set; }
    }
}
