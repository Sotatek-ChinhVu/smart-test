using Helper.Enum;

namespace Helper.Constants
{
    public static class SinHoConstant
    {
        public static Dictionary<string, string> CodeHoDic = new Dictionary<string, string>()
        {
                {nameof(CodeHoEnum.A), "初再診"},
                {nameof(CodeHoEnum.B), "医学管理"},
                {nameof(CodeHoEnum.C), "在宅"},
                {nameof(CodeHoEnum.D), "検査"},
                {nameof(CodeHoEnum.E), "画像"},

                {nameof(CodeHoEnum.F), "投薬"},
                {nameof(CodeHoEnum.G), "注射"},
                {nameof(CodeHoEnum.H), "リハビリテーション"},
                {nameof(CodeHoEnum.I), "精神"},
                {nameof(CodeHoEnum.J), "処置"},

                {nameof(CodeHoEnum.K), "手術"},
                {nameof(CodeHoEnum.L), "麻酔"},
                {nameof(CodeHoEnum.M), "放射線"},
                {nameof(CodeHoEnum.N), "病理"},
                {nameof(CodeHoEnum.R), "労災"},

                {nameof(CodeHoEnum.JB), "文書料"},
                {nameof(CodeHoEnum.JS), "自費算定"},
                {nameof(CodeHoEnum.SZ), "消費税"},
                {nameof(CodeHoEnum.UZ), "内税"},
                {nameof(CodeHoEnum.ALL), "合計"},
        };
    }
}
