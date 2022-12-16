using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.DrugDetail;
using UseCase.DrugDetailData.ShowKanjaMuke;

namespace EmrCloudApi.Presenters.DrugDetailData
{
    public class ShowKanjaMukePresenter : IShowKanjaMukeOutputPort
    {
        public Response<ShowDrugDetailHtmlResponse> Result { get; private set; } = new Response<ShowDrugDetailHtmlResponse>();

        public void Complete(ShowKanjaMukeOutputData outputData)
        {
            Result = new Response<ShowDrugDetailHtmlResponse>()
            {
                Data = new ShowDrugDetailHtmlResponse(outputData.HtmlData),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case ShowKanjaMukeStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case ShowKanjaMukeStatus.InvalidLevel:
                    Result.Message = ResponseMessage.InvalidLevel;
                    break;
                case ShowKanjaMukeStatus.InvalidSelectedIndexOfMenuLevel:
                    Result.Message = ResponseMessage.DrugMenuInvalidIndexMenu;
                    break;
                case ShowKanjaMukeStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
