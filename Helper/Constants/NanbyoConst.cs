namespace Helper.Constants;

public static class NanbyoConst
{
    /// <summary>
    /// 算定対象外
    /// </summary>
    public const int SanteiGai = 0;

    /// <summary>
    /// 難病外来指導管理料算定対象
    /// </summary>
    public const int Gairai = 9;

    public static Dictionary<int, string> NanByoKbnDict => new Dictionary<int, string>()
        {
            {SanteiGai, "" },
            {Gairai, "9 難病" }
        };

    public static Dictionary<int, string> DisplayedNanByoKbnDict => new Dictionary<int, string>()
        {
            {SanteiGai, "" },
            {Gairai, "難病" }
        };
}
