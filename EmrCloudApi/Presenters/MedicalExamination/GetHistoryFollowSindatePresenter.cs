using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetHistoryFollowSindate;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetHistoryFollowSindatePresenter : IGetHistoryFollowSindateOutputPort
    {
        public Response<GetHistoryFollowSindateResponse> Result { get; private set; } = new();
        public void Complete(GetHistoryFollowSindateOutputData outputData)
        {
            Result = new Response<GetHistoryFollowSindateResponse>()
            {
                Data = new GetHistoryFollowSindateResponse(outputData.RaiinfList),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetHistoryFollowSindateStatus.NoData:
                    Result.Message = ResponseMessage.GetMedicalExaminationNoData;
                    break;
                case GetHistoryFollowSindateStatus.Successed:
                    Result.Message = ResponseMessage.GetMedicalExaminationSuccessed;
                    break;
                case GetHistoryFollowSindateStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}