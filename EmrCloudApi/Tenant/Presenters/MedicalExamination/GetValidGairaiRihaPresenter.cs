using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetValidGairaiRiha;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetValidGairaiRihaPresenter : IGetValidGairaiRihaOutputPort
    {
        public Response<GetValidGairaiRihaResponse> Result { get; private set; } = default!;

        public void Complete(GetValidGairaiRihaOutputData outputData)
        {
            Result = new Response<GetValidGairaiRihaResponse>()
            {
                Data = new GetValidGairaiRihaResponse(outputData.Type, outputData.ItemName, outputData.LastDaySanteiRiha, outputData.RihaItemName),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetValidGairaiRihaStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetValidGairaiRihaStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetValidGairaiRihaStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetValidGairaiRihaStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case GetValidGairaiRihaStatus.InvalidSyosaiKbn:
                    Result.Message = ResponseMessage.RaiinInfTodayOdrInvalidSyosaiKbn;
                    break;
                case GetValidGairaiRihaStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetValidGairaiRihaStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
