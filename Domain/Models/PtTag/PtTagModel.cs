using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Domain.Models.PtTag
{
    public class StickyNoteModel
    {
        public StickyNoteModel(int hpId, long ptId, long seqNo, string memo, int startDate, int endDate, int isDspUketuke, int isDspKarte, int isDspKaikei, int isDspRece, string backgroundColor, int tagGrpCd, int alphablendVal, int fontSize, int isDeleted, int width, int height, int left, int top)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            Memo = memo;
            StartDate = startDate;
            EndDate = endDate;
            IsDspUketuke = isDspUketuke;
            IsDspKarte = isDspKarte;
            IsDspKaikei = isDspKaikei;
            IsDspRece = isDspRece;
            BackgroundColor = backgroundColor;
            TagGrpCd = tagGrpCd;
            AlphablendVal = alphablendVal;
            FontSize = fontSize;
            IsDeleted = isDeleted;
            Width = width;
            Height = height;
            Left = left;
            Top = top;
        }

        public StickyNoteModel()
        {

            HpId = 0;
            PtId = 0;
            SeqNo = 0;
            Memo = String.Empty;
            StartDate = 0;
            EndDate = 0;
            IsDspUketuke = 0;
            IsDspKarte = 0;
            IsDspKaikei = 0;
            IsDspRece = 0;
            BackgroundColor = String.Empty;
            TagGrpCd = 0;
            AlphablendVal = 0;
            FontSize = 0;
            IsDeleted = 0;
            Width = 0;
            Height = 0;
            Left = 0;
            Top = 0;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public string Memo { get; private set; } = string.Empty;

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int IsDspUketuke { get; private set; }

        public int IsDspKarte { get; private set; }

        public int IsDspKaikei { get; private set; }

        public int IsDspRece { get; private set; }

        public string BackgroundColor { get; private set; } = string.Empty;

        public int TagGrpCd { get; private set; }

        public int AlphablendVal { get; private set; }

        public int FontSize { get; private set; }

        public int IsDeleted { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Left { get; private set; }

        public int Top { get; private set; }
    }
}
