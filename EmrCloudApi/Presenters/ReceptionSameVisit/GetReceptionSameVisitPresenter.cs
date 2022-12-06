using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReceptionSameVisit;
using UseCase.ReceptionSameVisit.Get;

namespace EmrCloudApi.Presenters.ReceptionSameVisit
{
    public class GetReceptionSameVisitPresenter: IGetReceptionSameVisitOutputPort
    {
        public Response<GetReceptionSameVisitResponse> Result { get; private set; } = new Response<GetReceptionSameVisitResponse>();

        public void Complete(GetReceptionSameVisitOutputData output)
        {
            Result.Data = new GetReceptionSameVisitResponse(output.ListSameVisit);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetReceptionSameVisitStatus status) => status switch
        {
            GetReceptionSameVisitStatus.Success => ResponseMessage.Success,
            GetReceptionSameVisitStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetReceptionSameVisitStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            GetReceptionSameVisitStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
            _ => string.Empty
        };
    }
}
