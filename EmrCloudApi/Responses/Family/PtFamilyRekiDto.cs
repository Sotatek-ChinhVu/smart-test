using UseCase.Family;

namespace EmrCloudApi.Responses.Family;

public class PtFamilyRekiDto
{
    public PtFamilyRekiDto(FamilyRekiItem model)
    {
        Id = model.Id;
        ByomeiCd = model.ByomeiCd;
        Byomei = model.Byomei;
        Cmt = model.Cmt;
        SortNo = model.SortNo;
        IsDeleted = model.IsDeleted;
    }

    public long Id { get; private set; }

    public string ByomeiCd { get; private set; }

    public string Byomei { get; private set; }

    public string Cmt { get; private set; }

    public bool IsDeleted { get; private set; }

    public int SortNo { get; private set; }
}
