using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.User;
using UseCase.User.UpsertList;

namespace EmrCloudApi.Tenant.Presenters.User
{
    public class UpsertUserListPresenter : IUpsertUserListOutputPort
    {
        public Response<UpsertUserResponse> Result { get; private set; } = default!;

        public void Complete(UpsertUserListOutputData outputData)
        {
            Result = new Response<UpsertUserResponse>()
            {
                Data = new UpsertUserResponse(outputData.Status == UpsertUserListStatus.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }
        private string GetMessage(UpsertUserListStatus status) => status switch 
        {
            UpsertUserListStatus.Success => ResponseMessage.Success,
            UpsertUserListStatus.Failed => ResponseMessage.Failed,
            UpsertUserListStatus.DuplicateId => ResponseMessage.DuplicateId,
            UpsertUserListStatus.ExistedId => ResponseMessage.ExistedId,
            UpsertUserListStatus.UserListInputNoData => ResponseMessage.UserListInputNoData,
            UpsertUserListStatus.UserListInvalidId => ResponseMessage.UserListInvalidId,
            UpsertUserListStatus.UserListInvalidUserId => ResponseMessage.UserListInvalidUserId,
            UpsertUserListStatus.UserListInvalidJobCd => ResponseMessage.UserListInvalidJobCd,
            UpsertUserListStatus.UserListInvalidManagerKbn => ResponseMessage.UserListInvalidManagerKbn,
            UpsertUserListStatus.UserListInvalidKaId => ResponseMessage.UserListInvalidKaId,
            UpsertUserListStatus.UserListInvalidKanaName => ResponseMessage.UserListInvalidKanaName,
            UpsertUserListStatus.UserListInvalidName => ResponseMessage.UserListInvalidName,
            UpsertUserListStatus.UserListInvalidSname => ResponseMessage.UserListInvalidSname,
            UpsertUserListStatus.UserListInvalidLoginId => ResponseMessage.UserListInvalidLoginId,
            UpsertUserListStatus.UserListInvalidLoginPass => ResponseMessage.UserListInvalidLoginPass,
            UpsertUserListStatus.UserListInvalidStartDate => ResponseMessage.UserListInvalidStartDate,
            UpsertUserListStatus.UserListInvalidEndDate => ResponseMessage.UserListInvalidEndDate,
            UpsertUserListStatus.UserListInvalidSortNo => ResponseMessage.UserListInvalidSortNo,
            UpsertUserListStatus.UserListInvalidIsDeleted => ResponseMessage.UserListInvalidIsDeleted,
            UpsertUserListStatus.UserListInvalidRenkeiCd1 => ResponseMessage.UserListInvalidRenkeiCd1,
            UpsertUserListStatus.UserListInvalidDrName => ResponseMessage.UserListInvalidDrName,
            UpsertUserListStatus.UserListInvalidHpId => ResponseMessage.InvalidHpId,
            _ => string.Empty
        };
    }
}