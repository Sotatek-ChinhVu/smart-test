using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.UpsertMaterialMaster;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class UpsertMaterialMasterPresenter : IUpsertMaterialMasterOutputPort
    {
        public Response<UpsertMaterialMasterResponse> Result { get; private set; } = default!;

        public void Complete(UpsertMaterialMasterOutputData outputData)
        {
            Result = new Response<UpsertMaterialMasterResponse>()
            {
                Data = new UpsertMaterialMasterResponse(outputData.Status == UpsertMaterialMasterStatus.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private static string GetMessage(UpsertMaterialMasterStatus status) => status switch
        {
            UpsertMaterialMasterStatus.Success => ResponseMessage.Success,
            UpsertMaterialMasterStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
