using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Todo;
using EmrCloudApi.Responses;
using UseCase.Todo.TodoInf;

namespace EmrCloudApi.Presenters.Todo
{
    public class UpsertTodoInfPresenter : IUpsertTodoInfOutputPort
    {
        public Response<UpsertTodoInfResponse> Result { get; private set; } = default!;
        public void Complete(UpsertTodoInfOutputData outputData)
        {
            Result = new Response<UpsertTodoInfResponse>()
            {
                Data = new UpsertTodoInfResponse(outputData.Status == UpsertTodoInfStatus.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }
        private static string GetMessage(UpsertTodoInfStatus status) => status switch
        {
            UpsertTodoInfStatus.Success => ResponseMessage.Success,
            UpsertTodoInfStatus.InvalidTodoNo => ResponseMessage.InvalidTodoNo,
            UpsertTodoInfStatus.InvalidTodoEdaNo => ResponseMessage.InvalidTodoEdaNo,
            UpsertTodoInfStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            UpsertTodoInfStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
            UpsertTodoInfStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            UpsertTodoInfStatus.InvalidTodoKbnNo => ResponseMessage.InvalidTodoKbnNo,
            UpsertTodoInfStatus.InvalidTodoGrpNo => ResponseMessage.InvalidTodoGrpNo,
            UpsertTodoInfStatus.InvalidTanto => ResponseMessage.InvalidTanto,
            UpsertTodoInfStatus.InvalidTerm => ResponseMessage.InvalidTerm,
            UpsertTodoInfStatus.InvalidIsDone => ResponseMessage.InvalidIsDone,
            UpsertTodoInfStatus.InvalidIsDeleted => ResponseMessage.InvalidIsDeleted,
            UpsertTodoInfStatus.InvalidExistedTodoNo => ResponseMessage.InvalidExistedTodoNo,
            UpsertTodoInfStatus.InvalidExistedTodoEdaNo => ResponseMessage.InvalidExistedTodoEdaNo,
            UpsertTodoInfStatus.InvalidExistedPtId => ResponseMessage.InvalidExistedPtId,
            _ => string.Empty
        };
    }
}