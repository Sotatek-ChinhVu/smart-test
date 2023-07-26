using System.Text.Json.Serialization;

namespace Domain.Models.DrugInfor;

public class DrugInforModel
{
    public DrugInforModel(string name, string genericName, string unit, string maker, string vender, int kohatuKbn, double ten, string receUnitName, string mark, string yjCode, string pathPicZai, string pathPicHou, string defaultPathPicZai, string customPathPicZai, string otherPicZai, string defaultPathPicHou, string customPathPicHou, string otherPicHou, List<string> listPicHou, List<string> listPicZai)
    {
        Name = name;
        GenericName = genericName;
        Unit = unit;
        Maker = maker;
        Vender = vender;
        KohatuKbn = kohatuKbn;
        Ten = ten;
        ReceUnitName = receUnitName;
        Mark = mark;
        YjCode = yjCode;
        PathPicZai = pathPicZai;
        PathPicHou = pathPicHou;
        DefaultPathPicZai = defaultPathPicZai;
        CustomPathPicZai = customPathPicZai;
        OtherPicZai = otherPicZai;
        DefaultPathPicHou = defaultPathPicHou;
        CustomPathPicHou = customPathPicHou;
        OtherPicHou = otherPicHou;
        ListPicHou = listPicHou;
        ListPicZai = listPicZai;
    }

    public DrugInforModel()
    {
        Name = string.Empty;
        GenericName = string.Empty;
        Unit = string.Empty;
        Maker = string.Empty;
        Vender = string.Empty;
        ReceUnitName = string.Empty;
        Mark = string.Empty;
        YjCode = string.Empty;
        PathPicZai = string.Empty;
        PathPicHou = string.Empty;
        DefaultPathPicZai = string.Empty;
        CustomPathPicZai = string.Empty;
        OtherPicZai = string.Empty;
        DefaultPathPicHou = string.Empty;
        CustomPathPicHou = string.Empty;
        OtherPicHou = string.Empty;
        ListPicHou = new List<string>();
        ListPicZai = new List<string>();
    }

    [JsonPropertyName("name")]
    public string Name { get; private set; }

    [JsonPropertyName("genericName")]
    public string GenericName { get; private set; }

    [JsonPropertyName("unit")]
    public string Unit { get; private set; }

    [JsonPropertyName("maker")]
    public string Maker { get; private set; }

    [JsonPropertyName("vender")]
    public string Vender { get; private set; }

    [JsonPropertyName("kohatuKbn")]
    public int KohatuKbn { get; private set; }

    [JsonPropertyName("ten")]
    public double Ten { get; private set; }

    [JsonPropertyName("receUnitName")]
    public string ReceUnitName { get; private set; }

    [JsonPropertyName("mark")]
    public string Mark { get; private set; }

    [JsonPropertyName("yjCode")]
    public string YjCode { get; private set; }

    [JsonPropertyName("pathPicZai")]
    public string PathPicZai { get; set; }

    [JsonPropertyName("pathPicHou")]
    public string PathPicHou { get; set; }

    [JsonPropertyName("defaultPathPicZai")]
    public string DefaultPathPicZai { get; set; }

    [JsonPropertyName("customPathPicZai")]
    public string CustomPathPicZai { get; set; }

    [JsonPropertyName("otherPicZai")]
    public string OtherPicZai { get; set; }

    [JsonPropertyName("defaultPathPicHou")]
    public string DefaultPathPicHou { get; set; }

    [JsonPropertyName("customPathPicHou")]
    public string CustomPathPicHou { get; set; }

    [JsonPropertyName("otherPicHou")]
    public string OtherPicHou { get; set; }

    [JsonPropertyName("listPicHou")]
    public List<string> ListPicHou { get; set; }

    [JsonPropertyName("listPicZai")]
    public List<string> ListPicZai { get; set; }

    [JsonPropertyName("kohatuKbnName")]
    public string KohatuKbnName
    {
        get
        {
            switch (KohatuKbn)
            {
                case 0:
                    return "なし";

                case 1:
                    return "ー";

                case 2:
                    return "あり";

                default:
                    return string.Empty;
            }
        }
    }
}
