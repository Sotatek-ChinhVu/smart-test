using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InputItem;
using UseCase.InputItem.GetDosageDrugList;
using UseCase.InputItem.Search;

namespace EmrCloudApi.Tenant.Presenters.InputItem
{
    public class GetDosageDrugListPresenter : IGetDosageDrugListOutputPort
    {
        public Response<GetDosageDrugListResponse> Result { get; private set; } = default!;

        public void Complete(GetDosageDrugListOutputData outputData)
        {
            Result = new Response<GetDosageDrugListResponse>
            {
                Data = new GetDosageDrugListResponse(outputData.ListDosageDrugModel),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetDosageDrugListStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetDosageDrugListStatus.Fail:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case GetDosageDrugListStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetDosageDrugListStatus.InputDataNull:
                    Result.Message = ResponseMessage.InputDataNull;
                    break;
            }
        }
    }
}
