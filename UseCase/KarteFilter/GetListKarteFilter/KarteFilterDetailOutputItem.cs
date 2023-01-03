using Domain.Models.KarteFilterMst;

namespace UseCase.KarteFilter.GetListKarteFilter;

public class KarteFilterDetailOutputItem
{
    public KarteFilterDetailOutputItem(KarteFilterDetailModel model)
    {
        BookMarkChecked = model.BookMarkChecked;
        ListHokenId = model.ListHokenId;
        ListKaId = model.ListKaId;
        ListUserId = model.ListUserId;
    }

    public bool BookMarkChecked { get; private set; }

    public List<int> ListHokenId { get; private set; }

    public List<int> ListKaId { get; private set; }

    public List<int> ListUserId { get; private set; }
}
