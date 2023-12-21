namespace Domain.Models.ReleasenoteRead
{
    public class ReleasenoteReadModel
    {
        public ReleasenoteReadModel(string header/*, Dictionary<string, string> nameFile_Path*/, List<string> subfiles, string path) 
        { 
            Header = header;
            //NameFile_Path = nameFile_Path;
            Subfiles = subfiles;
            Path = path;
        }

        public string Header { get; private set; }

        //public Dictionary<string, string> NameFile_Path { get; private set; }

        public List<string> Subfiles { get; private set; }

        public string Path {  get; private set; }
    }
}
