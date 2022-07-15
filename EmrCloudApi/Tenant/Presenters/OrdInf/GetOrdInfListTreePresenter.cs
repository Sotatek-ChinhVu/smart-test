using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.OrdInfs;
using EmrCloudApi.Tenant.Responses.User;
using Microsoft.AspNetCore.Mvc;
using UseCase.OrdInfs.GetListTrees;
using UseCase.User.GetList;

namespace EmrCloudApi.Tenant.Presenters.User
{
    public class GetOrdInfListTreePresenter : IGetOrdInfListTreeOutputPort
    {
        public Response<GetOrdInfListTreeResponse> Result { get; private set; } = new Response<GetOrdInfListTreeResponse>();
        
        public void Complete(GetOrdInfListTreeOutputData outputData)
        {
            Result.Data = new GetOrdInfListTreeResponse
            {
                GroupHokenItems = outputData.GroupHokens
            };
        }
    }
}
