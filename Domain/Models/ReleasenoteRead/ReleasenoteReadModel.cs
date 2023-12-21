namespace Domain.Models.ReleasenoteRead
{
    public class ReleasenoteReadModel
    {
        public ReleasenoteReadModel(string header, List<string> subfiles, string path) 
        { 
            Header = header;
            Subfiles = subfiles;
            Path = path;
        }

        public string Header { get; private set; }

        public List<string> Subfiles { get; private set; }

        public string Path {  get; private set; }
    }
}
