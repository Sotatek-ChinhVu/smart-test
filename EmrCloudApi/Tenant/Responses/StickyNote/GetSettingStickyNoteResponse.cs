namespace EmrCloudApi.Tenant.Responses.StickyNote
{
    public class GetSettingStickyNoteResponse
    {
        public GetSettingStickyNoteResponse(int startDate, int endDate, int fontSize, int opacity, int width, int height, int isDspUketuke, int isDspKarte, int isDspKaikei, int tagGrpCd)
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
        }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int FontSize { get; private set; }

        public int Opacity { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int IsDspUketuke { get; private set; }

        public int IsDspKarte { get; private set; }

        public int IsDspKaikei { get; private set; }

        public int TagGrpCd { get; private set; }
    }
}
