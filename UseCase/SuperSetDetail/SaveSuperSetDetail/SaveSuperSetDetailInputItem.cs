using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;

namespace UseCase.SuperSetDetail.SaveSuperSetDetail;

public class SaveSuperSetDetailInputItem
{
    public SaveSuperSetDetailInputItem(List<SaveSetByomeiInputItem> setByomeiModelInputs)
    {
        SetByomeiModelInputs = setByomeiModelInputs;
    }

    public List<SaveSetByomeiInputItem> SetByomeiModelInputs { get; private set; }
}
