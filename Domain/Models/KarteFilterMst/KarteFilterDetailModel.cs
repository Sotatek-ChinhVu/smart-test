namespace Domain.Models.KarteFilterMst;

public class KarteFilterDetailModel
{
    public KarteFilterDetailModel(int hpId, int userId, long filterId, bool bookMarkChecked, List<int> listHokenId, List<int> listKaId, List<int> listUserId)
    {
        HpId = hpId;
        UserId = userId;
        FilterId = filterId;
        BookMarkChecked = bookMarkChecked;
        ListHokenId = listHokenId;
        ListKaId = listKaId;
        ListUserId = listUserId;
    }

    public KarteFilterDetailModel(int hpId, int userId)
    {
        HpId = hpId;
        UserId = userId;
        FilterId = 0;
        BookMarkChecked = false;
        ListHokenId = new List<int>();
        ListKaId = new List<int>();
        ListUserId = new List<int>();
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long FilterId { get; private set; }

    public bool BookMarkChecked { get; private set; }

    public List<int> ListHokenId { get; private set; }

    public List<int> ListKaId { get; private set; }

    public List<int> ListUserId { get; private set; }
}
