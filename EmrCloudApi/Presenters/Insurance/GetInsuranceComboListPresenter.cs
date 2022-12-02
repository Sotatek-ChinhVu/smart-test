using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.InsuranceList;
using UseCase.Insurance.GetComboList;

namespace EmrCloudApi.Presenters.InsuranceList
{
    public class GetInsuranceComboListPresenter : IGetInsuranceComboListOutputPort
    {
        public Response<GetInsuranceComboListResponse> Result { get; private set; } = default!;
        public void Complete(GetInsuranceComboListOutputData output)
        {
            Result = new Response<GetInsuranceComboListResponse>()
            {

                Data = new GetInsuranceComboListResponse(output.Data),
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {

                case GetInsuranceComboListStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetInsuranceComboListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetInsuranceComboListStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetInsuranceComboListStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetInsuranceComboListStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}