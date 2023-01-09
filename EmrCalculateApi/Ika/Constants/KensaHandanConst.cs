using Helper.Constants;

namespace EmrCalculateApi.Ika.Constants
{
    class KensaHandanConst
    {
        #region (検体検査)

        /// <summary>
        /// 1: 尿・糞便等検査
        /// </summary>
        public const int NyouFunben = 1;

        /// <summary>
        /// 2: 血液学的検査
        /// </summary>
        public const int Ketueki = 2;

        /// <summary>
        /// 3: 生化学的検査（Ⅰ）　
        /// </summary>
        public const int Seika1 = 3;

        /// <summary>
        /// 4: 生化学的検査（Ⅱ）
        /// </summary>
        public const int Seika2 = 4;

        /// <summary>
        /// 5: 免疫学的検査
        /// </summary>
        public const int Meneki = 5;

        /// <summary>
        /// 6: 微生物学的検査
        /// </summary>
        public const int Biseibutu = 6;
        #endregion

        // (検体検査)
        //8: 基本的検体検査

        #region (生理検査)

        /// <summary>
        /// 11: 呼吸機能検査
        /// </summary>
        public const int Kokyu = 11;

        /// <summary>
        /// 13: 脳波検査
        /// </summary>
        public const int Noha = 13;

        /// <summary>
        /// 14: 神経・筋検査
        /// </summary>
        public const int Sinkei = 14;

        /// <summary>
        /// 15: ラジオアイソトープ検査
        /// </summary>
        public const int Radio = 15;

        /// <summary>
        /// 16: 眼科学的検査
        /// </summary>
        public const int Ganka = 16;

        /// <summary>
        /// 17: 遺伝子関連・染色体検査判断料(2020/04～追加)
        /// </summary>
        public const int Idensi = 17;

        #endregion

        #region (その他検査)
        
        /// <summary>
        /// 31: 核医学診断（Ｅ１０１－２～Ｅ１０１－５） 
        /// </summary>
        public const int Kakuigaku = 31;

        /// <summary>
        /// 32: 核医学診断（それ以外） 33: コンピューター断層診断
        /// </summary>
        public const int KakuigakuSonota = 32;

        #endregion

        #region (病理検査)

        /// <summary>
        /// 40: 病理診断 ※
        /// ※40: 病理診断は41: 病理診断（組織診断）、42: 病理診断（細胞診断）を含む。
        /// </summary>
        public const int Byori = 40;

        /// <summary>
        /// 41: 病理診断（組織診断）
        /// </summary>
        public const int ByoriSosiki = 41;

        /// <summary>
        /// 42: 病理診断（細胞診断）
        /// </summary>
        public const int ByoriSaibo = 42;

        #endregion

        /// <summary>
        /// 検査等実施判断グループ区分と、判断料、取消ダミーの組み合わせリスト
        /// </summary>
        public static List<(int[] kbn, string santeiItem, string cancelItem)> KensaHandanList =
        new List<(int[], string, string)>
        {
            // 1. 尿・糞便等検査判断料
            (new int[]{KensaHandanConst.NyouFunben}, ItemCdConst.KensaHandanNyou, ItemCdConst.KensaHandanNyouCancel),
            // 2. 血液学的検査判断料
            (new int[]{ KensaHandanConst.Ketueki }, ItemCdConst.KensaHandanKetueki, ItemCdConst.KensaHandanKetuekiCancel),
            // 3. 生化学的検査（１）判断料
            (new int[]{ KensaHandanConst.Seika1 }, ItemCdConst.KensaHandanSeika1, ItemCdConst.KensaHandanSeika1Cancel),
            // 4. 生化学的検査（２）判断料
            (new int[]{ KensaHandanConst.Seika2 }, ItemCdConst.KensaHandanSeika2, ItemCdConst.KensaHandanSeika2Cancel),
            // 5. 免疫学的検査判断料
            (new int[]{ KensaHandanConst.Meneki }, ItemCdConst.KensaHandanMeneki, ItemCdConst.KensaHandanMenekiCancel),
            // 6. 微生物学的検査判断料
            (new int[]{ KensaHandanConst.Biseibutu }, ItemCdConst.KensaHandanBiseibutu, ItemCdConst.KensaHandanBiseibutuCancel),
            // 11 呼吸機能検査等判断料
            (new int[]{ KensaHandanConst.Kokyu }, ItemCdConst.KensaHandanKokyu, ItemCdConst.KensaHandanKokyuCancel),
            // 13 脳波検査判断料２
            (new int[]{ KensaHandanConst.Noha }, ItemCdConst.KensaHandanNoha2, ItemCdConst.KensaHandanNohaCancel),
            // 14 神経・筋検査判断料
            (new int[]{ KensaHandanConst.Sinkei }, ItemCdConst.KensaHandanSinkei, ItemCdConst.KensaHandanSinkeiCancel),
            // 15 ラジオアイソトープ検査判断料
            (new int[]{ KensaHandanConst.Radio }, ItemCdConst.KensaHandanRadio, ItemCdConst.KensaHandanRadioCancel),
            // 17 遺伝子関連・染色体検査判断料
            (new int[]{ KensaHandanConst.Idensi }, ItemCdConst.KensaHandanIdensi, ItemCdConst.KensaHandanIdensiCancel),
            // 40, 41, 42病理判断料
            (new int[]{ KensaHandanConst.Byori, KensaHandanConst.ByoriSosiki, KensaHandanConst.ByoriSaibo }, ItemCdConst.KensaHandanByori, ItemCdConst.KensaHandanByoriCancel)

        };
    }
}
