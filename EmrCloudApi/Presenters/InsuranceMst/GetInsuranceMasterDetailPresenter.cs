using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.InsuranceMst;
using UseCase.InsuranceMst.GetMasterDetails;

namespace EmrCloudApi.Presenters.InsuranceMst
{
    public class GetInsuranceMasterDetailPresenter : IGetInsuranceMasterDetailOutputPort
    {
        public Response<GetInsuranceMasterDetailResponse> Result { get; private set; } = default!;
        public void Complete(GetInsuranceMasterDetailOutputData output)
        {
            Result = new Response<GetInsuranceMasterDetailResponse>()
            {
                Data = new GetInsuranceMasterDetailResponse(
                    output.InsuranceMstData.Select(x=> new InsuranceMasterDto(x.Master, x.Details.Select(m=> new InsuranceDetailDto(m.HokenEdaNo, m.SortNo, m.HokenSName, m.PrefNo, m.HokenEdaNo, m.StartDate))))),
                Status = (int)output.Status
            };
            switch (output.Status)
            {

                case GetInsuranceMasterDetailStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetInsuranceMasterDetailStatus.InvalidFHokenNo:
                    Result.Message = ResponseMessage.InvalidFHokenNo;
                    break;
                case GetInsuranceMasterDetailStatus.InvalidFHokenSbtKbn:
                    Result.Message = ResponseMessage.InvalidFHokenSbtKbn;
                    break;
                case GetInsuranceMasterDetailStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetInsuranceMasterDetailStatus.DataNotFound:
                    Result.Message = ResponseMessage.NotFound;
                    break;
            }
        }
    }
}
