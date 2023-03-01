using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.Get;

namespace EmrCloudApi.Presenters.Reception
{
    public class GetReceptionPresenter : IGetReceptionOutputPort
    {
        public Response<GetReceptionResponse> Result { get; private set; } = default!;

        public void Complete(GetReceptionOutputData outputData)
        {
            Result = new Response<GetReceptionResponse>()
            {
                Data = new GetReceptionResponse(),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetReceptionStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    Result.Data.Reception = null;
                    break;
                case GetReceptionStatus.ReceptionNotExisted:
                    Result.Message = ResponseMessage.NoData;
                    Result.Data.Reception = null;
                    break;
                case GetReceptionStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    Result.Data.Reception = outputData.ReceptionDto;
                    break;
            }
        }
    }
}
