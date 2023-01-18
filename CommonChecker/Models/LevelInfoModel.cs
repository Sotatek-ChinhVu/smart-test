namespace CommonChecker.Models
{
    public class LevelInfoModel
    {

        public string Title { get; set; }

        public string FirstItemName { get; set; }

        public string SecondItemName { get; set; }

        public string Comment { get; set; }

        public int Level { get; set; }

        public string Caption
        {
            get
            {
                return FirstItemName + (string.IsNullOrEmpty(SecondItemName) ? string.Empty : " × " + SecondItemName);
            }
        }

        public bool IsShowLevelButton
        {
            get
            {
                return !string.IsNullOrEmpty(Title);
            }
        }

    }
}
