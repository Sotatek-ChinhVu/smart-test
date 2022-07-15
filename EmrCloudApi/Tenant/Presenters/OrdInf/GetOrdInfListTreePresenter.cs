using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.OrdInfs;
using UseCase.OrdInfs.GetListTrees;

namespace EmrCloudApi.Tenant.Presenters.User
{
    public class GetOrdInfListTreePresenter : IGetOrdInfListTreeOutputPort
    {
        public Response<GetOrdInfListTreeResponse> Result { get; private set; } = default!;

        public void Complete(GetOrdInfListTreeOutputData outputData)
        {
            Result = new Response<GetOrdInfListTreeResponse>()
            {
                Data = new GetOrdInfListTreeResponse(),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetOrdInfListTreeStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.GetOrdInfInvalidRaiinNo;
                    Result.Data.GroupHokenItems = new List<GroupHokenItem>();
                    break;
                case GetOrdInfListTreeStatus.InvalidHpId:
                    Result.Message = ResponseMessage.GetOrdInfInvalidHpId;
                    Result.Data.GroupHokenItems = new List<GroupHokenItem>();
                    break;
                case GetOrdInfListTreeStatus.InvalidPtId:
                    Result.Message = ResponseMessage.GetOrdInfInvalidPtId;
                    Result.Data.GroupHokenItems = new List<GroupHokenItem>();
                    break;
                case GetOrdInfListTreeStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetOrdInfInvalidSinDate;
                    Result.Data.GroupHokenItems = new List<GroupHokenItem>();
                    break;
                case GetOrdInfListTreeStatus.NoData:
                    Result.Message = ResponseMessage.GetOrdInfNoData;
                    Result.Data.GroupHokenItems = new List<GroupHokenItem>();
                    break;
                case GetOrdInfListTreeStatus.Successed:
                    Result.Message = ResponseMessage.GetOrdInfSuccessed;
                    Result.Data.GroupHokenItems = outputData.GroupHokens;
                    break;
            }

        }
    }
}
