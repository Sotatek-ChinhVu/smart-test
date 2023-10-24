using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.ContainerMasterUpdate;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class ContainerMasterUpdatePresenter : IContainerMasterUpdateOutPutPort
    {
        public Response<ContainerMasterUpdateResponse> Result { get; private set; } = default!;

        public void Complete(ContainerMasterUpdateOutPutData outputData)
        {
            Result = new Response<ContainerMasterUpdateResponse>()
            {
                Data = new ContainerMasterUpdateResponse(outputData.Status == ContainerMasterUpdateStatus.Successful),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private static string GetMessage(ContainerMasterUpdateStatus status) => status switch
        {
            ContainerMasterUpdateStatus.Successful => ResponseMessage.Success,
            ContainerMasterUpdateStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
