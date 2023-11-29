namespace Domain.Models.TodayOdr;

public class OdrDateInfModel
{
    public OdrDateInfModel(int grpId, int sortNo, string grpName, string sinDateBinding, List<OdrDateDetailModel> odrDateDetailList)
    {
        GrpId = grpId;
        SortNo = sortNo;
        GrpName = grpName;
        SinDateBinding = sinDateBinding;
        OdrDateDetailList = odrDateDetailList;
    }

    public OdrDateInfModel(int grpId, int sortNo, string grpName, string sinDateBinding, List<OdrDateDetailModel> odrDateDetailList, bool isDeleted)
    {
        GrpId = grpId;
        SortNo = sortNo;
        GrpName = grpName;
        SinDateBinding = sinDateBinding;
        OdrDateDetailList = odrDateDetailList;
        IsDeleted = isDeleted;
    }

    public int GrpId { get; private set; }

    public int SortNo { get; private set; }

    public string GrpName { get; private set; }

    public string SinDateBinding { get; private set; }

    public List<OdrDateDetailModel> OdrDateDetailList { get; private set; }

    public bool IsDeleted { get; private set; }
}
