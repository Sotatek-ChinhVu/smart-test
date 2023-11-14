using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.UpdateSingleDoseMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class UpdateSingleDoseMstPresenter : IUpdateSingleDoseMstOutputPort
    {
        public Response<UpdateSingleDoseMstResponse> Result { get; private set; } = default!;
        public void Complete(UpdateSingleDoseMstOutputData outputData)
        {
            Result = new Response<UpdateSingleDoseMstResponse>
            {
                Status = outputData.StatusUpdate ? 1 : 0
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
