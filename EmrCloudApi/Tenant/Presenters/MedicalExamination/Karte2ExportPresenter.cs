using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using UseCase.MedicalExamination.Karte2Print;

namespace EmrCloudApi.Tenant.Presenters.MedicalExamination
{
    public class Karte2ExportPresenter : IKarte2ExportOutputPort
    {
        public Response<Karte2ExportResponse> Result { get; private set; } = default!;

        public void Complete(Karte2ExportOutputData outputData)
        {
            Result = new Response<Karte2ExportResponse>()
            {
                Data = new Karte2ExportResponse(outputData.Url),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case Karte2PrintStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case Karte2PrintStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case Karte2PrintStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case Karte2PrintStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case Karte2PrintStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case Karte2PrintStatus.InvalidDeleteCondition:
                    Result.Message = ResponseMessage.InvalidDeleteCondition;
                    break;
                case Karte2PrintStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case Karte2PrintStatus.InvalidUser:
                    Result.Message = ResponseMessage.InvalidUser;
                    break;
                case Karte2PrintStatus.InvalidUrl:
                    Result.Message = ResponseMessage.InvalidUrl;
                    break;
            }
        }
    }
}
