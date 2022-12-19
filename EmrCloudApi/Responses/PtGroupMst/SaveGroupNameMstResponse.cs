using UseCase.PtGroupMst.SaveGroupNameMst;

namespace EmrCloudApi.Responses.PtGroupMst
{
    public class SaveGroupNameMstResponse
    {
        public SaveGroupNameMstResponse(SaveGroupNameMstStatus state)
        {
            State = state;
        }

        public SaveGroupNameMstStatus State { get; private set; }
    }
}
