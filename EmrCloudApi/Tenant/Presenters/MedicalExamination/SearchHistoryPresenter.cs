using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.SearchHistory;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class SearchHistoryPresenter : ISearchHistoryOutputPort
    {
        public Response<SearchHistoryResponse> Result { get; private set; } = default!;

        public void Complete(SearchHistoryOutputData outputData)
        {
            Result = new Response<SearchHistoryResponse>()
            {
                Data = new SearchHistoryResponse(outputData.ReceptionItem, outputData.SearchIndex),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchHistoryStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case SearchHistoryStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SearchHistoryStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case SearchHistoryStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case SearchHistoryStatus.InvalidCurrentIndex:
                    Result.Message = ResponseMessage.InvalidCurrentIndex;
                    break;
                case SearchHistoryStatus.InvalidFilterId:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidFilterId;
                    break;
                case SearchHistoryStatus.InvalidIsDeleted:
                    Result.Message = ResponseMessage.InvalidIsDeleted;
                    break;
                case SearchHistoryStatus.InvalidSearchType:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidSearchType;
                    break;
                case SearchHistoryStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case SearchHistoryStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
