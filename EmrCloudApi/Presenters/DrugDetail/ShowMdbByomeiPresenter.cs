using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.DrugDetail;
using UseCase.DrugDetailData.ShowMdbByomei;

namespace EmrCloudApi.Presenters.DrugDetailData
{
    public class ShowMdbByomeiPresenter : IShowMdbByomeiOutputPort
    {
        public Response<ShowDrugDetailHtmlResponse> Result { get; private set; } = new Response<ShowDrugDetailHtmlResponse>();

        public void Complete(ShowMdbByomeiOutputData outputData)
        {
            Result = new Response<ShowDrugDetailHtmlResponse>()
            {
                Data = new ShowDrugDetailHtmlResponse(outputData.HtmlData),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case ShowMdbByomeiStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case ShowMdbByomeiStatus.InvalidLevel:
                    Result.Message = ResponseMessage.InvalidLevel;
                    break;
                case ShowMdbByomeiStatus.InvalidSelectedIndexOfMenuLevel:
                    Result.Message = ResponseMessage.DrugMenuInvalidIndexMenu;
                    break;
                case ShowMdbByomeiStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
