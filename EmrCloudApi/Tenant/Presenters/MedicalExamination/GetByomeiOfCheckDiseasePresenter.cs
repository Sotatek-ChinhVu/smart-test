using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetByomeiFollowItemCd;

namespace EmrCloudApi.Tenant.Presenters.MedicalExamination
{
    public class GetByomeiOfCheckDiseasePresenter : IGetByomeiFollowItemCdOutputPort
    {
        public Response<GetByomeiOfCheckDiseaseResponse> Result { get; private set; } = default!;

        public void Complete(GetByomeiFollowItemCdOutputData outputData)
        {

            Result = new Response<GetByomeiOfCheckDiseaseResponse>()
            {
                Data = new GetByomeiOfCheckDiseaseResponse(outputData.Byomeis),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case GetByomeiFollowItemCdStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetByomeiFollowItemCdStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetByomeiFollowItemCdStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case GetByomeiFollowItemCdStatus.InvalidByomeis:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidByomei;
                    break;
                case GetByomeiFollowItemCdStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetByomeiFollowItemCdStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetByomeiFollowItemCdStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
