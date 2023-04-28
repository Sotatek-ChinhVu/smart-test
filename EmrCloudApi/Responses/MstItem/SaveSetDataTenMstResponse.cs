using UseCase.MstItem.SaveSetDataTenMst;

namespace EmrCloudApi.Responses.MstItem
{
    public class SaveSetDataTenMstResponse
    {
        public SaveSetDataTenMstResponse(SaveSetDataTenMstStatus status)
        {
            Status = status;
        }

        public SaveSetDataTenMstStatus Status { get; private set; }
    }
}
