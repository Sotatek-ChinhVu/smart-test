namespace Helper.Constants
{
    public static class PtDiseaseConst
    {
        public const string FREE_WORD = "0000999";

        public static Dictionary<string, int> TenkiKbns { get; } = new Dictionary<string, int>()
        {
            {"下記以外", 0 },
            {"治ゆ", 1},
            {"中止", 2 },
            {"死亡", 3},
            {"その他", 9}
        };

        public static Dictionary<string, int> SikkanKbns { get; } = new Dictionary<string, int>()
        {
            {"対象外", 1 },
            {"皮膚科特定疾患指導管理料（１）算定対象", 3},
            {"皮膚科特定疾患指導管理料（２）算定対象", 4},
            {"特定疾患療養指導料／老人慢性疾患生活指導料算定対象", 5},
            {"てんかん指導料算定対象", 7},
            {"特定疾患療養管理料又はてんかん指導料算", 8}
        };

        public static Dictionary<string, int> NanByoCds { get; } = new Dictionary<string, int>()
        {
            {"算定対象外", 0},
            {"難病外来指導管理料算定対象", 9}
        };

        public enum ValidationStatus
        {
            InvalidTenkiKbn = 6,
            InvalidSikkanKbn,
            InvalidNanByoCd,
            InvalidFreeWord,
            InvalidTenkiDateContinue,
            InvalidTekiDateAndStartDate,
            Valid
        };
    }
}
