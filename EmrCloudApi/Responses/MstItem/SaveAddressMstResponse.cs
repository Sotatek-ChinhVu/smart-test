using UseCase.MstItem.SaveAddressMst;

namespace EmrCloudApi.Responses.MstItem
{
    public class SaveAddressMstResponse
    {
        public SaveAddressMstResponse(long id, string postCd, string errorMessage, SaveAddressMstStatus status)
        {
            Id = id;
            PostCd = postCd;
            ErrorMessage = errorMessage;
            Status = status;
        }

        public long Id { get; private set; }

        public string PostCd { get; private set; }

        public string ErrorMessage { get; private set; }

        public SaveAddressMstStatus Status { get; private set; }
    }
}
