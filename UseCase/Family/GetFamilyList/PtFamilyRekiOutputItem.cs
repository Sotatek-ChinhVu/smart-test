using Domain.Models.Family;

namespace UseCase.Family.GetListFamily;

public class PtFamilyRekiOutputItem
{
    public PtFamilyRekiOutputItem(PtFamilyRekiModel model)
    {
        Id = model.Id;
        ByomeiCd = model.ByomeiCd;
        Byomei = model.Byomei;
        Cmt = model.Cmt;
        SortNo = model.SortNo;
    }

    public long Id { get; private set; }

    public string ByomeiCd { get; private set; }

    public string Byomei { get; private set; }

    public string Cmt { get; private set; }

    public int SortNo { get; private set; }
}
