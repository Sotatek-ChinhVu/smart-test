using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem.DiseaseNameMstSearch;
using EmrCloudApi.Responses;
using UseCase.MstItem.DiseaseNameMstSearch;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.CompareTenMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class CompareTenMstPresenter : ICompareTenMstOutputPort
    {
        public Response<CompareTenMstResponse> Result { get; private set; } = new();

        public void Complete(CompareTenMstOutputData output)
        {
            Result.Data = new CompareTenMstResponse(output.ListData);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(CompareTenMstStatus status) => status switch
        {
            CompareTenMstStatus.Success => ResponseMessage.Success,
            CompareTenMstStatus.Faild => ResponseMessage.Failed,
            CompareTenMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            CompareTenMstStatus.InvalidSindate => ResponseMessage.InvalidSinDate,
            _ => string.Empty
        };
    }
}
