using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.RaiinKubun;
using UseCase.RaiinKubunMst.SaveRaiinKbnInfList;

namespace EmrCloudApi.Presenters.RaiinKubun
{
    public class SaveRaiinKbnInfListPresenter : ISaveRaiinKbnInfListOutputPort
    {
        public Response<SaveRaiinKbnInfListResponse> Result { get; private set; } = default!;

        public void Complete(SaveRaiinKbnInfListOutputData outputData)
        {
            Result = new Response<SaveRaiinKbnInfListResponse>()
            {
                Data = new SaveRaiinKbnInfListResponse(outputData.Status == SaveRaiinKbnInfListStatus.Successed),
                Status = (int)outputData.Status
            };
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(SaveRaiinKbnInfListStatus status) => status switch
        {
            SaveRaiinKbnInfListStatus.Successed => ResponseMessage.Success,
            SaveRaiinKbnInfListStatus.Failed => ResponseMessage.Failed,
            SaveRaiinKbnInfListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveRaiinKbnInfListStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            SaveRaiinKbnInfListStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            SaveRaiinKbnInfListStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            SaveRaiinKbnInfListStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
            SaveRaiinKbnInfListStatus.InvalidKbnInf => ResponseMessage.RaiinKubunInvalidKbnInf,
            _ => string.Empty
        };
    }
}
