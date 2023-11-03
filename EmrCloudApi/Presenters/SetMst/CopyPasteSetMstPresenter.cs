﻿using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using UseCase.SetMst.CopyPasteSetMst;

namespace EmrCloudApi.Presenters.SetMst
{
    public class CopyPasteSetMstPresenter : ICopyPasteSetMstOutputPort
    {
        public Response<CopyPasteSetMstResponse> Result { get; private set; } = new Response<CopyPasteSetMstResponse>();

        public void Complete(CopyPasteSetMstOutputData output)
        {
            var rootSet = output.SetMstModels.OrderBy(s => s.Level1).ThenBy(s => s.Level2).ThenBy(s => s.Level3).FirstOrDefault();
            Result.Data = new CopyPasteSetMstResponse(rootSet?.SetCd ?? 0);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(CopyPasteSetMstStatus status) => status switch
        {
            CopyPasteSetMstStatus.Successed => ResponseMessage.Success,
            CopyPasteSetMstStatus.Failed => ResponseMessage.Failed,
            CopyPasteSetMstStatus.InvalidLevel => ResponseMessage.InvalidLevel,
            CopyPasteSetMstStatus.InvalidCopySetCd => ResponseMessage.InvalidCopySetCd,
            CopyPasteSetMstStatus.InvalidPasteSetCd => ResponseMessage.InvalidPasteSetCd,
            CopyPasteSetMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            CopyPasteSetMstStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            CopyPasteSetMstStatus.InvalidPasteSetKbn => ResponseMessage.InvalidPasteSetKbn,
            CopyPasteSetMstStatus.InvalidPasteSetKbnEdaNo => ResponseMessage.InvalidPasteSetKbnEdaNo,
            CopyPasteSetMstStatus.InvalidCopySetKbn => ResponseMessage.InvalidCopySetKbn,
            CopyPasteSetMstStatus.InvalidCopySetKbnEdaNo => ResponseMessage.InvalidCopySetKbnEdaNo,
            CopyPasteSetMstStatus.MedicalScreenLocked => ResponseMessage.MedicalScreenLocked,
            _ => string.Empty
        };
    }
}
