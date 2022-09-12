using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.SupperSetDetail.SupperSetDetail;

public class GetSupperSetDetailOutputData : IOutputData
{
    public GetSupperSetDetailOutputData(GetSupperSetDetailListStatus status)
    {
        SuperSetDetailModel = new SuperSetDetailModel(
                                                        new List<SetByomeiModel>(),
                                                        new SetKarteInfModel(0, 0, string.Empty),
                                                        new List<SetOrdInfModel>()
                                                    );
        Status = status;
    }

    public GetSupperSetDetailOutputData(SuperSetDetailModel superSetDetailModel, GetSupperSetDetailListStatus status)
    {
        SuperSetDetailModel = superSetDetailModel;
        Status = status;
    }

    public SuperSetDetailModel SuperSetDetailModel { get; private set; }
    public GetSupperSetDetailListStatus Status { get; private set; }
}
