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
        private static string GetMessage(UpsertUserListStatus status) => status switch 
        {
            UpsertUserListStatus.Success => ResponseMessage.Success,
            UpsertUserListStatus.Failed => ResponseMessage.Failed,
            UpsertUserListStatus.DuplicateId => ResponseMessage.DuplicateId,
            UpsertUserListStatus.ExistedId => ResponseMessage.ExistedId,
            UpsertUserListStatus.UserListInputNoData => ResponseMessage.UpsertInputNoData,
            UpsertUserListStatus.UserListUpdateNoSuccess => ResponseMessage.UpsertUpdateNoSuccess,
            UpsertUserListStatus.UserListKaIdNoExist => ResponseMessage.UpsertKaIdNoExist,
            UpsertUserListStatus.UserListIdNoExist => ResponseMessage.UpsertIdNoExist,
            UpsertUserListStatus.UserListInvalidExistedLoginId => ResponseMessage.UpsertInvalidExistedLoginId,
            UpsertUserListStatus.InvalidId => ResponseMessage.UpsertInvalidId,
            UpsertUserListStatus.InvalidUserId => ResponseMessage.UpsertInvalidUserId,
            UpsertUserListStatus.InvalidJobCd => ResponseMessage.UpsertInvalidJobCd,
            UpsertUserListStatus.InvalidManagerKbn => ResponseMessage.UpsertInvalidManagerKbn,
            UpsertUserListStatus.InvalidKaId => ResponseMessage.UpsertInvalidKaId,
            UpsertUserListStatus.InvalidKanaName => ResponseMessage.UpsertInvalidKanaName,
            UpsertUserListStatus.InvalidName => ResponseMessage.UpsertInvalidName,
            UpsertUserListStatus.InvalidSname => ResponseMessage.UpsertInvalidSname,
            UpsertUserListStatus.InvalidLoginId => ResponseMessage.UpsertInvalidLoginId,
            UpsertUserListStatus.InvalidLoginPass => ResponseMessage.UpsertInvalidLoginPass,
            UpsertUserListStatus.InvalidStartDate => ResponseMessage.UpsertInvalidStartDate,
            UpsertUserListStatus.InvalidEndDate => ResponseMessage.UpsertInvalidEndDate,
            UpsertUserListStatus.InvalidSortNo => ResponseMessage.UpsertInvalidSortNo,
            UpsertUserListStatus.InvalidIsDeleted => ResponseMessage.UpsertInvalidIsDeleted,
            UpsertUserListStatus.InvalidRenkeiCd1 => ResponseMessage.UpsertInvalidRenkeiCd1,
            UpsertUserListStatus.InvalidDrName => ResponseMessage.UpsertInvalidDrName,
            UpsertUserListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            UpsertUserListStatus.UserListInvalidExistedUserId => ResponseMessage.UpsertInvalidExistedUserId,
            _ => string.Empty
        };
    }
}