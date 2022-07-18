using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KarteInfs;
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
                Data = new GetListKarteInfResponse(),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetListKarteInfStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.GetReceptionInvalidRaiinNo;
                    Result.Data.KarteInfs = outputData.KarteInfs;
                    break;
                case GetListKarteInfStatus.InvalidPtId:
                    Result.Message = ResponseMessage.GetKarteInfInvalidPtId;
                    Result.Data.KarteInfs = outputData.KarteInfs;
                    break;
                case GetListKarteInfStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetKarteInfInvalidSinDate;
                    Result.Data.KarteInfs = outputData.KarteInfs;
                    break;
                case GetListKarteInfStatus.NoData:
                    Result.Message = ResponseMessage.GetKarteInfNoData;
                    Result.Data.KarteInfs = outputData.KarteInfs;
                    break;
                case GetListKarteInfStatus.Successed:
                    Result.Message = ResponseMessage.GetKarteInfSuccessed;
                    Result.Data.KarteInfs = outputData.KarteInfs;
                    break;
            }
        }
    }
}
