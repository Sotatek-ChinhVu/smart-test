using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UketukeSbt;
using EmrCloudApi.Responses.User;
using Helper.Constants;
using UseCase.FlowSheet.Upsert;
using UseCase.UketukeSbtMst.GetNext;
using UseCase.UketukeSbtMst.Upsert;
using UseCase.User.UpsertList;

namespace EmrCloudApi.Presenters.UketukeSbt;

public class UpsertUketukeSbtMstListPresenter : IUpsertUketukeSbtMstOutputPort
{
    public Response<UpsertUketukeSbtMstListResponse> Result { get; private set; } = default!;

    public void Complete(UpsertUketukeSbtMstOutputData outputData)
    {
        Result = new Response<UpsertUketukeSbtMstListResponse>()
        {
            Data = new UpsertUketukeSbtMstListResponse(outputData.Status == UpsertUketukeSbtMstStatus.Success),
            Message = GetMessage(outputData.Status),
            Status = (int)outputData.Status
        };
    }
    private static string GetMessage(UpsertUketukeSbtMstStatus status) => status switch
    {
        UpsertUketukeSbtMstStatus.Success => ResponseMessage.Success,
        UpsertUketukeSbtMstStatus.Failed => ResponseMessage.Failed,
        UpsertUketukeSbtMstStatus.InvalidKbnId => ResponseMessage.InvalidKbnId,
        UpsertUketukeSbtMstStatus.InvalidKbnName => ResponseMessage.InvalidKbnName,
        UpsertUketukeSbtMstStatus.InputNoData => ResponseMessage.InputNoData,
        UpsertUketukeSbtMstStatus.InvalidSortNo => ResponseMessage.InvalidSortNo,
        UpsertUketukeSbtMstStatus.InvalidIsDeleted => ResponseMessage.InvalidIsDeleted,
        UpsertUketukeSbtMstStatus.UketukeListExistedInputData => ResponseMessage.UketukeListExistedInputData,
        UpsertUketukeSbtMstStatus.UketukeListInvalidExistedKbnId => ResponseMessage.UketukeListInvalidExistedKbnId,
        _ => string.Empty
    };
}
