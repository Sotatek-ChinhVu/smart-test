using EmrCloudApi.Constants;
using EmrCloudApi.Responses.RaiinListSetting;
using EmrCloudApi.Responses;
using UseCase.RaiinListSetting.GetFilingcategory;

namespace EmrCloudApi.Presenters.RaiinListSetting
{
    public class GetFilingcategoryPresenter : IGetFilingcategoryOutputPort
    {
        public Response<GetFilingcategoryResponse> Result { get; private set; } = new Response<GetFilingcategoryResponse>();

        public void Complete(GetFilingcategoryOutputData outputData)
        {
            Result.Data = new GetFilingcategoryResponse(outputData.Data);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(GetFilingcategoryStatus status) => status switch
        {
            GetFilingcategoryStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetFilingcategoryStatus.Successful => ResponseMessage.Success,
            GetFilingcategoryStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
