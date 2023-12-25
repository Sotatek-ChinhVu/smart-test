namespace Domain.Models.ReleasenoteRead
{
    public class ReleasenoteReadModel
    {
        public ReleasenoteReadModel(string header, Dictionary<string, Dictionary<string, string>> subfiles, string path) 
        { 
            Header = header;
            Subfiles = subfiles;
            Path = path;
        }

        public string Header { get; private set; }

        public Dictionary<string, Dictionary<string, string>> Subfiles { get; private set; }

        public string Path {  get; private set; }
    }
}
