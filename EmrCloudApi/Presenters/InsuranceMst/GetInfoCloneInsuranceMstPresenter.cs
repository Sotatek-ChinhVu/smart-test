using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.InsuranceMst;
using UseCase.InsuranceMst.GetInfoCloneInsuranceMst;

namespace EmrCloudApi.Presenters.InsuranceMst
{
    public class GetInfoCloneInsuranceMstPresenter : IGetInfoCloneInsuranceMstOutputPort
    {
        public Response<GetInfoCloneInsuranceMstResponse> Result { get; private set; } = default!;

        public void Complete(GetInfoCloneInsuranceMstOutputData outputData)
        {
            Result = new Response<GetInfoCloneInsuranceMstResponse>()
            {

                Data = new GetInfoCloneInsuranceMstResponse(outputData.HokenEdaNo, outputData.SortNo),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {

                case GetInfoCloneInsuranceMstStatus.InvalidPrefNo:
                    Result.Message = ResponseMessage.InvalidPrefNo;
                    break;
                case GetInfoCloneInsuranceMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetInfoCloneInsuranceMstStatus.InvalidHokenNo:
                    Result.Message = ResponseMessage.InvalidHokenNo;
                    break;
                case GetInfoCloneInsuranceMstStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
