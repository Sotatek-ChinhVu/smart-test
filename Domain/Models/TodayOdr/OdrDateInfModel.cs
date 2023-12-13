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

    public OdrDateInfModel(int grpId, int sortNo, string grpName, int isDeleted, List<OdrDateDetailModel> odrDateDetailList)
    {
        GrpId = grpId;
        SortNo = sortNo;
        GrpName = grpName;
        IsDeleted = isDeleted;
        SinDateBinding = string.Empty;
        OdrDateDetailList = odrDateDetailList;
    }

    public int GrpId { get; set; }

    public int SortNo { get; private set; }

    public string GrpName { get; private set; }

    public string SinDateBinding { get; private set; }

    public List<OdrDateDetailModel> OdrDateDetailList { get; private set; }

    public int IsDeleted { get; private set; }

    public bool CheckDefaultValue()
    {
        return GrpName == string.Empty && GrpId == 0;
    }
}
