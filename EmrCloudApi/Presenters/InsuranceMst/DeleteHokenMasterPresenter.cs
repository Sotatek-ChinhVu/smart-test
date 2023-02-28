using EmrCloudApi.Constants;
using EmrCloudApi.Responses.InsuranceMst;
using EmrCloudApi.Responses;
using UseCase.InsuranceMst.DeleteHokenMaster;

namespace EmrCloudApi.Presenters.InsuranceMst
{
    public class DeleteHokenMasterPresenter : IDeleteHokenMasterOutputPort
    {
        public Response<DeleteHokenMasterResponse> Result { get; private set; } = default!;

        public void Complete(DeleteHokenMasterOutputData outputData)
        {
            Result = new Response<DeleteHokenMasterResponse>()
            {
                Data = new DeleteHokenMasterResponse(outputData.Status),
                Status = (int)outputData.Status,
                Message = outputData.Message ?? GetMessage(outputData.Status)
            };
        }

        private string GetMessage(DeleteHokenMasterStatus status) => status switch
        {
            DeleteHokenMasterStatus.Successful => ResponseMessage.Success,
            DeleteHokenMasterStatus.Failed => ResponseMessage.Failed,
            DeleteHokenMasterStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            DeleteHokenMasterStatus.InvalidHokenNo => ResponseMessage.InvalidHokenId,
            DeleteHokenMasterStatus.InvalidHokenEdaNo => ResponseMessage.InvalidHokenEdaNo,
            DeleteHokenMasterStatus.InvalidPrefNo => ResponseMessage.InvalidPrefNo,
            DeleteHokenMasterStatus.InvalidStartDate => ResponseMessage.InvalidStarDate,
            _ => string.Empty
        };
    }
}
