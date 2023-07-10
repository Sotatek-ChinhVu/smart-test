﻿using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using UseCase.SetMst.ReorderSetMst;

namespace EmrCloudApi.Presenters.SetMst
{
    public class ReorderSetMstPresenter : IReorderSetMstOutputPort
    {
        public Response<ReorderSetMstResponse> Result { get; private set; } = new Response<ReorderSetMstResponse>();

        public void Complete(ReorderSetMstOutputData output)
        {
            Result.Data = new ReorderSetMstResponse(output.Status == ReorderSetMstStatus.Successed);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(ReorderSetMstStatus status) => status switch
        {
            ReorderSetMstStatus.Successed => ResponseMessage.Success,
            ReorderSetMstStatus.Failed => ResponseMessage.Failed,
            ReorderSetMstStatus.InvalidLevel => ResponseMessage.InvalidLevel,
            ReorderSetMstStatus.InvalidDragSetCd => ResponseMessage.InvalidDragSetCd,
            ReorderSetMstStatus.InvalidDropSetCd => ResponseMessage.InvalidDropSetCd,
            ReorderSetMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            ReorderSetMstStatus.MedicalScreenLocked => ResponseMessage.MedicalScreenLocked,
            _ => string.Empty
        };
    }
}
