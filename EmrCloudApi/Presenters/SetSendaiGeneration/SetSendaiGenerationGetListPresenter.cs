using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem.DiseaseSearch;
using EmrCloudApi.Responses;
using UseCase.MstItem.GetDiseaseList;
using UseCase.SetSendaiGeneration.GetList;
using EmrCloudApi.Responses.GetSendaiGeneration;

namespace EmrCloudApi.Presenters.SetSendaiGeneration
{
    public class SetSendaiGenerationGetListPresenter : ISetSendaiGenerationOutputPort
    {
        public Response<SetSendaiGenerationGetListResponse> Result { get; private set; } = new();

        public void Complete(SetSendaiGenerationOutputData output)
        {
            Result.Data = new SetSendaiGenerationGetListResponse(output.ListData);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(SetSendaiGenerationStatus status) => status switch
        {
            SetSendaiGenerationStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SetSendaiGenerationStatus.Success => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
