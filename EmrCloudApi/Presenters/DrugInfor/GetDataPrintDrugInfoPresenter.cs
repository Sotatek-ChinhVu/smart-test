using EmrCloudApi.Responses;
using EmrCloudApi.Responses.DrugInfor;
using UseCase.DrugInfor.GetDataPrintDrugInfo;

namespace EmrCloudApi.Presenters.DrugInfor;

public class GetDataPrintDrugInfoPresenter : IGetDataPrintDrugInfoOutputPort
{
    public Response<GetDataPrintDrugInfoResponse> Result { get; private set; } = new();

    public void Complete(GetDataPrintDrugInfoOutputData outputData)
    {
        Result.Data = new GetDataPrintDrugInfoResponse(outputData.DrugInfor, outputData.HtmlData, outputData.DrugType);
    }
}
