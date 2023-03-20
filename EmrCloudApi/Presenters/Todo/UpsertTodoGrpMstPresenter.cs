using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Todo;
using EmrCloudApi.Tenant.Responses.ApprovalInfo;
using UseCase.ApprovalInfo.UpdateApprovalInfList;
using UseCase.Todo.TodoGrpMst;
using UseCase.Todo.UpsertTodoGrpMst;

namespace EmrCloudApi.Presenters.TodoGroupMst
{
    public class UpsertTodoGrpMstPresenter : IUpsertTodoGrpMstOutputPort
    {
        public Response<UpsertTodoGrpMstResponse> Result { get; private set; } = default!;
        public void Complete(UpsertTodoGrpMstOutputData outputData)
        {
            Result = new Response<UpsertTodoGrpMstResponse>()
            {
                Data = new UpsertTodoGrpMstResponse(outputData.Status == TodoGrpMstConstant.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }
        private static string GetMessage(TodoGrpMstConstant status) => status switch
        {
            TodoGrpMstConstant.Success => ResponseMessage.Success,
            TodoGrpMstConstant.Failed => ResponseMessage.Failed,
            TodoGrpMstConstant.InvalidGrpColor => ResponseMessage.InvalidGrpColor,
            TodoGrpMstConstant.InvalidTodoGrpName => ResponseMessage.InvalidTodoGrpName,
            TodoGrpMstConstant.InvalidTodoGrpNo => ResponseMessage.InvalidTodoGrpNo,
            TodoGrpMstConstant.InvalidTodoGrpMst => ResponseMessage.InvalidTodoGrpMst,
            TodoGrpMstConstant.InputNoData => ResponseMessage.InputNoData,
            TodoGrpMstConstant.InvalidExistedTodoGrpNo => ResponseMessage.InvalidExistedTodoGrpNo,
            TodoGrpMstConstant.InvalidIsDeleted => ResponseMessage.InvalidIsDeleted,
            TodoGrpMstConstant.InvalidSortNo => ResponseMessage.InvalidSortNo,
            _ => string.Empty
        };
    }
}
