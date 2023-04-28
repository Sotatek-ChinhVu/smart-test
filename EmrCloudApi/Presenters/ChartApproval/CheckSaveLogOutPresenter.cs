using EmrCloudApi.Responses.ChartApproval;
using EmrCloudApi.Responses;
using UseCase.ChartApproval.CheckSaveLogOut;

namespace EmrCloudApi.Presenters.ChartApproval
{
    public class CheckSaveLogOutPresenter : ICheckSaveLogOutOutputPort
    {
        public Response<CheckSaveLogOutResponse> Result { get; private set; } = new Response<CheckSaveLogOutResponse>();

        public void Complete(CheckSaveLogOutOutputData outputData)
        {
            Result.Data = new CheckSaveLogOutResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
    }
}
