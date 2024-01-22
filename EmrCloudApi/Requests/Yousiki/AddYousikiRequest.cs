using UseCase.Yousiki.AddYousiki;

namespace EmrCloudApi.Requests.Yousiki;

public class AddYousikiRequest
{
    public int SinYm { get; set; }

    public long PtNum { get; set; }

    public int SelectDataType { get; set; }

    public ReactAddYousiki ReactAddYousiki { get; set; } = new();
}
