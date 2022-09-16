using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MonshinInfor;
using UseCase.MonshinInfor.GetList;

namespace EmrCloudApi.Tenant.Presenters.MonshinInf
{
    public class GetMonshinInforListPresenter : IGetMonshinInforListOutputPort
    {
        public Response<GetMonshinInforListResponse> Result { get; private set; } = new Response<GetMonshinInforListResponse>();

        public void Complete(GetMonshinInforListOutputData outputData)
        {
            Result.Data = new GetMonshinInforListResponse(outputData.MonshinInforModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetMonshinInforListStatus status) => status switch
        {
            GetMonshinInforListStatus.Success => ResponseMessage.Success,
            GetMonshinInforListStatus.InvalidData => ResponseMessage.InvalidKeyword,
            GetMonshinInforListStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
