using Domain.Models.KarteFilterMst;

namespace UseCase.KarteFilter.GetListKarteFilter;

public class KarteFilterMstOutputItem
{
    public KarteFilterMstOutputItem(KarteFilterMstModel model)
    {
        FilterId = model.FilterId;
        FilterName = model.FilterName;
        SortNo = model.SortNo;
        AutoApply = model.AutoApply;
        KarteFilterDetailModel = new KarteFilterDetailOutputItem(model.KarteFilterDetailModel);
    }

    public long FilterId { get; private set; }

    public string FilterName { get; private set; }

    public int SortNo { get; private set; }

    public int AutoApply { get; private set; }

    public KarteFilterDetailOutputItem KarteFilterDetailModel { get; private set; }
}
