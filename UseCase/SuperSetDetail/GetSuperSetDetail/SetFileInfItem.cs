using Domain.Models.SuperSetDetail;

namespace UseCase.SuperSetDetail.GetSuperSetDetail;

public class SetFileInfItem
{
    public SetFileInfItem(SetFileInfModel model)
    {
        IsSchema = model.IsSchema;
        LinkFile = model.LinkFile;
    }

    public bool IsSchema { get; private set; }

    public string LinkFile { get; private set; }
}
