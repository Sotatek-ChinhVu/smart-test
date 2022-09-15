using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;

namespace UseCase.SuperSetDetail.SaveSuperSetDetail;

public class SaveSuperSetDetailInputItem
{
    public List<SaveSetByomeiInputItem> SetByomeiModelInputs { get; private set; } = new();
}
