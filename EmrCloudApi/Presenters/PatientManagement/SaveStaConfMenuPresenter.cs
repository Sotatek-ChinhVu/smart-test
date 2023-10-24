using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientManagement;
using UseCase.PatientManagement.SaveStaConf;

namespace EmrCloudApi.Presenters.PatientManagement
{
    public class SaveStaConfMenuPresenter : ISaveStaConfMenuOutputPort
    {
        public Response<SaveStaConfMenuResponse> Result { get; private set; } = new Response<SaveStaConfMenuResponse>();

        public void Complete(SaveStaConfMenuOutputData outputData)
        {
            Result.Data = new SaveStaConfMenuResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(SaveStaConfMenuStatus status) => status switch
        {
            SaveStaConfMenuStatus.Successed => ResponseMessage.Success,
            SaveStaConfMenuStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
