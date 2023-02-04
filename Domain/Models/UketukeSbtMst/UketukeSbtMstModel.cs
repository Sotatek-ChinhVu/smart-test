namespace Domain.Models.UketukeSbtMst;
using static Helper.Constants.UketukeSbtMstConstant;

public class UketukeSbtMstModel
{
    public UketukeSbtMstModel(int kbnId, string kbnName, int sortNo, int isDeleted)
    {
        KbnId = kbnId;
        KbnName = kbnName;
        SortNo = sortNo;
        IsDeleted = isDeleted;
        CreateMachine = string.Empty;
        UpdateMachine = string.Empty;
    }

    public UketukeSbtMstModel(int kbnId, string kbnName, int sortNo, int isDeleted, DateTime createDate, int createId, string createMachine, DateTime updateDate, int updateId, string updateMachine)
    {
        KbnId = kbnId;
        KbnName = kbnName;
        SortNo = sortNo;
        IsDeleted = isDeleted;
        CreateDate = createDate;
        CreateId = createId;
        CreateMachine = createMachine;
        UpdateDate = updateDate;
        UpdateId = updateId;
        UpdateMachine = updateMachine;
    }

    public int KbnId { get; private set; }

    public string KbnName { get; private set; }

    public int SortNo { get; private set; }

    public int IsDeleted { get; private set; }

    public DateTime CreateDate { get; private set; }

    public int CreateId { get; private set; }

    public string CreateMachine { get; private set; }

    public DateTime UpdateDate { get; private set; }

    public int UpdateId { get; private set; }

    public string UpdateMachine { get; private set; }

    public ValidationStatus Validation()
    {
        if(KbnId < 0)
        {
            return ValidationStatus.InvalidKbnId;
        }
        if(KbnName.Length > 20)
        {
            return ValidationStatus.InvalidKbnName;
        }
        if(SortNo < 0)
        {
            return ValidationStatus.InvalidSortNo;
        }
        if(IsDeleted < 0)
        {
            return ValidationStatus.InvalidIsDeleted;
        }
        return ValidationStatus.Valid;
    }
}
