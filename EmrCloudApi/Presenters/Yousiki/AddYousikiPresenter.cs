using EmrCloudApi.Responses.Yousiki;
using UseCase.Yousiki.AddYousiki;
using EmrCloudApi.Responses;
using EmrCloudApi.Constants;

namespace EmrCloudApi.Presenters.Yousiki
{
    public class AddYousikiPresenter : IAddYousikiOutputPort
    {
        public Response<AddYousikiResponse> Result { get; private set; } = new();

        public void Complete(AddYousikiOutputData output)
        {
            Result.Data = new AddYousikiResponse(output.MessageType);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(AddYousikiStatus status) => status switch
        {
            AddYousikiStatus.Successed => ResponseMessage.Success,
            AddYousikiStatus.Failed => ResponseMessage.Failed,
            AddYousikiStatus.InvalidYousikiSinYm => ResponseMessage.InvalidYousikiSinYm,
            AddYousikiStatus.InvalidYousikiSelectDataType0 => ResponseMessage.InvalidYousikiSelectDataType0,
            AddYousikiStatus.InvalidYousikiSelectDataType1 => ResponseMessage.InvalidYousikiSelectDataType1,
            AddYousikiStatus.InvalidYousikiSelectDataType2 => ResponseMessage.InvalidYousikiSelectDataType2,
            AddYousikiStatus.InvalidYousikiSelectDataType3 => ResponseMessage.InvalidYousikiSelectDataType3,
            AddYousikiStatus.IsYousikiExist => ResponseMessage.IsYousikiExist,
            AddYousikiStatus.InvalidHealthInsuranceAccepted => ResponseMessage.InvalidHealthInsuranceAccepted,
            _ => string.Empty
        };
    }
}
