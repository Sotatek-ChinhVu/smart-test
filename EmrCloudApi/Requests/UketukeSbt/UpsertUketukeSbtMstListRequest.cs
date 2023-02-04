using Domain.Models.UketukeSbtMst;

namespace EmrCloudApi.Requests.UketukeSbt;

public class UpsertUketukeSbtMstListRequest
{
    public List<UketukeSbtMstModel> uketukeSbtMsts { get; set; } = new List<UketukeSbtMstModel> ();

    public class UketukeSbtMstModel
    {
        public int KbnId { get; set; }

        public string KbnName { get; set; } = string.Empty;

        public int IsDeleted { get; set; }

        public int SortNo { get; set; }
    }
}
