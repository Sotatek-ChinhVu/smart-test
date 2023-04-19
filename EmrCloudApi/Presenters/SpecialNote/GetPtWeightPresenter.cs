using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SpecialNote;
using UseCase.SpecialNote.GetPtWeight;

namespace EmrCloudApi.Presenters.SpecialNote
{
    public class GetPtWeightPresenter : IGetPtWeightOutputPort
    {
        public Response<GetPtWeightResponse> Result { get; private set; } = default!;

        public void Complete(GetPtWeightOutputData outputData)
        {
            Result = new Response<GetPtWeightResponse>()
            {
                Data = new GetPtWeightResponse(outputData.HpId, outputData.PtId, outputData.IraiCd, outputData.SeqNo, outputData.IraiDate, outputData.RaiinNo, outputData.KensaItemCd, outputData.ResultVal, outputData.ResultType, outputData.AbnormalKbn, outputData.IsDeleted, outputData.CmtCd1, outputData.CmtCd2, outputData.UpdateDate),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetPtWeightStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetPtWeightStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;

                case GetPtWeightStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
