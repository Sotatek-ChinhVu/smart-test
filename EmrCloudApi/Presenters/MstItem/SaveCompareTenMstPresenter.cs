using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.SaveCompareTenMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class SaveCompareTenMstPresenter : ISaveCompareTenMstOutputPort
    {
        public Response<SaveCompareTenMstResponse> Result { get; private set; } = new Response<SaveCompareTenMstResponse>();

        private string GetMessage(SaveCompareTenMstStatus status) => status switch
        {
            SaveCompareTenMstStatus.Success => ResponseMessage.Success,
            SaveCompareTenMstStatus.ListDataEmpty => ResponseMessage.NoData,
            SaveCompareTenMstStatus.Faild => ResponseMessage.Failed,
            SaveCompareTenMstStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            _ => string.Empty
        };

        public void Complete(SaveCompareTenMstOutputData outputData)
        {
            Result.Data = new SaveCompareTenMstResponse(outputData.Result);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
    }
}
