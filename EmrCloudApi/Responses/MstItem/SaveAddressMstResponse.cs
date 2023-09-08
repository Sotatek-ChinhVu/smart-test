using UseCase.MstItem.SaveAddressMst;

namespace EmrCloudApi.Responses.MstItem
{
    public class SaveAddressMstResponse
    {
        public SaveAddressMstResponse(SaveAddressMstStatus status)
        {
            Status = status;
        }

        public SaveAddressMstStatus Status { get; private set; }
    }
}
