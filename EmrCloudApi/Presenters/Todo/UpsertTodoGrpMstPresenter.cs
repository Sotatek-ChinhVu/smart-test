using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Todo;
using UseCase.Todo.TodoGrpMst;

namespace EmrCloudApi.Presenters.TodoGroupMst
{
    public class UpsertTodoGrpMstPresenter : IUpsertTodoGrpMstOutputPort
    {
        public Response<UpsertTodoGrpMstResponse> Result { get; private set; } = default!;
        public void Complete(UpsertTodoGrpMstOutputData outputData)
        {
            Result = new Response<UpsertTodoGrpMstResponse>()
            {
                Data = new UpsertTodoGrpMstResponse(outputData.Status == UpsertTodoGrpMstStatus.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }
        private static string GetMessage(UpsertTodoGrpMstStatus status) => status switch
        {
            UpsertTodoGrpMstStatus.Success => ResponseMessage.Success,
            UpsertTodoGrpMstStatus.Failed => ResponseMessage.Failed,
            UpsertTodoGrpMstStatus.InvalidGrpColor => ResponseMessage.InvalidGrpColor,
            UpsertTodoGrpMstStatus.InvalidTodoGrpName => ResponseMessage.InvalidTodoGrpName,
            UpsertTodoGrpMstStatus.InvalidTodoGrpNo => ResponseMessage.InvalidTodoGrpNo,
            UpsertTodoGrpMstStatus.InvalidTodoGrpMst => ResponseMessage.InvalidTodoGrpMst,
            UpsertTodoGrpMstStatus.InputNoData => ResponseMessage.InputNoData,
            UpsertTodoGrpMstStatus.InvalidExistedTodoGrpNo => ResponseMessage.InvalidExistedTodoGrpNo,
            UpsertTodoGrpMstStatus.InvalidIsDeleted => ResponseMessage.InvalidIsDeleted,
            UpsertTodoGrpMstStatus.InvalidSortNo => ResponseMessage.InvalidSortNo,
            _ => string.Empty
        };
    }
}
