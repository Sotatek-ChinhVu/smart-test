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

    public KarteFilterMstModel(int hpId, int userId)
    {
        HpId = hpId;
        UserId = userId;
        FilterId = 0;
        FilterName = string.Empty;
        SortNo = 0;
        AutoApply = 0;
        IsDeleted = 0;
        KarteFilterDetailModel = new KarteFilterDetailModel(hpId, UserId);
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long FilterId { get; private set; }

    public string FilterName { get; private set; }

    public int SortNo { get; private set; }

    public int AutoApply { get; private set; }

    public int IsDeleted { get; private set; }

    public KarteFilterDetailModel KarteFilterDetailModel { get; private set; }

    public bool IsAutoApply
    {
        get
        {
            return FilterId > 0 && (AutoApply == 1);
        }
    }

    public bool OnlyBookmark
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return false;
            return KarteFilterDetailModel.BookMarkChecked;
        }
    }

    public bool IsAllDepartment
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.ListKaId.Count <= 0 && KarteFilterDetailModel.FilterId <= 0)) return true;

            return KarteFilterDetailModel.ListKaId.Contains(0);
        }
    }

    public List<int> ListDepartmentCode
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return new List<int>();
            return KarteFilterDetailModel.ListKaId;
        }
    }

    public bool IsAllDoctor
    {
        get
        {

            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return true;

            return KarteFilterDetailModel.ListUserId.Contains(0);
        }
    }

    public List<int> ListDoctorCode
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return new List<int>();

            return KarteFilterDetailModel.ListUserId;
        }
    }

    public bool IsAllHoken
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return true;

            return KarteFilterDetailModel.ListHokenId.Contains(0);
        }
    }
    public bool IsHoken
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return true;
            var allDepartmentSetting = KarteFilterDetailModel.ListHokenId.Where(h => h == 1)?.FirstOrDefault();
            return allDepartmentSetting != null;
        }
    }
    public bool IsJihi
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return true;

            return KarteFilterDetailModel.ListHokenId.Contains(2);
        }
    }
    public bool IsRosai
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return true;

            return KarteFilterDetailModel.ListHokenId.Contains(3);
        }
    }

    public bool IsJibai
    {
        get
        {
            if (FilterId <= 0 || (KarteFilterDetailModel.HpId <= 0 && KarteFilterDetailModel.UserId <= 0 && KarteFilterDetailModel.FilterId <= 0)) return true;

            return KarteFilterDetailModel.ListHokenId.Contains(4);
        }
    }

}
