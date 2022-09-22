using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.VisitingList;
using UseCase.VisitingList.PatientComment;

namespace EmrCloudApi.Tenant.Presenters.PatientInfor
{
    public class GetPatientCommentPresenter : IGetPatientCommentOutputPort
    {
        public Response<GetPatientCommentResponse> Result { get; private set; } = new Response<GetPatientCommentResponse>();

        public void Complete(GetPatientCommentOutputData outputData)
        {
            Result.Data = new GetPatientCommentResponse(outputData.PatientInforModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetPatientCommentStatus status) => status switch
        {
            GetPatientCommentStatus.Success => ResponseMessage.Success,
            GetPatientCommentStatus.InvalidData => ResponseMessage.InvalidKeyword,
            GetPatientCommentStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
