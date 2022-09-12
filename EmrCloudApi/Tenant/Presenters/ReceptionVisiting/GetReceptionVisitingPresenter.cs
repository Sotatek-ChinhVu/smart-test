using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses.PatientRaiinKubun;
using EmrCloudApi.Tenant.Responses;
using UseCase.PatientRaiinKubun.Get;
using UseCase.ReceptionVisiting.Get;
using EmrCloudApi.Tenant.Responses.ReceptionVisiting;
using System.Xml.Linq;

namespace EmrCloudApi.Tenant.Presenters.ReceptionVisiting
{
    public class GetReceptionVisitingPresenter : IGetReceptionVisitingOutputPort
    {
        public Response<GetReceptionVisitingResponse> Result { get; private set; } = new Response<GetReceptionVisitingResponse>();

        public void Complete(GetReceptionVisitingOutputData output)
        {
            Result.Data = new GetReceptionVisitingResponse(output.ListVisiting);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetReceptionVisitingStatus status) => status switch
        {
            GetReceptionVisitingStatus.Success => ResponseMessage.Success,
            GetReceptionVisitingStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            GetReceptionVisitingStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty

        };
    }
}

