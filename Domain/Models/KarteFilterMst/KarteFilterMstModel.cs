namespace Domain.Models.KarteFilterMst;

public class KarteFilterMstModel
{
    public KarteFilterMstModel(int hpId, int userId, long filterId, string filterName, int sortNo, int autoApply, int isDeleted, KarteFilterDetailModel karteFilterDetailModel)
    {
        HpId = hpId;
        UserId = userId;
        FilterId = filterId;
        FilterName = filterName;
        SortNo = sortNo;
        AutoApply = autoApply;
        IsDeleted = isDeleted;
        KarteFilterDetailModel = karteFilterDetailModel;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long FilterId { get; private set; }

    public string FilterName { get; private set; }

    public int SortNo { get; private set; }

    public int AutoApply { get; private set; }

    public int IsDeleted { get; private set; }

    public KarteFilterDetailModel KarteFilterDetailModel { get; private set; }

    public bool OnlyBookmark
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return false;
            return KarteFilterDetailModel.BookMarkChecked;
        }
    }

    public bool AllDepartment
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.ListKaId .Count <= 0 && KarteFilterDetailModel.FilterId <= 0)) return false;

            return KarteFilterDetailModel.ListKaId.Count > 0;
        }
    }

    public List<int> FilterWithListDepartmentCode
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return new List<int>();
            return KarteFilterDetailModel.ListKaId;
        }
    }

    public bool AllDoctor
    {
        get
        {

            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return true;

            return KarteFilterDetailModel.ListUserId.Count > 0;
        }
    }

    public List<int> FilterWithListDoctorCode
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return new List<int>();

            return KarteFilterDetailModel.ListUserId;
        }
    }

    public bool AllHoken
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return true;

            return KarteFilterDetailModel.ListHokenId.Count > 0;
        }
    }

}
