using Domain.Models.User;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetListUserResponse
    {
        public GetListUserResponse(List<UserMstModel> data)
        {
            Data = data;
        }
        public List<UserMstModel> Data { get; private set; }
    }
}
