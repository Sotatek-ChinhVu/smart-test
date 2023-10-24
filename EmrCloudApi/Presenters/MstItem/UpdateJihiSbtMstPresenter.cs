using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.UpdateJihiSbtMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class UpdateJihiSbtMstPresenter
    {
        public Response<UpdateJihiSbtMstResponse> Result { get; private set; } = default!;
        public void Complete(UpdateJihiSbtMstOutputData outputData)
        {
            Result = new Response<UpdateJihiSbtMstResponse>
            {
                Status = outputData.StatusUpdate == true ? 1 : 0
            };
            switch (outputData.StatusUpdate)
            {
                case true:
                    Result.Message = ResponseMessage.Success;
                    break;
                case false:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
