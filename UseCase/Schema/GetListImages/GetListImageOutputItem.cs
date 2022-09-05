namespace UseCase.Schema.GetListImages
{
    public class GetListImageOutputItem
    {
        public GetListImageOutputItem(string folderName, List<string> listItems)
        {
            FolderName = folderName;
            ListItems = listItems;
        }

        public string FolderName { get; private set; }
        public List<string> ListItems { get; private set; }
    }
}
