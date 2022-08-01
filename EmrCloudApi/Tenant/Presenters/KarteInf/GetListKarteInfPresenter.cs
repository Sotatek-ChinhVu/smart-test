using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KarteInfs;
using UseCase.KarteInf.GetList;
using UseCase.KarteInfs.GetLists;

namespace EmrCloudApi.Tenant.Presenters.KarteInfs
{
    public class GetListKarteInfPresenter : IGetListKarteInfOutputPort
    {
        public Response<GetListKarteInfResponse> Result { get; private set; } = default!;

        public void Complete(GetListKarteInfOutputData outputData)
        {
            Result = new Response<GetListKarteInfResponse>()
            {
                Data = new GetListKarteInfResponse(outputData.KarteInfs),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetListKarteInfStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.GetReceptionInvalidRaiinNo;
                    break;
                case GetListKarteInfStatus.InvalidPtId:
                    Result.Message = ResponseMessage.GetKarteInfInvalidPtId;
                    break;
                case GetListKarteInfStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetKarteInfInvalidSinDate;
                    break;
                case GetListKarteInfStatus.NoData:
                    Result.Message = ResponseMessage.GetKarteInfNoData;
                    break;
                case GetListKarteInfStatus.Successed:
                    Result.Message = ResponseMessage.GetKarteInfSuccessed;
                    break;
            }
        }
    }
}
