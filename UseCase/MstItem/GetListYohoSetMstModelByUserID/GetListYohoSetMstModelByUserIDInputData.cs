using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListYohoSetMstModelByUserID
{
    public class GetListYohoSetMstModelByUserIDInputData : IInputData<GetListYohoSetMstModelByUserIDOutputData>
    {
        public GetListYohoSetMstModelByUserIDInputData(int hpId,int userIdLogin, int sinDate, int userId)
        {
            HpId = hpId;
            UserIdLogin = userIdLogin;
            SinDate = sinDate;
            UserId = userId;
        }
        public int UserIdLogin {  get; private set; }
        public int HpId {  get; private set; }
        public int SinDate {  get; private set; }
        public int UserId {  get; private set; }
    }
}
