using UseCase.SupperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;

namespace UseCase.SupperSetDetail.SaveSuperSetDetail;

public class SaveSuperSetDetailInputItem
{
    public List<SaveSetByomeiInputItem> SetByomeiModelInputs { get; private set; } = new();
}
