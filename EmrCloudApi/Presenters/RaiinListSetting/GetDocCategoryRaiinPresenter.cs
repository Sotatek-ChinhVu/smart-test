using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using UseCase.RaiinListSetting.GetDocCategory;
using EmrCloudApi.Responses.RaiinListSetting;

namespace EmrCloudApi.Presenters.RaiinListSetting
{
    public class GetDocCategoryRaiinPresenter : IGetDocCategoryRaiinOutputPort
    {
        public Response<GetDocCategoryRaiinResponse> Result { get; private set; } = new Response<GetDocCategoryRaiinResponse>();

        public void Complete(GetDocCategoryRaiinOutputData outputData)
        {
            Result.Data = new GetDocCategoryRaiinResponse(outputData.Data);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(GetDocCategoryRaiinStatus status) => status switch
        {
            GetDocCategoryRaiinStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetDocCategoryRaiinStatus.Successful => ResponseMessage.Success,
            GetDocCategoryRaiinStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
