namespace Domain.Models.UketukeSbtMst;

public class UketukeSbtMstModel
{
    public UketukeSbtMstModel(int kbnId, string kbnName, int sortNo, int isDeleted)
    {
        KbnId = kbnId;
        KbnName = kbnName;
        SortNo = sortNo;
        IsDeleted = isDeleted;
    }

    public int KbnId { get; private set; }
    public string KbnName { get; private set; }
    public int SortNo { get; private set; }
    public int IsDeleted { get; private set; }
}
