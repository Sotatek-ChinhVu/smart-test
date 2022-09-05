using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Tenant.Presenters.MedicalExamination
{
    public class GetMedicalExaminationHistoryPresenter : IGetMedicalExaminationHistoryOutputPort
    {
        public Response<GetMedicalExaminationHistoryResponse> Result { get; private set; } = default!;

        public void Complete(GetMedicalExaminationHistoryOutputData outputData)
        {
            Result = new Response<GetMedicalExaminationHistoryResponse>()
            {
                Data = new GetMedicalExaminationHistoryResponse(outputData.RaiinfList),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetMedicalExaminationHistoryStatus.InvalidPtId:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidPtId;
                    break;
                case GetMedicalExaminationHistoryStatus.InvalidHpId:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidHpId;
                    break;
                case GetMedicalExaminationHistoryStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidSinDate;
                    break;
                case GetMedicalExaminationHistoryStatus.NoData:
                    Result.Message = ResponseMessage.GetMedicalExaminationNoData;
                    break;
                case GetMedicalExaminationHistoryStatus.Successed:
                    Result.Message = ResponseMessage.GetMedicalExaminationSuccessed;
                    break;
                case GetMedicalExaminationHistoryStatus.InvalidPageIndex:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidPageIndex;
                    break;
                case GetMedicalExaminationHistoryStatus.InvalidPageSize:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidPageSize;
                    break;
                case GetMedicalExaminationHistoryStatus.InvalidDeleteCondition:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidDeleteCondition;
                    break;
            }
        }
    }
}
