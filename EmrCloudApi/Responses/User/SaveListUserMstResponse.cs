using UseCase.User.SaveListUserMst;

namespace EmrCloudApi.Responses.User
{
    public class SaveListUserMstResponse
    {
        public SaveListUserMstResponse(SaveListUserMstStatus status)
        {
            Status = status;
        }

        public SaveListUserMstStatus Status { get; private set; }
    }
}
