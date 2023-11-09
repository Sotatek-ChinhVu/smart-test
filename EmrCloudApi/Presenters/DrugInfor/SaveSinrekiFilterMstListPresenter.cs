using EmrCloudApi.Constants;
using EmrCloudApi.Responses.DrugInfor.Dto;
using EmrCloudApi.Responses.DrugInfor;
using EmrCloudApi.Responses;
using UseCase.DrugInfor.SaveSinrekiFilterMstList;

namespace EmrCloudApi.Presenters.DrugInfor;

public class SaveSinrekiFilterMstListPresenter : ISaveSinrekiFilterMstListOutputPort
{
    public Response<SaveSinrekiFilterMstListResponse> Result { get; private set; } = new();

    public void Complete(SaveSinrekiFilterMstListOutputData output)
    {
        Result.Data = new SaveSinrekiFilterMstListResponse(output.Status == SaveSinrekiFilterMstListStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveSinrekiFilterMstListStatus status) => status switch
    {
        SaveSinrekiFilterMstListStatus.Successed => ResponseMessage.Success,
        SaveSinrekiFilterMstListStatus.Failed => ResponseMessage.Failed,
        SaveSinrekiFilterMstListStatus.InvalidItemCd => ResponseMessage.InvalidItemCd,
        SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstName => ResponseMessage.InvalidSinrekiFilterMstName,
        SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstGrpCd => ResponseMessage.InvalidSinrekiFilterMstGrpCd,
        SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstDetailId => ResponseMessage.InvalidSinrekiFilterMstDetailId,
        SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstKouiKbnId => ResponseMessage.InvalidSinrekiFilterMstKouiKbnId,
        SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstKouiSeqNo => ResponseMessage.InvalidSinrekiFilterMstKouiSeqNo,
        SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstDetaiDuplicateItemCd => ResponseMessage.InvalidSinrekiFilterMstDetaiDuplicateItemCd,
        _ => string.Empty
    };
}
