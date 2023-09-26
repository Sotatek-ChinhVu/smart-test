using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetListByomeiSetGenerationMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetListByomeiSetGenerationMstPresenter : IGetListByomeiSetGenerationMstOutputPort
    {
        public Response<GetListByomeiSetGenerationMstResponse> Result { get; private set; } = default!;

        public void Complete(GetListByomeiSetGenerationMstOutputData outputData)
        {
            Result = new Response<GetListByomeiSetGenerationMstResponse>
            {
                Data = new GetListByomeiSetGenerationMstResponse()
                {
                    Data = outputData.ByomeiSetGenerationMsts
                },
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetListByomeiSetGenerationMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;

                case GetListByomeiSetGenerationMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;

                case GetListByomeiSetGenerationMstStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
