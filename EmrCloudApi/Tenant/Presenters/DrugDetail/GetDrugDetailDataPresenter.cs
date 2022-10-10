using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.DrugDetail;
using UseCase.DrugDetailData;


namespace EmrCloudApi.Tenant.Presenters.DrugDetailData
{
    public class GetDrugDetailDataPresenter : IGetDrugDetailDataOutputPort
    {
        public Response<GetDrugDetailDataResponse> Result { get; private set; } = new Response<GetDrugDetailDataResponse>();

        public void Complete(GetDrugDetailDataOutputData outputData)
        {
            Result = new Response<GetDrugDetailDataResponse>()
            {
                Data = new GetDrugDetailDataResponse(outputData.Data),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case GetDrugDetailDataStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetDrugDetailDataStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case GetDrugDetailDataStatus.InvalidYJCode:
                    Result.Message = ResponseMessage.InvalidYJCode;
                    break;
                case GetDrugDetailDataStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
