using Domain.Models.NextOrder;

namespace UseCase.NextOrder.Get;

public class NextOrderFileInfItem
{
    public NextOrderFileInfItem(NextOrderFileInfModel model)
    {
        IsSchema = model.IsSchema;
        LinkFile = model.LinkFile;
    }

    public bool IsSchema { get; private set; }

    public string LinkFile { get; private set; }
}
