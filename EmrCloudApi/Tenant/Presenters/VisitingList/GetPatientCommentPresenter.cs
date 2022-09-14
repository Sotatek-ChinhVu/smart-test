using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.VisitingList;
using UseCase.VisitingList.PatientComment;

namespace EmrCloudApi.Tenant.Presenters.VisitingList
{
    public class GetPatientCommentPresenter : IGetPatientCommentOutputPort
    {
        public Response<GetPatientCommentResponse> Result { get; private set; } = new Response<GetPatientCommentResponse>();

        public void Complete(GetPatientCommentOutputData outputData)
        {
            Result.Data = new GetPatientCommentResponse(outputData.patientCommentModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetPatientCommentStatus status) => status switch
        {
            GetPatientCommentStatus.Success => ResponseMessage.Success,
            GetPatientCommentStatus.InvalidData => ResponseMessage.InvalidKeyword,
            GetPatientCommentStatus.NoData => ResponseMessage.NoData,
            _=>  string.Empty
        };
    }
}
