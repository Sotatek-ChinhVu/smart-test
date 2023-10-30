using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.CheckJihiSbtExistsInTenMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class CheckJihiSbtExistsInTenMstPresenter : ICheckJihiSbtExistsInTenMstOutputPort
    {
        public Response<CheckJihiSbtExistsInTenMstResponse> Result { get; private set; } = new();

        public void Complete(CheckJihiSbtExistsInTenMstOutputData output)
        {
            Result.Data = new CheckJihiSbtExistsInTenMstResponse(output.Status);
            Result.Message = GetMessage(output.Status);
            Result.Status = output.Status == true ? 1 : 0;
        }

        private string GetMessage(bool status) => status switch
        {
            true => ResponseMessage.Success,
            false => ResponseMessage.Failed
        };
    }
}
