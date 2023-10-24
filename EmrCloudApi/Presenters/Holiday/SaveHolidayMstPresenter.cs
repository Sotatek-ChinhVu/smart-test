using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using UseCase.Holiday.SaveHoliday;
using EmrCloudApi.Responses.Holiday;

namespace EmrCloudApi.Presenters.Holiday
{
    public class SaveHolidayMstPresenter : ISaveHolidayMstOutputPort
    {
        public Response<SaveHolidayMstResponse> Result { get; private set; } = new Response<SaveHolidayMstResponse>();
        public void Complete(SaveHolidayMstOutputData outputData)
        {
            Result.Data = new SaveHolidayMstResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(SaveHolidayMstStatus status) => status switch
        {
            SaveHolidayMstStatus.Successful => ResponseMessage.Success,
            SaveHolidayMstStatus.Failed => ResponseMessage.Failed,
            SaveHolidayMstStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            SaveHolidayMstStatus.NoPermission => ResponseMessage.NoPermission,
            _ => string.Empty
        };
    }
}
