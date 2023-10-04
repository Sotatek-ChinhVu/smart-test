using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetByomeiByCode;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetByomeiByCodePresenter : IGetByomeiByCodeOutputPort
    {
        public Response<GetByomeiByCodeResponse> Result { get; private set; } = default!;

        public void Complete(GetByomeiByCodeOutputData outputData)
        {
            Result = new Response<GetByomeiByCodeResponse>
            {
                Data = new GetByomeiByCodeResponse(outputData.Items),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetByomeiByCodeStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetByomeiByCodeStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case GetByomeiByCodeStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
