using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.ConvertInputItemToTodayOdr;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class ConvertInputItemToTodayOrdPresenter : IConvertInputItemToTodayOrdOutputPort
    {
        public Response<ConvertInputItemToTodayOrdResponse> Result { get; private set; } = default!;

        public void Complete(ConvertInputItemToTodayOrdOutputData outputData)
        {

            Result = new Response<ConvertInputItemToTodayOrdResponse>()
            {
                Data = new ConvertInputItemToTodayOrdResponse(outputData.YakkaOfOdrDetails),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case ConvertInputItemToTodayOrdStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case ConvertInputItemToTodayOrdStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ConvertInputItemToTodayOrdStatus.InvalidDetailInfs:
                    Result.Message = ResponseMessage.InvalidDetailInfs;
                    break;
            }
        }
    }
}
