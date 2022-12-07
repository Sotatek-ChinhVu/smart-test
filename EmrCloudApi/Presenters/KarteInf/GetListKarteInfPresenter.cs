using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.KarteInf;
using UseCase.KarteInf.GetList;
using UseCase.KarteInfs.GetLists;

namespace EmrCloudApi.Presenters.KarteInfs;

public class GetListKarteInfPresenter : IGetListKarteInfOutputPort
{
    public Response<GetListKarteInfResponse> Result { get; private set; } = default!;

    public void Complete(GetListKarteInfOutputData outputData)
    {
        Result = new Response<GetListKarteInfResponse>()
        {
            Data = new GetListKarteInfResponse(outputData.KarteInfs, outputData.ListKarteFile),
            Status = (byte)outputData.Status
        };
        switch (outputData.Status)
        {
            case GetListKarteInfStatus.InvalidRaiinNo:
                Result.Message = ResponseMessage.InvalidRaiinNo;
                break;
            case GetListKarteInfStatus.InvalidPtId:
                Result.Message = ResponseMessage.InvalidPtId;
                break;
            case GetListKarteInfStatus.InvalidSinDate:
                Result.Message = ResponseMessage.InvalidSinDate;
                break;
            case GetListKarteInfStatus.NoData:
                Result.Message = ResponseMessage.NoData;
                break;
            case GetListKarteInfStatus.Successed:
                Result.Message = ResponseMessage.Success;
                break;
        }
    }
}
