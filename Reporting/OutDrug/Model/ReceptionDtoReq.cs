using System.Text.Json.Serialization;

namespace Reporting.OutDrug.Model
{
    public class ReceptionDtoReq
    {
        [JsonConstructor]
        public ReceptionDtoReq(long ptId, int sinDate, long raiinNo)
        {
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
        }

        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public long RaiinNo { get; private set; }
    }
}
