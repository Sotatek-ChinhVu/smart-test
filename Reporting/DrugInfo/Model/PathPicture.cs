namespace Reporting.DrugInfo.Model;

public class PathPicture
{
    public PathPicture(string pathPicZai, string pathPicHou)
    {
        PathPicZai = pathPicZai;
        PathPicHou = pathPicHou;
    }

    public string PathPicZai { get; set; }

    public string PathPicHou { get; set; }
}
