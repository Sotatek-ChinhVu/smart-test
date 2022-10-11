using Domain.Models.PtTag;
using Entity.Tenant;

namespace EmrCloudApi.Tenant.Requests.StickyNote
{
    public class SaveStickyNoteRequest
    {
        public List<StickyNoteRequest> stickyNoteModels { get; set; } = new List<StickyNoteRequest>();
    }
    public class StickyNoteRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public long SeqNo { get; set; }

        public string Memo { get; set; } = string.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public int IsDspUketuke { get; set; }

        public int IsDspKarte { get; set; }

        public int IsDspKaikei { get; set; }

        public int IsDspRece { get; set; }

        public string BackgroundColor { get; set; } = string.Empty;

        public int TagGrpCd { get; set; }

        public int AlphablendVal { get; set; }

        public int FontSize { get; set; }

        public int IsDeleted { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }
        public StickyNoteModel Map()
        {
            return new StickyNoteModel(HpId, PtId, SeqNo, Memo, StartDate, EndDate, IsDspUketuke, IsDspKarte, IsDspKaikei, IsDspRece, BackgroundColor, TagGrpCd, AlphablendVal, FontSize, 1, Width, Height, Left, Top);
        }
    }
}
