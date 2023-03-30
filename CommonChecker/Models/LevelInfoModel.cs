namespace CommonChecker.Models
{
    public class LevelInfoModel
    {

        public string Title { get; set; } = string.Empty;

        public string FirstItemName { get; set; } = string.Empty;

        public string SecondItemName { get; set; } = string.Empty; 

        public string Comment { get; set; } = string.Empty;

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
