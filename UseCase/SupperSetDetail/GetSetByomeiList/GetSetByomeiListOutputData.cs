using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.SupperSetDetail.GetSetByomeiList;

public class GetSetByomeiListOutputData : IOutputData
{
    public GetSetByomeiListOutputData(GetSetByomeiListStatus status)
    {
        ListSetByomeiModel = new List<SetByomeiModel>();
        Status = status;
    }

    public GetSetByomeiListOutputData(List<SetByomeiModel> listSetByomeiModel, GetSetByomeiListStatus status)
    {
        ListSetByomeiModel = listSetByomeiModel;
        Status = status;
    }

    public List<SetByomeiModel> ListSetByomeiModel { get; private set; }
    public GetSetByomeiListStatus Status { get; private set; }
}
