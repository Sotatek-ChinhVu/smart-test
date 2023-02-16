using Domain.Models.Family;

namespace UseCase.Family;

public class FamilyRekiItem
{
    public FamilyRekiItem(long id, string byomeiCd, string byomei, string cmt, int sortNo, bool isDeleted)
    {
        Id = id;
        ByomeiCd = byomeiCd;
        Byomei = byomei;
        Cmt = cmt;
        SortNo = sortNo;
        IsDeleted = isDeleted;
    }

    public FamilyRekiItem(PtFamilyRekiModel model)
    {
        Id = model.Id;
        ByomeiCd = model.ByomeiCd;
        Byomei = model.Byomei;
        Cmt = model.Cmt;
        SortNo = model.SortNo;
        IsDeleted = false;
    }

    public long Id { get; private set; }

    public string ByomeiCd { get; private set; }

    public string Byomei { get; private set; }

    public string Cmt { get; private set; }

    public int SortNo { get; private set; }

    public bool IsDeleted { get; private set; }
}
