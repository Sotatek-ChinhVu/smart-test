namespace EmrCloudApi.Requests.User
{
    public class SaveListUserMstRequest
    {
        public SaveListUserMstRequest(List<UserMstDto> users)
        {
            Users = users;
        }

        public List<UserMstDto> Users { get; private set; }
    }
}
