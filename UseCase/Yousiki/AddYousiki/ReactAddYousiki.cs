using Newtonsoft.Json;

namespace UseCase.Yousiki.AddYousiki;

public class ReactAddYousiki
{
    [JsonConstructor]
    public ReactAddYousiki(bool confirmSelectDataType)
    {
        ConfirmSelectDataType = confirmSelectDataType;
    }

    public ReactAddYousiki()
    {
    }

    public bool ConfirmSelectDataType { get; private set; }
}
