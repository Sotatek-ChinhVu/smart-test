using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetCheckDisease;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetCheckDiseasePresenter : IGetCheckDiseaseOutputPort
    {
        public Response<GetCheckDiseaseResponse> Result { get; private set; } = default!;

        public void Complete(GetCheckDiseaseOutputData outputData)
        {

            Result = new Response<GetCheckDiseaseResponse>()
            {
                Data = new GetCheckDiseaseResponse(outputData.CheckDiseaseItemOutputDatas),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case GetCheckDiseaseStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetCheckDiseaseStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetCheckDiseaseStatus.InvalidDrugOrByomei:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidDrugOrByomei;
                    break;
                case GetCheckDiseaseStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetCheckDiseaseStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetCheckDiseaseStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
