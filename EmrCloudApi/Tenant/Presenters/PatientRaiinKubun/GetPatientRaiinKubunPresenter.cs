using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientRaiinKubun;
using UseCase.PatientRaiinKubun.Get;

namespace EmrCloudApi.Tenant.Presenters.PatientRaiinKubun
{
    public class GetPatientRaiinKubunPresenter : IGetPatientRaiinKubunOutputPort
    {
        public Response<GetPatientRaiinKubunResponse> Result { get; private set; } = default!;

        public void Complete(GetPatientRaiinKubunOutputData outputData)
        {
            Result = new Response<GetPatientRaiinKubunResponse>()
            {
                Data = new GetPatientRaiinKubunResponse(outputData.ListPatientRaiinKubun),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetPatientRaiinKubunStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetPatientRaiinKubunStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetPatientRaiinKubunStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case GetPatientRaiinKubunStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetPatientRaiinKubunStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
