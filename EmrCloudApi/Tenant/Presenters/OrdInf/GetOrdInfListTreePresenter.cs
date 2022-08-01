using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination.OrdInfs;
using UseCase.MedicalExamination.OrdInfs.GetListTrees;

namespace EmrCloudApi.Tenant.Presenters.OrdInfs
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
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    Result.Data.GroupHokenItems = new List<GroupHokenItem>();
                    break;
                case GetOrdInfListTreeStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    Result.Data.GroupHokenItems = new List<GroupHokenItem>();
                    break;
                case GetOrdInfListTreeStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    Result.Data.GroupHokenItems = new List<GroupHokenItem>();
                    break;
                case GetOrdInfListTreeStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    Result.Data.GroupHokenItems = new List<GroupHokenItem>();
                    break;
                case GetOrdInfListTreeStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    Result.Data.GroupHokenItems = new List<GroupHokenItem>();
                    break;
                case GetOrdInfListTreeStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    Result.Data.GroupHokenItems = outputData.GroupHokens;
                    break;
            }

        }
    }
}
