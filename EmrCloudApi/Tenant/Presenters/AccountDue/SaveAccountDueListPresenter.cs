﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.AccountDue;
using UseCase.AccountDue.SaveAccountDueList;

namespace EmrCloudApi.Tenant.Presenters.AccountDue;

public class SaveAccountDueListPresenter
{
    public Response<SaveAccountDueListResponse> Result { get; private set; } = new();

    public void Complete(SaveAccountDueListOutputData output)
    {
        Result.Data = new SaveAccountDueListResponse(output.Status == SaveAccountDueListStatus.Successed || output.Status == SaveAccountDueListStatus.NoItemChange);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveAccountDueListStatus status) => status switch
    {
        SaveAccountDueListStatus.Successed => ResponseMessage.Success,
        SaveAccountDueListStatus.Failed => ResponseMessage.Failed,
        SaveAccountDueListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveAccountDueListStatus.InvalidPtId => ResponseMessage.PtInfNotFould,
        SaveAccountDueListStatus.InvalidSindate => ResponseMessage.InvalidSinDate,
        SaveAccountDueListStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SaveAccountDueListStatus.InvalidNyukinKbn => ResponseMessage.InvalidNyukinKbn,
        SaveAccountDueListStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
        SaveAccountDueListStatus.InvalidSortNo => ResponseMessage.InvalidSortNo,
        SaveAccountDueListStatus.InvalidAdjustFutan => ResponseMessage.InvalidAdjustFutan,
        SaveAccountDueListStatus.InvalidNyukinGaku => ResponseMessage.InvalidNyukinGaku,
        SaveAccountDueListStatus.InvalidPaymentMethodCd => ResponseMessage.InvalidPaymentMethodCd,
        SaveAccountDueListStatus.InvalidNyukinDate => ResponseMessage.InvalidNyukinDate,
        SaveAccountDueListStatus.InvalidUketukeSbt => ResponseMessage.InvalidUketukeSbt,
        SaveAccountDueListStatus.NyukinCmtMaxLength100 => ResponseMessage.NyukinCmtMaxLength100,
        SaveAccountDueListStatus.InvalidSeikyuGaku => ResponseMessage.InvalidSeikyuGaku,
        SaveAccountDueListStatus.InvalidSeikyuTensu => ResponseMessage.InvalidSeikyuTensu,
        SaveAccountDueListStatus.InvalidSeqNo => ResponseMessage.InvalidSeqNo,
        SaveAccountDueListStatus.NoItemChange => ResponseMessage.NoItemChange,
        _ => string.Empty
    };
}
