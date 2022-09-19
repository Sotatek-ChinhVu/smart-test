using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.SuperSetDetail;

public class GetSuperSetDetailOutputData : IOutputData
{
    public GetSuperSetDetailOutputData(GetSuperSetDetailListStatus status)
    {
        SuperSetDetailModel = new SuperSetDetailModel(
                                                        new List<SetByomeiModel>(),
                                                        new SetKarteInfModel(0, 0, string.Empty),
                                                        new List<SetGroupOrderInfModel>()
                                                    );
        Status = status;
    }

    public GetSuperSetDetailOutputData(SuperSetDetailModel superSetDetailModel, GetSuperSetDetailListStatus status)
    {
        SuperSetDetailModel = superSetDetailModel;
        Status = status;
    }

    public SuperSetDetailModel SuperSetDetailModel { get; private set; }
    public GetSuperSetDetailListStatus Status { get; private set; }
}
