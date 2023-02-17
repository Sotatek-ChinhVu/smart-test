using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Diseases;
using EmrCloudApi.Responses;
using UseCase.Diseases.GetSetByomeiTree;

namespace EmrCloudApi.Presenters.Diseases
{
    public class GetSetByomeiTreePresenter : IGetSetByomeiTreeOutputPort
    {
        public Response<GetSetByomeiTreeResponse> Result { get; private set; } = new Response<GetSetByomeiTreeResponse>();

        public void Complete(GetSetByomeiTreeOutputData outputData)
        {
            Result = new Response<GetSetByomeiTreeResponse>()
            {
                Data = new GetSetByomeiTreeResponse(outputData.Datas),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSetByomeiTreeStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetSetByomeiTreeStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetSetByomeiTreeStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetSetByomeiTreeStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
            }
        }
    }
}
