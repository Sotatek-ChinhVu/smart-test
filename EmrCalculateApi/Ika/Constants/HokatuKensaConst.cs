using Helper.Constants;

namespace EmrCalculateApi.Ika.Constants
{
    class HokatuKensaConst
    {
        /// <summary>
        /// 生化学検査Ⅰ
        /// </summary>
        public const int Seika1 = 1;
        /// <summary>
        /// 内分泌学検査
        /// </summary>
        public const int Naibunpitu = 2;
        /// <summary>
        /// 肝炎ウイルス関連検査
        /// </summary>
        public const int Kanen = 3;
        /// <summary>
        /// 腫瘍マーカー精密検査
        /// </summary>
        public const int Syuyo = 5;
        /// <summary>
        /// 出血・凝固検査
        /// </summary>
        public const int Syukketu = 6;
        /// <summary>
        /// 自己抗体検査
        /// </summary>
        public const int JikoKoutai = 7;
        /// <summary>
        /// ウイルス抗体価（定性・半定量・定量） 
        /// </summary>
        public const int Virus = 9;
        /// <summary>
        /// グロブリンクラス別ウイルス抗体価 
        /// </summary>
        public const int Globulin = 10;
        /// <summary>
        /// 悪性腫瘍遺伝子検査
        /// </summary>
        public const int Akusei = 12;
        /// <summary>
        /// 悪性腫瘍遺伝子検査（容易なもの）
        /// </summary>
        public const int AkuseiYoui = 13;
        /// <summary>
        /// 悪性腫瘍遺伝子検査（複雑なもの）
        /// </summary>
        public const int AkuseiFukuzatu = 14;
        /// <summary>
        /// 悪性腫瘍遺伝子検査（血液・血漿）（ＲＯＳ１融合遺伝子検査、ＡＬＫ融合遺伝子検査、ＥＧＦＲ遺伝子検査（血漿））
        /// </summary>
        public const int AkuseiKetueki1 = 15;
        /// <summary>
        /// 悪性腫瘍遺伝子検査（血液・血漿）（ＭＥＴｅｘ14遺伝子検査、ＮＴＲＫ融合遺伝子検査）
        /// </summary>
        public const int AkuseiKetueki2 = 16;
        /// <summary>
        /// 内分泌不可試験
        /// </summary>
        public const int NaibunpituFuka = 8;
        /// <summary>
        /// Ige/Hrt
        /// </summary>
        public const int IgeHrt = 11;

        public static List<(int kbn, List<(int min, int max, string item)> ranges)> HoukatuKensaList =
            new List<(int, List<(int, int, string)>)>
            {
                // 生化学検査（Ｉ）
                (HokatuKensaConst.Seika1, new List<(int, int, string)>
                    {
                        (5, 7, ItemCdConst.KensaMarumeSeika5_7),
                        (8, 9, ItemCdConst.KensaMarumeSeika8_9),
                        (10, 999, ItemCdConst.KensaMarumeSeika10)
                    }),
                // 内分泌学検査
                (HokatuKensaConst.Naibunpitu, new List<(int, int, string)>
                    {
                        (3, 5, ItemCdConst.KensaMarumeNaibunpitu3_5),
                        (6, 7, ItemCdConst.KensaMarumeNaibunpitu6_7),
                        (8, 999, ItemCdConst.KensaMarumeNaibunpitu8)
                    }),
                // 肝炎ウイルス関連検査
                (HokatuKensaConst.Kanen, new List<(int, int, string)>
                    {
                        (3, 3, ItemCdConst.KensaMarumeKanen3),
                        (4, 4, ItemCdConst.KensaMarumeKanen4),
                        (5, 999, ItemCdConst.KensaMarumeKanen5)
                    }),
                // 腫瘍マーカー精密検査
                (HokatuKensaConst.Syuyo, new List<(int, int, string)>
                    {
                        (2, 2, ItemCdConst.KensaMarumeSyuyou2),
                        (3, 3, ItemCdConst.KensaMarumeSyuyou3),
                        (4, 999, ItemCdConst.KensaMarumeSyuyou4)
                    }),
                // 出血・凝固検査
                (HokatuKensaConst.Syukketu, new List<(int, int, string)>
                    {
                        (3, 4, ItemCdConst.KensaMarumeSyukketu3_4),
                        (5, 999, ItemCdConst.KensaMarumeSyukketu5)
                    }),
                // 自己抗体検査
                (HokatuKensaConst.JikoKoutai, new List<(int, int, string)>
                    {
                        (2, 2, ItemCdConst.KensaMarumeJikoKoutai2),
                        (3, 999, ItemCdConst.KensaMarumeJikoKoutai3)
                    }),
                // ウイルス抗体価（定性・半定量・定量） 
                (HokatuKensaConst.Virus, new List<(int, int, string)>
                    {
                        (8, 999, ItemCdConst.KensaMarumeVirus8)
                    }),
                // グロブリンクラス別ウイルス抗体価 
                (HokatuKensaConst.Globulin, new List<(int, int, string)>
                    {
                        (2, 999, ItemCdConst.KensaMarumeGlobulin2)
                    }),
                // 悪性腫瘍遺伝子検査
                (HokatuKensaConst.Akusei, new List<(int, int, string)>
                    {
                        (2, 2, ItemCdConst.KensaMarumeAkusei2),
                        (3, 999, ItemCdConst.KensaMarumeAkusei3)
                    }),
                // 悪性腫瘍遺伝子検査（容易なもの）2020/04～
                (HokatuKensaConst.AkuseiYoui, new List<(int, int, string)>
                    {
                        (2, 2, ItemCdConst.KensaMarumeAkusei2),
                        (3, 3, ItemCdConst.KensaMarumeAkusei3),
                        (4, 999, ItemCdConst.KensaMarumeAkusei4)
                    }),
                // 悪性腫瘍遺伝子検査（複雑なもの）2020/04～
                (HokatuKensaConst.AkuseiFukuzatu, new List<(int, int, string)>
                    {
                        (2, 2, ItemCdConst.KensaMarumeAkuseiFukuzatu2),
                        (3, 999, ItemCdConst.KensaMarumeAkuseiFukuzatu3)
                    }),
                // 悪性腫瘍遺伝子検査１ 2022/04～
                (HokatuKensaConst.AkuseiKetueki1, new List<(int, int, string)>
                    {
                        (2, 2, ItemCdConst.KensaMarumeAkuseiKetueki1_2),
                        (3, 999, ItemCdConst.KensaMarumeAkuseiKetueki1_3)
                    }),
                // 悪性腫瘍遺伝子検査２ 2022/04～
                (HokatuKensaConst.AkuseiKetueki2, new List<(int, int, string)>
                    {
                        (2, 999, ItemCdConst.KensaMarumeAkuseiKetueki2_2)
                    })
            };
    }
}
