using UseCase.Core.Sync.Core;

namespace UseCase.StickyNote
{
    public class GetSettingStickyNoteOutputData  : IOutputData
    {
        public GetSettingStickyNoteOutputData( int startDate, int endDate, int fontSize, int opacity, int width, int height, int isDspUketuke, int isDspKarte, int isDspKaikei, int tagGrpCd)
        {
            StartDate = startDate;
            EndDate = endDate;
            FontSize = fontSize;
            Opacity = opacity;
            Width = width;
            Height = height;
            IsDspUketuke = isDspUketuke;
            IsDspKarte = isDspKarte;
            IsDspKaikei = isDspKaikei;
            TagGrpCd = tagGrpCd;
            Status = UpdateStickyNoteStatus.Successed;
        }

        public GetSettingStickyNoteOutputData(UpdateStickyNoteStatus status)
        {
            StartDate = 0;
            EndDate = 0;
            FontSize = 0;
            Opacity = 0;
            Width = 0;
            Height = 0;
            IsDspUketuke = 0;
            IsDspKarte = 0;
            IsDspKaikei = 0;
            TagGrpCd = 0;
            Status = status;
        }

        public int StartDate { get; private set; }

        public int EndDate { get;private set; }

        public int FontSize { get; private set; }

        public int Opacity { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int IsDspUketuke { get; private set; }

        public int IsDspKarte { get; private set; }

        public int IsDspKaikei { get; private set; }

        public int TagGrpCd { get; private set; }
        public UpdateStickyNoteStatus Status { get; private set; }

    }
}
