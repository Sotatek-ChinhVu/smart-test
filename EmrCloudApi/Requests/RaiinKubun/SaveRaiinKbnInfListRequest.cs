using Domain.Models.Reception;

namespace EmrCloudApi.Requests.RaiinKubun
{
    public class SaveRaiinKbnInfListRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public long RaiinNo { get; set; }
        public int UserId { get; set; }
        public List<RaiinKbnInfDto> KbnInfDtos { get; set; } = new();
    }
}
