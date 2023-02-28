using Domain.Models.UketukeSbtMst;

namespace EmrCloudApi.Requests.UketukeSbt;

public class UpsertUketukeSbtMstListRequest
{
    public List<UketukeSbtMstDto> UketukeSbtMsts { get; set; } = new List<UketukeSbtMstDto>();

    public class UketukeSbtMstDto
    {
        public int KbnId { get; set; }

        public string KbnName { get; set; } = string.Empty;

        public int IsDeleted { get; set; }

        public int SortNo { get; set; }
    }
}
