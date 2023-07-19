using EmrCloudApi.Constants;
using EmrCloudApi.Responses.User;
using EmrCloudApi.Responses;
using UseCase.User.GetListFunctionPermission;

namespace EmrCloudApi.Presenters.User
{
    public class GetListFunctionPermissionPresenter : IGetListFunctionPermissionOutputPort
    {
        public Response<GetListFunctionPermissionResponse> Result { get; private set; } = new Response<GetListFunctionPermissionResponse>();

        public void Complete(GetListFunctionPermissionOutputData output)
        {
            Result.Data = new GetListFunctionPermissionResponse(output.FunctionPermissions);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetListFunctionPermissionStatus status) => status switch
        {
            GetListFunctionPermissionStatus.Successful => ResponseMessage.Success,
            GetListFunctionPermissionStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
