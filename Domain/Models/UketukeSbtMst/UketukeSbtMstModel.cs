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
    }

    public UketukeSbtMstModel()
    {
        KbnName = string.Empty;
    }

    public int KbnId { get; private set; }

    public string KbnName { get; private set; }

    public int SortNo { get; private set; }

    public int IsDeleted { get; private set; }

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
