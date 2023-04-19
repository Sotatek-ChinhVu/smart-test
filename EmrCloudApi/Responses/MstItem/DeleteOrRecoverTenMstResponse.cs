using UseCase.MstItem.DeleteOrRecoverTenMst;

namespace EmrCloudApi.Responses.MstItem
{
    public class DeleteOrRecoverTenMstResponse
    {
        public DeleteOrRecoverTenMstResponse(DeleteOrRecoverTenMstStatus status)
        {
            Status = status;
        }

        public DeleteOrRecoverTenMstStatus Status { get; private set; }
    }
}
