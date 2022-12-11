using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;
using UseCase.SuperSetDetail.GetSuperSetDetail;

namespace UseCase.SuperSetDetail.SuperSetDetail;

public class GetSuperSetDetailOutputData : IOutputData
{
    public GetSuperSetDetailOutputData(GetSuperSetDetailListStatus status)
    {
        SuperSetDetailModel = new SuperSetDetailItem(new(), new(), new(), new());
        Status = status;
    }

    public GetSuperSetDetailOutputData(SuperSetDetailItem superSetDetailModel, GetSuperSetDetailListStatus status)
    {
        SuperSetDetailModel = superSetDetailModel;
        Status = status;
    }

    public SuperSetDetailItem SuperSetDetailModel { get; private set; }
    public GetSuperSetDetailListStatus Status { get; private set; }
}
