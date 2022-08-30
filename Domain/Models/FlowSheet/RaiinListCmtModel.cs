namespace Domain.Models.FlowSheet
{
    public class RaiinListCmtModel
    {

        public int CmtKbn { get; private set; }

        public string Text { get; private set; }

        public RaiinListCmtModel(int cmtKbn, string text)
        {
            CmtKbn = cmtKbn;
            Text = text;
        }

        public RaiinListCmtModel()
        {
            Text = string.Empty;
        }
    }
}
