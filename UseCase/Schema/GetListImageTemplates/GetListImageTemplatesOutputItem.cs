namespace UseCase.Schema.GetListImageTemplates
{
    public class GetListImageTemplatesOutputItem
    {
        public GetListImageTemplatesOutputItem(string folderName, List<string> listItems)
        {
            FolderName = folderName;
            ListItems = listItems;
        }

        public string FolderName { get; private set; }
        public List<string> ListItems { get; private set; }
    }
}
