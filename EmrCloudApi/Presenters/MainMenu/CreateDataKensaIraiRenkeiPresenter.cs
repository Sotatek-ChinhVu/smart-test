using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Responses.MainMenu.Dto;
using UseCase.MainMenu.CreateDataKensaIraiRenkei;

namespace EmrCloudApi.Presenters.MainMenu;

public class CreateDataKensaIraiRenkeiPresenter : ICreateDataKensaIraiRenkeiOutputPort
{
    public Response<CreateDataKensaIraiRenkeiResponse> Result { get; private set; } = new();

    public void Complete(CreateDataKensaIraiRenkeiOutputData output)
    {
        Result.Data = new CreateDataKensaIraiRenkeiResponse(output.Status == CreateDataKensaIraiRenkeiStatus.Successed, output.KensaIraiList.Select(item => new KensaIraiDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(CreateDataKensaIraiRenkeiStatus status) => status switch
    {
        CreateDataKensaIraiRenkeiStatus.Successed => ResponseMessage.Success,
        CreateDataKensaIraiRenkeiStatus.Failed => ResponseMessage.Failed,
        CreateDataKensaIraiRenkeiStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        CreateDataKensaIraiRenkeiStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
        CreateDataKensaIraiRenkeiStatus.InvalidCenterCd => ResponseMessage.InvalidCenterCd,
        _ => string.Empty
    };
}
