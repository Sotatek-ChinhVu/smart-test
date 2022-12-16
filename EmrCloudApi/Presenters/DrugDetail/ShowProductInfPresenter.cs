using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.DrugDetail;
using UseCase.DrugDetailData.ShowProductInf;

namespace EmrCloudApi.Presenters.DrugDetailData
{
    public class ShowProductInfPresenter : IShowProductInfOutputPort
    {
        public Response<ShowDrugDetailHtmlResponse> Result { get; private set; } = new Response<ShowDrugDetailHtmlResponse>();

        public void Complete(ShowProductInfOutputData outputData)
        {
            Result = new Response<ShowDrugDetailHtmlResponse>()
            {
                Data = new ShowDrugDetailHtmlResponse(outputData.HtmlData),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case ShowProductInfStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case ShowProductInfStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case ShowProductInfStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ShowProductInfStatus.InvalidLevel:
                    Result.Message = ResponseMessage.InvalidLevel;
                    break;
                case ShowProductInfStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
