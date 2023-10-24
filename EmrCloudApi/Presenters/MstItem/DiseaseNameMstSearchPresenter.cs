using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem.DiseaseNameMstSearch;
using UseCase.MstItem.DiseaseNameMstSearch;

namespace EmrCloudApi.Presenters.MstItem
{
    public class DiseaseNameMstSearchPresenter : IDiseaseNameMstSearchOutputPort
    {
        public Response<DiseaseNameMstSearchResponse> Result { get; private set; } = new();

        public void Complete(DiseaseNameMstSearchOutputData output)
        {
            Result.Data = new DiseaseNameMstSearchResponse(output.ListData.Select(item => new DiseaseNameMstSearchModel(item)).ToList());
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(DiseaseNameMstSearchStatus status) => status switch
        {
            DiseaseNameMstSearchStatus.Successful => ResponseMessage.Success,
            DiseaseNameMstSearchStatus.Failed => ResponseMessage.Failed,
            DiseaseNameMstSearchStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            _ => string.Empty
        };
    }
}
