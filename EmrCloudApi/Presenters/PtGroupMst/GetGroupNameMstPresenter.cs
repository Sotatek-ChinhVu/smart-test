using EmrCloudApi.Responses.PtGroupMst;
using EmrCloudApi.Responses;
using UseCase.PtGroupMst.GetGroupNameMst;
using EmrCloudApi.Constants;

namespace EmrCloudApi.Presenters.PtGroupMst
{
    public class GetGroupNameMstPresenter : IGetGroupNameMstOutputPort
    {
        public Response<GetGroupNameMstResponse> Result { get; private set; } = new Response<GetGroupNameMstResponse>();

        public void Complete(GetGroupNameMstOutputData outputData)
        {
            Result.Data = new GetGroupNameMstResponse(outputData.Data.Select(x => new GroupNameDtoResponse(x)).ToList());
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(GetGroupNameMstStatus status) => status switch
        {
            GetGroupNameMstStatus.Successful => ResponseMessage.Success,
            GetGroupNameMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetGroupNameMstStatus.DataNotFound => ResponseMessage.NotFound,
            _ => string.Empty
        };
    }
}
