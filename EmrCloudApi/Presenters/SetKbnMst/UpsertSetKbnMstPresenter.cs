using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetKbnMst;
using UseCase.SetKbnMst.Upsert;

namespace EmrCloudApi.Presenters.SetKbnMst
{
    public class UpsertSetKbnMstPresenter : IUpsertSetKbnMstOutputPort
    {
        public Response<UpsertSetKbnMstResponse> Result { get; private set; } = default!;

        public void Complete(UpsertSetKbnMstOutputData outputData)
        {
            Result = new Response<UpsertSetKbnMstResponse>()
            {
                Data = new UpsertSetKbnMstResponse(outputData.Status == UpsertSetKbnMstStatus.Successed),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case UpsertSetKbnMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case UpsertSetKbnMstStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case UpsertSetKbnMstStatus.InvalidSetKbn:
                    Result.Message = ResponseMessage.GetSetKbnListInvalidSetKbn;
                    break;
                case UpsertSetKbnMstStatus.InvalidSetKbnName:
                    Result.Message = ResponseMessage.SetKbnListInvalidSetKbnName;
                    break;
                case UpsertSetKbnMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case UpsertSetKbnMstStatus.InvalidInputData:
                    Result.Message = ResponseMessage.InputNoData;
                    break;
                case UpsertSetKbnMstStatus.InvalidSetKbnEdaNo:
                    Result.Message = ResponseMessage.InvalidSetKbnEdaNo;
                    break;
                case UpsertSetKbnMstStatus.InvalidKaCd:
                    Result.Message = ResponseMessage.InvalidKaId;
                    break;
                case UpsertSetKbnMstStatus.InvalidDocCd:
                    Result.Message = ResponseMessage.InvalidDocCd;
                    break;
                case UpsertSetKbnMstStatus.InvalidIsDelete:
                    Result.Message = ResponseMessage.InvalidIsDeleted;
                    break;
                case UpsertSetKbnMstStatus.InvalidGenerationId:
                    Result.Message = ResponseMessage.InvalidGenarationId;
                    break;
                case UpsertSetKbnMstStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
