using UseCase.Reception.Delete;

namespace EmrCloudApi.Requests.Reception
{
    public class DeleteReceptionRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public bool Flag { get; set; }

        public List<DeleteReception> RaiinNos { get; set; } = new();
    }
}
