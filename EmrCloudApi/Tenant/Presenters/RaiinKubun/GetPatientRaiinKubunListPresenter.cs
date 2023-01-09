using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.RaiinKubun;
using UseCase.RaiinKbn.GetPatientRaiinKubunList;

namespace EmrCloudApi.Presenters.RaiinKubun
{
    public class GetPatientRaiinKubunListPresenter : IGetPatientRaiinKubunListOutputPort
    {
        public Response<GetPatientRaiinKubunListResponse> Result { get; private set; } = default!;

        public void Complete(GetPatientRaiinKubunListOutputData outputData)
        {
            Result = new Response<GetPatientRaiinKubunListResponse>()
            {
                Data = new GetPatientRaiinKubunListResponse(outputData.ListPatientRaiinKubun),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetPatientRaiinKubunListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetPatientRaiinKubunListStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetPatientRaiinKubunListStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case GetPatientRaiinKubunListStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetPatientRaiinKubunListStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
