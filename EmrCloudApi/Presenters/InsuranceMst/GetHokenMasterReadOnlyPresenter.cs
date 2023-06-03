using EmrCloudApi.Constants;
using EmrCloudApi.Responses.InsuranceMst;
using EmrCloudApi.Responses;
using UseCase.InsuranceMst.GetHokenMasterReadOnly;

namespace EmrCloudApi.Presenters.InsuranceMst
{
    public class GetHokenMasterReadOnlyPresenter : IGetHokenMasterReadOnlyOutputPort
    {
        public Response<GetHokenMasterReadOnlyResponse> Result { get; private set; } = default!;

        public void Complete(GetHokenMasterReadOnlyOutputData outputData)
        {
            Result = new Response<GetHokenMasterReadOnlyResponse>()
            {

                Data = new GetHokenMasterReadOnlyResponse(outputData.HokenMaster),
                Status = (int)outputData.Status,
            };
            switch (outputData.Status)
            {

                case GetHokenMasterReadOnlyStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetHokenMasterReadOnlyStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
            }
        }
    }
}
