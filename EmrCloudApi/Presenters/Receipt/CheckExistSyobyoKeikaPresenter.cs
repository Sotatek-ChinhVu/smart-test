using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Receipt;
using EmrCloudApi.Responses;
using UseCase.Receipt.CheckExistSyobyoKeika;

namespace EmrCloudApi.Presenters.Receipt;

public class CheckExistSyobyoKeikaPresenter : ICheckExistSyobyoKeikaOutputPort
{
    public Response<CheckExistSyobyoKeikaResponse> Result { get; private set; } = new();

    public void Complete(CheckExistSyobyoKeikaOutputData outputData)
    {
        Result.Data = new CheckExistSyobyoKeikaResponse(outputData.Status == CheckExistSyobyoKeikaStatus.IsExisted);
        Result.Message = ResponseMessage.Success;
        Result.Status = (int)outputData.Status;
    }
}
