namespace Helper.Constants
{
    public class SikkanKbnConst
    {
        public const int None = 0;
        /// <summary>
        /// 皮1
        /// </summary>
        public const int Skin1 = 3;
        /// <summary>
        /// 皮2
        /// </summary>
        public const int Skin2 = 4;
        /// <summary>
        /// 特疾
        /// </summary>
        public const int Special = 5;
        /// <summary>
        /// てんかん
        /// </summary>
        public const int Epilepsy = 7;
        /// <summary>
        /// 特疾又はてんかん
        /// </summary>
        public const int Other = 8;

        public static Dictionary<int, string> SikkanKbnDict { get; } = new Dictionary<int, string>()
        {
            {None, ""},
            {Skin1, "3 皮1"},
            {Skin2, "4 皮2"},
            {Special, "5 特疾"},
            {Epilepsy, "7 てんかん"},
            {Other, "8 特疾又はてんかん"}
        };

        public static Dictionary<int, string> DisplayedSikkanKbnDict { get; } = new Dictionary<int, string>()
        {
            {None, ""},
            {Skin1, "皮1"},
            {Skin2, "皮2"},
            {Special, "特疾"},
            {Epilepsy, "てんかん"},
            {Other, "特疾又はてんかん"}
        };
    }
}
