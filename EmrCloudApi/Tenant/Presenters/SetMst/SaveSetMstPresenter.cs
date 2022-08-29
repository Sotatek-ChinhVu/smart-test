﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetMst;
using UseCase.SetMst.SaveSetMst;

namespace EmrCloudApi.Tenant.Presenters.SetMst;

public class SaveSetMstPresenter : ISaveSetMstOutputPort
{
    public Response<SaveSetMstResponse> Result { get; private set; } = new Response<SaveSetMstResponse>();

    public void Complete(SaveSetMstOutputData output)
    {
        Result.Data = new SaveSetMstResponse(output.setMstModel);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveSetMstStatus status) => status switch
    {
        SaveSetMstStatus.Successed => ResponseMessage.Success,
        SaveSetMstStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
