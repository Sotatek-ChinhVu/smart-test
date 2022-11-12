using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;
using UseCase.SuperSetDetail.GetSuperSetDetail;

namespace UseCase.SuperSetDetail.SuperSetDetail;

public class GetSuperSetDetailOutputData : IOutputData
{
    public GetSuperSetDetailOutputData(GetSuperSetDetailListStatus status)
    {
        SuperSetDetailModel = new SuperSetDetailItem(
                                                        new List<SetByomeiItem>(),
                                                        new SetKarteInfModel(0, 0, string.Empty),
                                                        new List<SetGroupOrderInfItem>()
                                                    );
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
