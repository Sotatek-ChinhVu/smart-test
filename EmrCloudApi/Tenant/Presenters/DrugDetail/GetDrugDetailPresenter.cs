using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.DrugDetail;
using UseCase.DrugDetail;

namespace EmrCloudApi.Tenant.Presenters.DrugDetail
{
    public class GetDrugDetailPresenter : IGetDrugDetailOutputPort
    {
        public Response<GetDrugDetailResponse> Result { get; private set; } = new Response<GetDrugDetailResponse>();

        public void Complete(GetDrugDetailOutputData outputData)
        {
            Result = new Response<GetDrugDetailResponse>()
            {
                Data = new GetDrugDetailResponse(outputData.DrugMenu),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetDrugDetailStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetDrugDetailStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetDrugDetailStatus.InValidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetDrugDetailStatus.InValidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
            }
        }
    }
}
