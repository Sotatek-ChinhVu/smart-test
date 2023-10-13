using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Responses.MainMenu.Dto;
using UseCase.MainMenu.ImportKensaIrai;

namespace EmrCloudApi.Presenters.MainMenu;

public class ImportKensaIraiPresenter : IImportKensaIraiOutputPort
{
    public Response<ImportKensaIraiResponse> Result { get; private set; } = new();

    public void Complete(ImportKensaIraiOutputData output)
    {
        Result.Data = new ImportKensaIraiResponse(output.KensaInfMessageList.Select(item => new KensaInfMessageDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(ImportKensaIraiStatus status) => status switch
    {
        ImportKensaIraiStatus.Successed => ResponseMessage.Success,
        ImportKensaIraiStatus.Failed => ResponseMessage.Failed,
        ImportKensaIraiStatus.InvalidCenterCd => ResponseMessage.InvalidCenterCd,
        ImportKensaIraiStatus.InvalidIraiCd => ResponseMessage.InvalidIraiCd,
        ImportKensaIraiStatus.InvalidKensaItemCd => ResponseMessage.InvalidKensaItemCd,
        ImportKensaIraiStatus.InvalidResultType => ResponseMessage.InvalidResultType,
        ImportKensaIraiStatus.InvalidAbnormalKbn => ResponseMessage.InvalidAbnormalKbn,
        ImportKensaIraiStatus.InvalidInputFile => ResponseMessage.InvalidInputFile,
        _ => string.Empty
    };
}
