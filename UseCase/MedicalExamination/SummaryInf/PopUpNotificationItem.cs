namespace UseCase.MedicalExamination.SummaryInf
{
    public class PopUpNotificationItem
    {
        public PopUpNotificationItem(string textInfo, string headerInfo)
        {
            TextInfo = textInfo;
            HeaderInfo = headerInfo;
        }

        public PopUpNotificationItem()
        {
            TextInfo = string.Empty;
            HeaderInfo = string.Empty;
        }

        public string TextInfo { get; private set; }

        public string HeaderInfo { get; private set; }
    }
}
