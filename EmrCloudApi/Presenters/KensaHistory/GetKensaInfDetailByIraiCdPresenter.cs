using EmrCloudApi.Constants;
using EmrCloudApi.Responses.KensaHistory;
using EmrCloudApi.Responses;
using UseCase.KensaHistory.GetListKensaInfDetail;
using UseCase.MstItem.SearchPostCode;
using UseCase.KensaHistory.GetListKensaCmtMst.GetKensaInfDetailByIraiCd;

namespace EmrCloudApi.Presenters.KensaHistory
{
    public class GetKensaInfDetailByIraiCdPresenter
    {
        public Response<GetKensaInfDetailByIraiCdResponse> Result { get; private set; } = new Response<GetKensaInfDetailByIraiCdResponse>();

        public void Complete(GetKensaInfDetailByIraiCdOutputData outputData)
        {
            Result = new Response<GetKensaInfDetailByIraiCdResponse>
            {
                Data = new GetKensaInfDetailByIraiCdResponse(outputData.KensaInfDetails),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchPostCodeStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SearchPostCodeStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SearchPostCodeStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
            }
        }
    }
}
